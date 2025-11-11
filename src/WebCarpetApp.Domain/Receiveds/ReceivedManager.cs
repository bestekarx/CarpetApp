using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;
using WebCarpetApp.Companies;
using WebCarpetApp.Customers;
using WebCarpetApp.Messaging;

namespace WebCarpetApp.Receiveds
{
    public class ReceivedManager(
        IRepository<Received, Guid> receivedRepository,
        IRepository<Company, Guid> companyRepository,
        MessageManager messageManager,
        IMessageSender messageSender,
        FicheNoManager ficheNoManager,
        IUnitOfWorkManager unitOfWorkManager)
        : DomainService
    {
        public async Task<Received> CreateReceivedAsync(
            Guid vehicleId,
            Guid customerId,
            string? note,
            int receivedType,
            int rowNumber,
            DateTime pickupDate,
            DateTime deliveryDate)
        {
            using var uow = unitOfWorkManager.Begin(requiresNew: true, isTransactional: true);

            try
            {
                var ficheNo = await ficheNoManager.GenerateNextFicheNoAsync();
            
                var received = new Received(
                    vehicleId,
                    customerId, 
                    ReceivedStatus.Active,
                    (ReceivedType) receivedType,
                    note, 
                    rowNumber,
                    pickupDate,
                    deliveryDate,
                    ficheNo);
                
                await receivedRepository.InsertAsync(received);

                await uow.CompleteAsync();
                return received;
            }
            catch (Exception ex)
            {
                await uow.RollbackAsync();
                throw new BusinessException(
                    WebCarpetAppDomainErrorCodes.ReceivedCreationFailed,
                    "Received creation failed: " + ex.Message);
            }
        }
        
        public async Task SendReceivedNotificationAsync(
            Received received, 
            Customer customer, 
            string cultureCode = "tr-TR")
        {
            var shouldSend = await messageManager.ShouldSendMessageAsync(
                customer.CompanyId, 
                MessageTaskType.ReceivedCreated);
            
            if (!shouldSend)
            {
                return; // SMS gönderilmeyecek
            }
            
            // 2. Onay gerekiyor mu kontrol et
            var requiresConfirmation = await messageManager.RequiresConfirmationAsync(
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
            var company = await companyRepository.GetAsync(customer.CompanyId);
            
            var values = new Dictionary<string, object>
            {
                { "CustomerName", customer.FullName },
                { "ReceivedDate", received.DeliveryDate.ToString("dd.MM.yyyy") },
                { "CarpetNumber", received.RowNumber.ToString() }, // Örnek, gerçek uygulama farklı olabilir
                { "CompanyPhone", company.Name }, // Şirket telefonu için gerçek uygulamada ilgili alan kullanılmalı
                { "FicheNo", received.FicheNo } // FicheNo'yu da mesaj değeri olarak ekleyelim
            };
            
            // 4. Formatlanmış mesajı oluştur ve gönder
            var message = await messageManager.FormatMessageAsync(
                customer.CompanyId,
                MessageTaskType.ReceivedCreated,
                values,
                cultureCode);
                
            if (!string.IsNullOrEmpty(message))
            {
                // 5. MessageUser bilgilerini al ve SMS gönder
                var messageUserId = await messageManager.GetMessageUserIdAsync(customer.CompanyId);
                if (messageUserId.HasValue)
                {
                    await messageSender.SendMessageAsync(customer.Phone, message, messageUserId.Value);
                }
            }
        }
        public async Task<bool> ReorderReceivedItemsAsync(List<Guid> orderedIds)
        {
            var receivedItems = await receivedRepository.GetListAsync(x => orderedIds.Contains(x.Id));
        
            if (receivedItems.Count != orderedIds.Count)
            {
                throw new BusinessException(
                    WebCarpetAppDomainErrorCodes.EntityNotFound,
                    "Some of the received items were not found."
                );
            }

            var itemsDictionary = receivedItems.ToDictionary(x => x.Id);

            for (int i = 0; i < orderedIds.Count; i++)
            {
                var item = itemsDictionary[orderedIds[i]];
                item.UpdateRowNumber(i);
            }

            await receivedRepository.UpdateManyAsync(receivedItems);
            return true;
        }

        public async Task<bool> CancelReceivedAsync(Guid id)
        {
            var received = await receivedRepository.GetAsync(id);
        
            if (received == null)
            {
                throw new BusinessException(
                    WebCarpetAppDomainErrorCodes.EntityNotFound,
                    "Received item not found."
                );
            }

            received.CancelReceive();
            await receivedRepository.UpdateAsync(received);
        
            return true;
        }

        public async Task<bool> UpdateVehicleReceivedAsync(Guid id, Guid vehicleId)
        {
            var received = await receivedRepository.GetAsync(id);
        
            if (received == null)
            {
                throw new BusinessException(
                    WebCarpetAppDomainErrorCodes.EntityNotFound,
                    "Received item not found."
                );
            }

            received.UpdateVehicle(vehicleId);
            await receivedRepository.UpdateAsync(received);
        
            return true;
        }
        public async Task<bool> UpdateNoteReceivedAsync(Guid id, string note)
        {
            var received = await receivedRepository.GetAsync(id);
        
            if (received == null)
            {
                throw new BusinessException(
                    WebCarpetAppDomainErrorCodes.EntityNotFound,
                    "Received item not found."
                );
            }

            received.UpdateNote(note);
            await receivedRepository.UpdateAsync(received);
        
            return true;
        }
    }
}