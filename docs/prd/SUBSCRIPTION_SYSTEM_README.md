# 🚀 WebCarpetApp - Subscription System

## 📋 OVERVIEW

Professional SaaS multi-tenant subscription management system for carpet cleaning businesses. Built with ABP Framework, .NET 9, and SQL Server.

## ✨ FEATURES

### 🎯 Core Subscription Features
- ✅ **7-Day Free Trial** - Automatic trial creation for new tenants
- ✅ **Multi-Tier Pricing** - 4 subscription plans (Trial, Starter, Professional, Enterprise)
- ✅ **User Limit Enforcement** - Per-plan user limitations
- ✅ **Team Management** - User invitation system with role-based access
- ✅ **Subscription Validation** - Middleware-based access control
- ✅ **Payment Integration Ready** - Hooks for Stripe, PayPal, etc.
- ✅ **Audit Trail** - Complete subscription history tracking

### 🏢 Multi-Tenant Architecture
- ✅ **Tenant Isolation** - Complete data separation
- ✅ **Owner Management** - Primary and secondary owners
- ✅ **User Invitations** - Secure token-based invitations
- ✅ **Role-Based Access** - Flexible permission system

## 💳 SUBSCRIPTION PLANS

| Plan | Users | Price/Month | Features | Plan ID |
|------|-------|-------------|----------|---------|
| **Trial** | 2 | $0.00 | 7 days, basic features | `11111111-1111-1111-1111-111111111111` |
| **Starter** | 5 | $29.99 | Small business, basic reports | `22222222-2222-2222-2222-222222222222` |
| **Professional** | 25 | $99.99 | Growing business, advanced reports, API access | `33333333-3333-3333-3333-333333333333` |
| **Enterprise** | 100 | $299.99 | Large organizations, all features, priority support | `44444444-4444-4444-4444-444444444444` |

## 🏗️ ARCHITECTURE

### Database Tables
- **SubscriptionPlans** - Available subscription tiers
- **TenantSubscriptions** - Current tenant subscription status
- **TenantOwners** - Tenant ownership mapping
- **UserInvitations** - User invitation management
- **SubscriptionHistories** - Audit trail for all subscription changes

### Key Components
- **SubscriptionManager** - Core subscription business logic
- **UserInvitationManager** - Team invitation workflows
- **TenantOwnerManager** - Ownership management
- **SubscriptionValidationMiddleware** - Request-level access control

## 🔌 API ENDPOINTS

### Authentication
- `POST /api/account/login` - Enhanced login with subscription info
- `POST /api/account/register-with-trial` - Register tenant with 7-day trial
- `GET /api/account/login-status` - Current authentication status

### Subscription Management
- `GET /api/account/subscriptions/plans` - Available subscription plans
- `GET /api/account/subscriptions/my-subscription` - Current subscription details
- `PUT /api/account/subscriptions/upgrade` - Upgrade to paid plan
- `GET /api/account/subscriptions/usage` - Usage statistics and limits

### Team Management
- `GET /api/account/team/members` - Current team members
- `POST /api/account/team/invite` - Invite new team member
- `POST /api/account/team/accept-invitation` - Accept invitation
- `DELETE /api/account/team/members/{userId}` - Remove team member

## 🚀 QUICK START

### 1. Database Setup
```sql
-- Verify subscription plans exist
SELECT Name, DisplayName, MaxUserCount, Price FROM SubscriptionPlans;
```

### 2. Register New Company with Trial
```bash
curl -X POST "https://localhost:5001/api/account/register-with-trial" \
  -H "Content-Type: application/json" \
  -d '{
    "tenantName": "Test Halı A.Ş.",
    "ownerEmail": "owner@testhali.com",
    "ownerName": "Test Owner",
    "password": "TestPass123!",
    "tenantDescription": "Test company"
  }'
```

### 3. Login and Get Token
```bash
curl -X POST "https://localhost:5001/api/account/login" \
  -H "Content-Type: application/json" \
  -d '{
    "userNameOrEmailAddress": "owner@testhali.com",
    "password": "TestPass123!"
  }'
```

### 4. Check Subscription Status
```bash
curl -X GET "https://localhost:5001/api/account/subscriptions/my-subscription" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

## 📁 FILES OVERVIEW

### Postman Collections
- `WebCarpetApp_Subscription_APIs.postman_collection.json` - Complete API collection
- `WebCarpetApp_Development.postman_environment.json` - Development environment
- `API_Testing_Guide.md` - Comprehensive testing scenarios

### Domain Layer
- `src/WebCarpetApp.Domain/Subscriptions/` - Domain entities and services
- `src/WebCarpetApp.EntityFrameworkCore/` - EF Core configurations

### Application Layer
- `src/WebCarpetApp.Application/Subscriptions/` - Application services
- `src/WebCarpetApp.Application.Contracts/Subscriptions/` - DTOs and interfaces

### API Layer
- `src/WebCarpetApp.HttpApi.Host/Controllers/Subscriptions/` - REST controllers
- `src/WebCarpetApp.HttpApi.Host/Authentication/` - Middleware and authentication

## 🔐 SECURITY FEATURES

### Subscription Validation
- **Middleware-based** access control
- **JWT token** validation with subscription claims
- **Automatic trial expiration** handling
- **User limit enforcement** per request

### Multi-Tenant Security
- **Tenant isolation** at database level
- **Owner-only permissions** for sensitive operations
- **Secure invitation tokens** (64-character random)
- **Role-based access control**

## 💰 PAYMENT INTEGRATION

### Ready for Integration
The system is designed to integrate with popular payment providers:

```csharp
// Example: Stripe integration
public async Task<TenantSubscriptionDto> UpgradeSubscriptionAsync(UpgradeSubscriptionDto input)
{
    // 1. Process payment with Stripe
    var paymentResult = await _stripeService.ProcessPaymentAsync(input);

    // 2. Update subscription if payment successful
    if (paymentResult.Success)
    {
        return await _subscriptionManager.UpgradeSubscriptionAsync(
            tenantId,
            input.SubscriptionPlanId,
            paymentResult.TransactionId
        );
    }
}
```

### Supported Payment Providers
- **Stripe** - Credit cards, digital wallets
- **PayPal** - PayPal accounts, credit cards
- **Square** - In-person and online payments
- **RevenueCat** - Subscription management (recommended)

## 📈 BUSINESS LOGIC

### Trial Management
- **Automatic trial creation** on registration
- **7-day trial period** with full functionality
- **Trial expiration warnings** (3 days before)
- **Seamless upgrade** to paid plans

### User Limit Enforcement
- **Real-time validation** before user actions
- **Invitation blocking** when limit reached
- **Graceful degradation** with clear error messages
- **Automatic limit updates** on plan changes

### Subscription History
- **Complete audit trail** of all subscription changes
- **Payment transaction tracking**
- **Plan upgrade/downgrade history**
- **User action logging** with timestamps

## 🎯 PRODUCTION READINESS

### ✅ Completed Features
- [x] Multi-tenant database structure
- [x] Subscription plan management
- [x] User invitation system
- [x] Payment integration hooks
- [x] Access control middleware
- [x] Complete API endpoints
- [x] Audit trail logging

### 🔄 Recommended Next Steps
- [ ] Email notification system
- [ ] Payment webhook handlers
- [ ] Subscription analytics dashboard
- [ ] Mobile app integration
- [ ] Advanced reporting features

## 📞 SUPPORT

For questions about the subscription system:
1. Check the `API_Testing_Guide.md` for testing scenarios
2. Review the Postman collection for API examples
3. Examine the domain services for business logic details

---

**🎉 Congratulations! Your SaaS subscription system is ready for production!** 🚀