using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WebCarpetApp.Customers.Dtos;

namespace WebCarpetApp.Customers;

public class CustomerVerificationAppService :
CrudAppService<
        CustomerVerification,
        CustomerVerificationDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateCustomerVerificationDto,
        CreateUpdateCustomerVerificationDto>,
    ICustomerVerificationAppService
{
    private readonly IRepository<CustomerVerification, Guid> _verificationRepository;
    //private readonly IMessageService _messageService;

    //IMessageService messageService ctor
    public CustomerVerificationAppService(IRepository<CustomerVerification, Guid> verificationRepository)
        : base(verificationRepository)
    {
        _verificationRepository = verificationRepository;
        //_messageService = messageService;
    }

    public async Task SendVerificationCodeAsync(Guid customerId)
    {
        var verificationCode = new Random().Next(100000, 999999).ToString();

        var verification = new CustomerVerification
        {
            CustomerId = customerId,
            VerificationCode = verificationCode,
            ExpirationTime = Clock.Now.AddMinutes(5),
            IsUsed = false
        };

        await _verificationRepository.InsertAsync(verification);

        //await _messageService.SendSmsAsync("Telefon numarası", $"Onay kodunuz: {verificationCode}");
    }
    
    public async Task<bool> VerifyCustomerAsync(Guid customerId, string verificationCode)
    {
        var verification = await _verificationRepository.FirstOrDefaultAsync(v =>
            v.CustomerId == customerId &&
            v.VerificationCode == verificationCode &&
            v.ExpirationTime > Clock.Now &&
            !v.IsUsed);

        if (verification == null)
        {
            return false;  // Kod geçersiz veya süresi dolmuş
        }

        // Kod doğrulandı
        verification.IsUsed = true;
        await _verificationRepository.UpdateAsync(verification);

        // Customer'ı onaylı hale getir
        /*var customer = await _customerRepository.GetAsync(customerId);
        customer.IsConfirmed = true;
        customer.ConfirmedAt = Clock.Now;

        await _customerRepository.UpdateAsync(customer);
        */
        return true;
    }

}