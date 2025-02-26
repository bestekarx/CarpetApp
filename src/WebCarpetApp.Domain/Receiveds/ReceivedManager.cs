using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using WebCarpetApp.Companies;
using WebCarpetApp.Customers;
using WebCarpetApp.Messaging;
using WebCarpetApp.Vehicles;

namespace WebCarpetApp.Receiveds
{
    public class ReceivedManager : DomainService
    {
        private readonly IRepository<Received, Guid> _receivedRepository;
        private readonly IRepository<Customer, Guid> _customerRepository;
        private readonly IRepository<Vehicle, Guid> _vehicleRepository;
        private readonly IRepository<Company, Guid> _companyRepository;
        private readonly MessageManager _messageManager;
        private readonly IMessageSender _messageSender;
        
        public ReceivedManager(
            IRepository<Received, Guid> receivedRepository,
            IRepository<Customer, Guid> customerRepository,
            IRepository<Vehicle, Guid> vehicleRepository,
            IRepository<Company, Guid> companyRepository,
            MessageManager messageManager,
            IMessageSender messageSender)
        {
            _receivedRepository = receivedRepository;
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
            _companyRepository = companyRepository;
            _messageManager = messageManager;
            _messageSender = messageSender;
        }
        
        public async Task<Received> CreateReceivedAsync(
            Guid vehicleId,
            Guid customerId,
            string note,
            int rowNumber,
            DateTime? purchaseDate = null,
            bool sendSms = true,
            string cultureCode = "tr-TR")
        {
            // 1. Müşteri ve araç bilgilerini doğrula
            var customer = await _customerRepository.GetAsync(customerId);
            var vehicle = await _vehicleRepository.GetAsync(vehicleId);
            
            // 2. Received kaydını oluştur
            var received = new Received(
                vehicleId,
                customerId, 
                ReceivedStatus.Active,
                note, 
                rowNumber,
                purchaseDate ?? DateTime.Now,
                DateTime.Now);
            
            await _receivedRepository.InsertAsync(received);
            
            // 3. SMS gönderimi gerekiyorsa işle
            if (sendSms)
            {
                await SendReceivedNotificationAsync(received, customer, cultureCode);
            }
            
            return received;
        }
        
        public async Task SendReceivedNotificationAsync(
            Received received, 
            Customer customer, 
            string cultureCode = "tr-TR")
        {
            // 1. Bu şirket için SMS gönderilmeli mi kontrol et
            var shouldSend = await _messageManager.ShouldSendMessageAsync(
                customer.CompanyId, 
                MessageTaskType.ReceivedCreated);
            
            if (!shouldSend)
            {
                return; // SMS gönderilmeyecek
            }
            
            // 2. Onay gerekiyor mu kontrol et
            var requiresConfirmation = await _messageManager.RequiresConfirmationAsync(
                customer.CompanyId, 
                MessageTaskType.ReceivedCreated);
            
            if (requiresConfirmation)
            {
                // Bu noktada UI'da onay sormak gerekiyor
                // Bu domain service'de onay sorma imkanı olmadığından,
                // onay gerekiyorsa işlemi burada sonlandırıp, bu bilgiyi döndürmek daha doğru olur
                // Onay durumu uygulama katmanında değerlendirilebilir
                return;
            }
            
            // 3. Mesaj şablonunu formatlamak için değerleri hazırla
            var company = await _companyRepository.GetAsync(customer.CompanyId);
            
            var values = new Dictionary<string, object>
            {
                { "CustomerName", customer.FullName },
                { "ReceivedDate", received.ReceivedDate.ToString("dd.MM.yyyy") },
                { "CarpetNumber", received.RowNumber.ToString() }, // Örnek, gerçek uygulama farklı olabilir
                { "CompanyPhone", company.Name } // Şirket telefonu için gerçek uygulamada ilgili alan kullanılmalı
            };
            
            // 4. Formatlanmış mesajı oluştur ve gönder
            var message = await _messageManager.FormatMessageAsync(
                customer.CompanyId,
                MessageTaskType.ReceivedCreated,
                values,
                cultureCode);
                
            if (!string.IsNullOrEmpty(message))
            {
                // 5. MessageUser bilgilerini al ve SMS gönder
                var messageUserId = await _messageManager.GetMessageUserIdAsync(customer.CompanyId);
                if (messageUserId.HasValue)
                {
                    await _messageSender.SendMessageAsync(customer.Phone, message, messageUserId.Value);
                }
            }
        }
        public async Task ReorderReceivedItemsAsync(List<Guid> orderedIds)
        {
            var receivedItems = await _receivedRepository.GetListAsync(x => orderedIds.Contains(x.Id));
        
            // Validate that all items exist
            if (receivedItems.Count != orderedIds.Count)
            {
                throw new BusinessException(
                    WebCarpetAppDomainErrorCodes.EntityNotFound,
                    "Some of the received items were not found."
                );
            }

            // Create a dictionary for quick lookup of items
            var itemsDictionary = receivedItems.ToDictionary(x => x.Id);

            // Update row numbers based on the new order
            for (int i = 0; i < orderedIds.Count; i++)
            {
                var item = itemsDictionary[orderedIds[i]];
                item.UpdateRowNumber(i);
            }

            await _receivedRepository.UpdateManyAsync(receivedItems);
        }

        public async Task<Received> CancelReceivedAsync(Guid id)
        {
            var received = await _receivedRepository.GetAsync(id);
        
            if (received == null)
            {
                throw new BusinessException(
                    WebCarpetAppDomainErrorCodes.EntityNotFound,
                    "Received item not found."
                );
            }

            received.CancelReceive();
            await _receivedRepository.UpdateAsync(received);
        
            return received;
        }
    }
}