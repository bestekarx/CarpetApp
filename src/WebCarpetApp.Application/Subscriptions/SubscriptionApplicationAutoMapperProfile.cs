using AutoMapper;
using WebCarpetApp.Subscriptions;

namespace WebCarpetApp.Subscriptions;

public class SubscriptionApplicationAutoMapperProfile : Profile
{
    public SubscriptionApplicationAutoMapperProfile()
    {
        CreateMap<SubscriptionPlan, SubscriptionPlanDto>();
        CreateMap<TenantSubscription, TenantSubscriptionDto>();
        CreateMap<UserInvitation, UserInvitationDto>();
        CreateMap<TenantOwner, TenantOwnerDto>();
        CreateMap<SubscriptionHistory, SubscriptionHistoryDto>();
    }
}