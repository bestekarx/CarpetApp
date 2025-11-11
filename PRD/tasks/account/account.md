# WebCarpetApp - Subscription-Based Account Management System

## 🎯 PROJE HEDEFİ

Kullanıcıların abonelik satın alarak kendi tenant'larını oluşturabileceği ve alt kullanıcılar ekleyebileceği profesyonel SaaS platformu.

### İş Senaryosu Örneği:
1. **Ahmet Bey** (Halı Yıkama İşletme Sahibi) → **5 User Premium Plan** satın alır
2. **Ahmet Bey** otomatik olarak **Tenant Owner** olur ve **Tenant** oluşturulur
3. **Ahmet Bey** çalışanları için 4 alt kullanıcı oluşturur:
   - Fatma (Müşteri Hizmetleri)
   - Mehmet (Saha Çalışanı)
   - Ayşe (Muhasebe)
   - Ali (Operasyon Yöneticisi)
4. Bu 5 kişi aynı tenant altında çalışır, subscription durumu kontrol edilir

## 📊 MEVCUT ABP YAPISI ANALİZİ

### ✅ Mevcut Tablolar:
- **AbpUsers**: Kullanıcı bilgileri
- **AbpTenants**: Tenant bilgileri
- **AbpUserRoles**: Kullanıcı rolleri
- **UserTenantMappings**: Custom user-tenant ilişkisi (mevcut)

### ⚠️ Eksik Yapılar:
- **Subscription Management**: Abonelik yönetimi
- **Tenant Ownership**: Tenant sahipliği
- **Plan Management**: Abonelik planları
- **User Limits**: Kullanıcı limitlerı
- **Trial System**: Deneme sürümü

## 🗃️ YENİ VERİTABANI ŞEMASI

### 1. SubscriptionPlans (Abonelik Planları)
```sql
CREATE TABLE SubscriptionPlans (
    Id uniqueidentifier PRIMARY KEY,
    Name nvarchar(100) NOT NULL, -- "Starter", "Professional", "Enterprise"
    DisplayName nvarchar(200) NOT NULL,
    Description nvarchar(500),
    MaxUserCount int NOT NULL, -- 5, 25, 100
    Price decimal(18,2) NOT NULL, -- Aylık fiyat
    Currency nvarchar(3) DEFAULT 'USD', -- USD, TRY, EUR
    BillingPeriod int NOT NULL, -- 1=Monthly, 12=Yearly
    Features nvarchar(max), -- JSON format: ["advanced_reports", "api_access"]
    IsActive bit DEFAULT 1,
    IsPopular bit DEFAULT 0, -- Popular badge
    CreationTime datetime2 NOT NULL,
    LastModificationTime datetime2
);

-- Sample Data
INSERT INTO SubscriptionPlans VALUES
(NEWID(), 'trial', '7-Day Trial', 'Free trial for 7 days', 2, 0.00, 'USD', 1, '["basic_features"]', 1, 0, GETDATE(), NULL),
(NEWID(), 'starter', 'Starter Plan', 'Perfect for small businesses', 5, 29.99, 'USD', 1, '["basic_reports", "5_users"]', 1, 0, GETDATE(), NULL),
(NEWID(), 'professional', 'Professional Plan', 'Growing businesses', 25, 99.99, 'USD', 1, '["advanced_reports", "api_access", "25_users"]', 1, 1, GETDATE(), NULL),
(NEWID(), 'enterprise', 'Enterprise Plan', 'Large organizations', 100, 299.99, 'USD', 1, '["all_features", "priority_support", "100_users"]', 1, 0, GETDATE(), NULL);
```

### 2. TenantSubscriptions (Tenant Abonelikleri)
```sql
CREATE TABLE TenantSubscriptions (
    Id uniqueidentifier PRIMARY KEY,
    TenantId uniqueidentifier NOT NULL,
    SubscriptionPlanId uniqueidentifier NOT NULL,
    OwnerUserId uniqueidentifier NOT NULL, -- Aboneliği satın alan kullanıcı
    Status int NOT NULL, -- 0=Active, 1=Expired, 2=Cancelled, 3=Suspended, 4=Trial
    StartDate datetime2 NOT NULL,
    EndDate datetime2 NOT NULL,
    IsAutoRenew bit DEFAULT 1,
    CurrentUserCount int DEFAULT 0, -- Mevcut kullanıcı sayısı
    MaxUserCount int NOT NULL, -- Plan limiti
    PaymentStatus int NOT NULL, -- 0=Pending, 1=Paid, 2=Failed, 3=Refunded
    ExternalSubscriptionId nvarchar(255), -- RevenueCat/Stripe subscription ID
    ExternalCustomerId nvarchar(255), -- External customer ID
    CreationTime datetime2 NOT NULL,
    LastModificationTime datetime2,

    FOREIGN KEY (TenantId) REFERENCES AbpTenants(Id),
    FOREIGN KEY (SubscriptionPlanId) REFERENCES SubscriptionPlans(Id),
    FOREIGN KEY (OwnerUserId) REFERENCES AbpUsers(Id)
);
```

### 3. TenantOwners (Tenant Sahipleri)
```sql
CREATE TABLE TenantOwners (
    Id uniqueidentifier PRIMARY KEY,
    TenantId uniqueidentifier NOT NULL,
    UserId uniqueidentifier NOT NULL,
    IsFounder bit DEFAULT 1, -- İlk kuran kişi
    OwnershipStartDate datetime2 NOT NULL,
    OwnershipEndDate datetime2, -- Transfer durumunda
    IsActive bit DEFAULT 1,
    CreationTime datetime2 NOT NULL,

    FOREIGN KEY (TenantId) REFERENCES AbpTenants(Id),
    FOREIGN KEY (UserId) REFERENCES AbpUsers(Id),
    UNIQUE(TenantId, UserId) -- Bir kullanıcı aynı tenant'ta birden fazla owner olamaz
);
```

### 4. UserInvitations (Kullanıcı Davetleri)
```sql
CREATE TABLE UserInvitations (
    Id uniqueidentifier PRIMARY KEY,
    TenantId uniqueidentifier NOT NULL,
    InviterUserId uniqueidentifier NOT NULL, -- Davet eden (genelde owner)
    Email nvarchar(320) NOT NULL,
    InvitationToken nvarchar(500) NOT NULL, -- Unique invitation token
    Status int NOT NULL, -- 0=Pending, 1=Accepted, 2=Expired, 3=Cancelled
    ExpirationDate datetime2 NOT NULL,
    AcceptedDate datetime2,
    AcceptedUserId uniqueidentifier, -- Daveti kabul eden user
    RoleNames nvarchar(500), -- JSON array: ["Admin", "User"]
    PersonalMessage nvarchar(1000), -- Kişisel mesaj
    CreationTime datetime2 NOT NULL,

    FOREIGN KEY (TenantId) REFERENCES AbpTenants(Id),
    FOREIGN KEY (InviterUserId) REFERENCES AbpUsers(Id),
    FOREIGN KEY (AcceptedUserId) REFERENCES AbpUsers(Id)
);
```

### 5. SubscriptionHistory (Abonelik Geçmişi)
```sql
CREATE TABLE SubscriptionHistory (
    Id uniqueidentifier PRIMARY KEY,
    TenantId uniqueidentifier NOT NULL,
    SubscriptionPlanId uniqueidentifier NOT NULL,
    UserId uniqueidentifier NOT NULL,
    Action int NOT NULL, -- 0=Created, 1=Renewed, 2=Upgraded, 3=Downgraded, 4=Cancelled
    PreviousPlanId uniqueidentifier, -- Upgrade/downgrade durumunda
    Amount decimal(18,2) NOT NULL,
    Currency nvarchar(3) NOT NULL,
    PaymentMethod nvarchar(50), -- "credit_card", "bank_transfer", "paypal"
    ExternalTransactionId nvarchar(255),
    Notes nvarchar(1000),
    CreationTime datetime2 NOT NULL,

    FOREIGN KEY (TenantId) REFERENCES AbpTenants(Id),
    FOREIGN KEY (SubscriptionPlanId) REFERENCES SubscriptionPlans(Id),
    FOREIGN KEY (PreviousPlanId) REFERENCES SubscriptionPlans(Id),
    FOREIGN KEY (UserId) REFERENCES AbpUsers(Id)
);
```

## 🔧 ENTITY TASARIMLARI

### SubscriptionPlan Entity
```csharp
public class SubscriptionPlan : AuditedEntity<Guid>
{
    public string Name { get; set; } // starter, professional, enterprise
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public int MaxUserCount { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public BillingPeriod BillingPeriod { get; set; }
    public string Features { get; set; } // JSON serialized List<string>
    public bool IsActive { get; set; } = true;
    public bool IsPopular { get; set; } = false;

    // Helper methods
    public List<string> GetFeatures() => JsonSerializer.Deserialize<List<string>>(Features ?? "[]");
    public void SetFeatures(List<string> features) => Features = JsonSerializer.Serialize(features);
}

public enum BillingPeriod
{
    Monthly = 1,
    Quarterly = 3,
    SemiAnnually = 6,
    Yearly = 12
}
```

### TenantSubscription Entity
```csharp
public class TenantSubscription : AuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid SubscriptionPlanId { get; set; }
    public Guid OwnerUserId { get; set; }
    public SubscriptionStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsAutoRenew { get; set; } = true;
    public int CurrentUserCount { get; set; }
    public int MaxUserCount { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string ExternalSubscriptionId { get; set; }
    public string ExternalCustomerId { get; set; }

    // Navigation properties
    public virtual SubscriptionPlan SubscriptionPlan { get; set; }
    public virtual IdentityUser OwnerUser { get; set; }

    // Helper methods
    public bool IsActive => Status == SubscriptionStatus.Active && DateTime.UtcNow <= EndDate;
    public bool IsExpired => DateTime.UtcNow > EndDate;
    public bool CanAddMoreUsers => CurrentUserCount < MaxUserCount;
    public int RemainingUserSlots => MaxUserCount - CurrentUserCount;
    public TimeSpan RemainingTime => EndDate - DateTime.UtcNow;
}

public enum SubscriptionStatus
{
    Active = 0,
    Expired = 1,
    Cancelled = 2,
    Suspended = 3,
    Trial = 4
}

public enum PaymentStatus
{
    Pending = 0,
    Paid = 1,
    Failed = 2,
    Refunded = 3
}
```

### TenantOwner Entity
```csharp
public class TenantOwner : CreationAuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid UserId { get; set; }
    public bool IsFounder { get; set; } = true;
    public DateTime OwnershipStartDate { get; set; }
    public DateTime? OwnershipEndDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual IdentityUser User { get; set; }
}
```

## 🚀 DOMAIN SERVICES

### SubscriptionManager
```csharp
public class SubscriptionManager : DomainService
{
    private readonly IRepository<TenantSubscription, Guid> _subscriptionRepository;
    private readonly IRepository<SubscriptionPlan, Guid> _planRepository;
    private readonly IRepository<TenantOwner, Guid> _tenantOwnerRepository;
    private readonly ITenantManager _tenantManager;

    // Subscription Operations
    public async Task<TenantSubscription> CreateTrialSubscriptionAsync(Guid userId, string tenantName)
    {
        var trialPlan = await GetTrialPlanAsync();
        var tenant = await CreateTenantAsync(tenantName, userId);

        var subscription = new TenantSubscription
        {
            TenantId = tenant.Id,
            SubscriptionPlanId = trialPlan.Id,
            OwnerUserId = userId,
            Status = SubscriptionStatus.Trial,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(7), // 7-day trial
            MaxUserCount = trialPlan.MaxUserCount,
            CurrentUserCount = 1
        };

        return await _subscriptionRepository.InsertAsync(subscription);
    }

    public async Task<bool> CanUserLoginAsync(Guid userId, Guid? tenantId)
    {
        if (tenantId == null) return true; // Host user

        var subscription = await GetActiveSubscriptionAsync(tenantId.Value);
        return subscription != null && subscription.IsActive;
    }

    public async Task<bool> CanAddUserToTenantAsync(Guid tenantId)
    {
        var subscription = await GetActiveSubscriptionAsync(tenantId);
        return subscription?.CanAddMoreUsers ?? false;
    }

    public async Task UpgradeSubscriptionAsync(Guid tenantId, Guid newPlanId, string externalSubscriptionId)
    {
        var subscription = await GetActiveSubscriptionAsync(tenantId);
        var newPlan = await _planRepository.GetAsync(newPlanId);

        // Create history record
        await CreateSubscriptionHistoryAsync(subscription, SubscriptionAction.Upgraded, newPlan);

        // Update subscription
        subscription.SubscriptionPlanId = newPlanId;
        subscription.MaxUserCount = newPlan.MaxUserCount;
        subscription.ExternalSubscriptionId = externalSubscriptionId;
        subscription.PaymentStatus = PaymentStatus.Paid;

        await _subscriptionRepository.UpdateAsync(subscription);
    }
}
```

### UserInvitationManager
```csharp
public class UserInvitationManager : DomainService
{
    public async Task<UserInvitation> InviteUserAsync(
        Guid tenantId,
        Guid inviterUserId,
        string email,
        List<string> roleNames,
        string personalMessage = null)
    {
        // Check subscription limits
        var canAddUser = await _subscriptionManager.CanAddUserToTenantAsync(tenantId);
        if (!canAddUser)
        {
            throw new BusinessException("SUBSCRIPTION_USER_LIMIT_EXCEEDED");
        }

        var invitation = new UserInvitation
        {
            TenantId = tenantId,
            InviterUserId = inviterUserId,
            Email = email,
            InvitationToken = GenerateInvitationToken(),
            Status = InvitationStatus.Pending,
            ExpirationDate = DateTime.UtcNow.AddDays(7),
            RoleNames = JsonSerializer.Serialize(roleNames),
            PersonalMessage = personalMessage
        };

        await _invitationRepository.InsertAsync(invitation);

        // Send invitation email
        await _emailSender.SendInvitationEmailAsync(invitation);

        return invitation;
    }

    public async Task<IdentityUser> AcceptInvitationAsync(string token, RegisterDto registerDto)
    {
        var invitation = await GetInvitationByTokenAsync(token);

        // Validate invitation
        if (invitation.Status != InvitationStatus.Pending)
            throw new BusinessException("INVITATION_ALREADY_PROCESSED");

        if (invitation.ExpirationDate < DateTime.UtcNow)
            throw new BusinessException("INVITATION_EXPIRED");

        // Create user
        var user = await _userManager.CreateUserAsync(registerDto, invitation.TenantId);

        // Assign roles
        var roleNames = JsonSerializer.Deserialize<List<string>>(invitation.RoleNames);
        await _userManager.AddToRolesAsync(user, roleNames);

        // Update invitation
        invitation.Status = InvitationStatus.Accepted;
        invitation.AcceptedDate = DateTime.UtcNow;
        invitation.AcceptedUserId = user.Id;

        // Update subscription user count
        await _subscriptionManager.IncrementUserCountAsync(invitation.TenantId);

        return user;
    }
}
```

## 🔐 YETKİLENDİRME SİSTEMİ

### Custom Authorization Handler
```csharp
public class SubscriptionAuthorizationHandler : AuthorizationHandler<SubscriptionRequirement>
{
    private readonly ISubscriptionManager _subscriptionManager;
    private readonly ICurrentTenant _currentTenant;

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        SubscriptionRequirement requirement)
    {
        if (_currentTenant.Id == null)
        {
            context.Succeed(requirement); // Host user
            return;
        }

        var canAccess = await _subscriptionManager.CanUserLoginAsync(
            context.User.GetUserId(),
            _currentTenant.Id);

        if (canAccess)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}

public class SubscriptionRequirement : IAuthorizationRequirement
{
    public SubscriptionRequirement() { }
}
```

### Login Interceptor
```csharp
public class SubscriptionLoginInterceptor : ITransientDependency
{
    public async Task<bool> ValidateLoginAsync(Guid userId, Guid? tenantId)
    {
        // Host users always allowed
        if (tenantId == null) return true;

        var subscription = await _subscriptionManager.GetActiveSubscriptionAsync(tenantId.Value);

        if (subscription == null)
        {
            throw new UserFriendlyException("No active subscription found for this tenant.");
        }

        if (!subscription.IsActive)
        {
            if (subscription.Status == SubscriptionStatus.Expired)
            {
                throw new UserFriendlyException("Your subscription has expired. Please renew to continue.");
            }
            else if (subscription.Status == SubscriptionStatus.Suspended)
            {
                throw new UserFriendlyException("Your account has been suspended. Please contact support.");
            }
            else
            {
                throw new UserFriendlyException("Your subscription is not active.");
            }
        }

        return true;
    }
}
```

## 🌐 API ENDPOINTS PLANI

### Account Management Endpoints

#### POST /api/account/register-with-trial
```json
{
  "email": "ahmet@example.com",
  "password": "StrongPass123!",
  "confirmPassword": "StrongPass123!",
  "firstName": "Ahmet",
  "lastName": "Yılmaz",
  "companyName": "Ahmet Halı Yıkama",
  "phoneNumber": "+905551234567"
}

Response:
{
  "success": true,
  "userId": "guid",
  "tenantId": "guid",
  "tenantName": "ahmet-hali-yikama",
  "subscriptionId": "guid",
  "trialEndDate": "2025-01-11T00:00:00Z",
  "accessToken": "jwt-token",
  "refreshToken": "refresh-token"
}
```

#### POST /api/account/login
```json
{
  "emailOrUsername": "ahmet@example.com",
  "password": "StrongPass123!",
  "tenantName": "ahmet-hali-yikama", // Optional - subdomain'den resolve edilebilir
  "rememberMe": true
}

Response:
{
  "success": true,
  "userId": "guid",
  "tenantId": "guid",
  "tenantName": "ahmet-hali-yikama",
  "subscriptionStatus": "Active",
  "subscriptionEndDate": "2025-02-01T00:00:00Z",
  "isOwner": true,
  "accessToken": "jwt-token",
  "refreshToken": "refresh-token"
}
```

### Subscription Management Endpoints

#### GET /api/account/subscription-plans
```json
[
  {
    "id": "guid",
    "name": "trial",
    "displayName": "7-Day Trial",
    "description": "Free trial for 7 days",
    "maxUserCount": 2,
    "price": 0.00,
    "currency": "USD",
    "billingPeriod": 1,
    "features": ["basic_features"],
    "isPopular": false
  },
  {
    "id": "guid",
    "name": "professional",
    "displayName": "Professional Plan",
    "description": "Growing businesses",
    "maxUserCount": 25,
    "price": 99.99,
    "currency": "USD",
    "billingPeriod": 1,
    "features": ["advanced_reports", "api_access", "25_users"],
    "isPopular": true
  }
]
```

#### GET /api/account/my-subscription
```json
{
  "id": "guid",
  "planName": "Professional Plan",
  "status": "Active",
  "startDate": "2025-01-01T00:00:00Z",
  "endDate": "2025-02-01T00:00:00Z",
  "isAutoRenew": true,
  "currentUserCount": 8,
  "maxUserCount": 25,
  "remainingDays": 28,
  "canAddMoreUsers": true,
  "remainingUserSlots": 17,
  "paymentStatus": "Paid",
  "nextBillingDate": "2025-02-01T00:00:00Z",
  "nextBillingAmount": 99.99
}
```

#### POST /api/account/upgrade-subscription
```json
{
  "newPlanId": "guid",
  "paymentMethodId": "pm_1234567890", // Stripe/RevenueCat payment method
  "prorationMode": "immediate" // immediate, next_billing_cycle
}
```

### Team Management Endpoints

#### GET /api/account/team-members
```json
[
  {
    "userId": "guid",
    "email": "fatma@example.com",
    "fullName": "Fatma Demir",
    "roles": ["Customer Service"],
    "isActive": true,
    "lastLoginDate": "2025-01-03T10:30:00Z",
    "joinedDate": "2025-01-01T00:00:00Z",
    "isOwner": false
  }
]
```

#### POST /api/account/invite-user
```json
{
  "email": "mehmet@example.com",
  "firstName": "Mehmet",
  "lastName": "Kaya",
  "roleNames": ["Field Worker"],
  "personalMessage": "Aramıza hoş geldin Mehmet!"
}

Response:
{
  "invitationId": "guid",
  "invitationToken": "token",
  "expirationDate": "2025-01-11T00:00:00Z",
  "invitationUrl": "https://carpet.app/accept-invitation?token=xyz"
}
```

#### POST /api/account/accept-invitation
```json
{
  "invitationToken": "xyz123",
  "password": "StrongPass123!",
  "confirmPassword": "StrongPass123!",
  "firstName": "Mehmet",
  "lastName": "Kaya",
  "phoneNumber": "+905551234568"
}
```

#### DELETE /api/account/team-members/{userId}
Remove user from tenant (Owner only)

### Owner Management Endpoints

#### GET /api/account/tenant-info
```json
{
  "tenantId": "guid",
  "tenantName": "ahmet-hali-yikama",
  "displayName": "Ahmet Halı Yıkama",
  "createdDate": "2025-01-01T00:00:00Z",
  "ownerUserId": "guid",
  "isCurrentUserOwner": true,
  "customDomain": "ahmet.mycarpetapp.com",
  "settings": {
    "allowPublicRegistration": false,
    "requireEmailConfirmation": true,
    "defaultUserRole": "User"
  }
}
```

#### PUT /api/account/tenant-settings
```json
{
  "displayName": "Ahmet Halı Yıkama Ltd.",
  "allowPublicRegistration": false,
  "requireEmailConfirmation": true,
  "defaultUserRole": "User"
}
```

## 🔄 İŞ AKIŞI SENARYOLARI

### Senaryo 1: Yeni Kullanıcı Kaydı (Trial)
1. Kullanıcı `/register-with-trial` endpoint'ine istek atar
2. System yeni tenant oluşturur (subdomain: `{companyName}`)
3. Trial subscription oluşturur (7 gün, 2 kullanıcı)
4. Kullanıcıyı tenant owner yapar
5. Welcome email gönderir
6. JWT token döner

### Senaryo 2: Team Member Invitation
1. Owner `/invite-user` endpoint'ine istek atar
2. System subscription limit kontrol eder
3. Invitation record oluşturur ve email gönderir
4. Davet edilen kişi email'deki link'e tıklar
5. Register form'u doldurur
6. System kullanıcı oluşturur ve tenant'a atar
7. Subscription user count artırır

### Senaryo 3: Login Process
1. Kullanıcı login ister
2. System kullanıcı credentials kontrol eder
3. Tenant subscription status kontrol eder
4. Eğer subscription aktif değilse error döner
5. Eğer aktifse JWT token döner

### Senaryo 4: Subscription Upgrade
1. Owner subscription plan upgrade ister
2. Payment process (RevenueCat/Stripe)
3. Payment success callback
4. Subscription güncellenir
5. User limit artırılır
6. History kaydı oluşturulur

## 🚧 IMPLEMENTATION PLAN

### Phase 1: Core Infrastructure (1 hafta)
- [ ] Database migration scripts
- [ ] Entity definitions
- [ ] Domain services (SubscriptionManager, UserInvitationManager)
- [ ] Basic authorization handlers

### Phase 2: API Development (1 hafta)
- [ ] Account management endpoints
- [ ] Subscription endpoints
- [ ] Team management endpoints
- [ ] Owner management endpoints

### Phase 3: Authentication Integration (3 gün)
- [ ] Login/Register flow enhancement
- [ ] JWT token custom claims
- [ ] Subscription validation middleware
- [ ] Trial expiration handling

### Phase 4: Payment Integration (1 hafta)
- [ ] RevenueCat/Stripe webhook handlers
- [ ] Subscription renewal automation
- [ ] Payment failure handling
- [ ] Upgrade/downgrade flows

### Phase 5: Email & Notifications (3 gün)
- [ ] Invitation email templates
- [ ] Trial expiration warnings
- [ ] Payment failure notifications
- [ ] Welcome email sequences

## 🎯 BAŞARI KRİTERLERİ

### Functional Requirements
- ✅ Kullanıcı trial ile kayıt olabilir
- ✅ Owner team member ekleyebilir
- ✅ Subscription limit kontrolü çalışır
- ✅ Payment integration çalışır
- ✅ Trial to paid conversion çalışır

### Technical Requirements
- ✅ Multi-tenant data isolation
- ✅ Performance: Login < 500ms
- ✅ Security: JWT + subscription validation
- ✅ Scalability: Webhook-based payment processing

### Business Requirements
- ✅ 7-day trial conversion rate > %15
- ✅ User invitation acceptance rate > %80
- ✅ Payment success rate > %95
- ✅ System uptime > %99.5

Bu sistem ile WebCarpetApp profesyonel bir SaaS platformu haline gelecek! 🚀