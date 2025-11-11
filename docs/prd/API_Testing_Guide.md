# WebCarpetApp - API Testing Guide 🚀

## 📋 OVERVIEW

WebCarpetApp Subscription System API'lerini test etmek için kapsamlı bir rehber. Bu sistem SaaS multi-tenant yapısında, subscription-based halı yıkama management platformudur.

## 🔧 SETUP

### 1. Postman Collection Import
1. `WebCarpetApp_Subscription_APIs.postman_collection.json` dosyasını Postman'e import edin
2. `WebCarpetApp_Development.postman_environment.json` environment'ını import edin
3. Environment'ı "WebCarpetApp - Development" olarak seçin

### 2. Base URL Configuration
```
Development: https://localhost:5001
Production: https://your-domain.com
```

### 3. Database Verification
```sql
-- Subscription plans'ların oluştuğunu kontrol edin
SELECT Name, DisplayName, MaxUserCount, Price FROM SubscriptionPlans;

-- Beklenen sonuç:
-- trial        | 7-Day Trial      | 2   | 0.00
-- starter      | Starter Plan     | 5   | 29.99
-- professional | Professional Plan| 25  | 99.99
-- enterprise   | Enterprise Plan  | 100 | 299.99
```

## 🎯 TEST SCENARIOS

### 📝 Scenario 1: Yeni Şirket Kaydı ve Trial Başlatma

#### 1.1 Company Registration
```http
POST /api/account/register-with-trial
```
```json
{
  "tenantName": "Elit Halı Temizlik A.Ş.",
  "ownerEmail": "owner@elithali.com",
  "ownerName": "Mehmet Özkan",
  "password": "SecurePass123!",
  "tenantDescription": "Profesyonel halı ve koltuk temizlik hizmetleri"
}
```

**Expected Response:**
```json
{
  "success": true,
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "hasActiveSubscription": true,
  "isTrialActive": true,
  "subscriptionEndDate": "2025-01-12T10:00:00Z",
  "daysRemaining": 7,
  "planName": "trial",
  "planDisplayName": "7-Day Trial",
  "currentUserCount": 1,
  "maxUserCount": 2
}
```

#### 1.2 Verify Trial Subscription
```http
GET /api/account/subscriptions/my-subscription
Authorization: Bearer {{ACCESS_TOKEN}}
```

#### 1.3 Check Usage Statistics
```http
GET /api/account/subscriptions/usage
Authorization: Bearer {{ACCESS_TOKEN}}
```

### 📝 Scenario 2: Team Member Davetleri

#### 2.1 Invite First Employee
```http
POST /api/account/team/invite
Authorization: Bearer {{ACCESS_TOKEN}}
```
```json
{
  "email": "employee1@elithali.com",
  "roleNames": ["User", "OrderManager"],
  "invitationMessage": "Şirketimizin yeni sistemine hoş geldiniz! Halı yıkama siparişlerini bu platform üzerinden yönetebilirsiniz."
}
```

#### 2.2 Check Pending Invitations
```http
GET /api/account/team/invitations
Authorization: Bearer {{ACCESS_TOKEN}}
```

#### 2.3 Validate Invitation (Employee Side)
```http
GET /api/account/team/validate-invitation?invitationToken=RECEIVED_TOKEN
```

#### 2.4 Accept Invitation
```http
POST /api/account/team/accept-invitation
Authorization: Bearer {{EMPLOYEE_ACCESS_TOKEN}}
```
```json
{
  "invitationToken": "RECEIVED_TOKEN"
}
```

### 📝 Scenario 3: Subscription Upgrade Journey

#### 3.1 Check If Can Add More Users
```http
GET /api/account/subscriptions/can-add-user
Authorization: Bearer {{ACCESS_TOKEN}}
```

#### 3.2 Upgrade to Starter Plan
```http
PUT /api/account/subscriptions/upgrade
Authorization: Bearer {{ACCESS_TOKEN}}
```
```json
{
  "subscriptionPlanId": "22222222-2222-2222-2222-222222222222",
  "paymentTransactionId": "stripe_pi_1234567890",
  "notes": "Trial süresi dolduğu için Starter plana geçiş yapıldı"
}
```

#### 3.3 Verify Upgrade
```http
GET /api/account/subscriptions/my-subscription
Authorization: Bearer {{ACCESS_TOKEN}}
```

### 📝 Scenario 4: Business Growth - Professional Plan

#### 4.1 Add More Team Members (Test Limits)
```http
POST /api/account/team/invite
Authorization: Bearer {{ACCESS_TOKEN}}
```
```json
{
  "email": "manager@elithali.com",
  "roleNames": ["Manager"],
  "invitationMessage": "Yönetici olarak sisteme katılmanız bekleniyor."
}
```

#### 4.2 Upgrade to Professional Plan
```http
PUT /api/account/subscriptions/upgrade
Authorization: Bearer {{ACCESS_TOKEN}}
```
```json
{
  "subscriptionPlanId": "33333333-3333-3333-3333-333333333333",
  "paymentTransactionId": "stripe_pi_professional_789",
  "notes": "İş büyüyor, 25 kullanıcıya kadar ihtiyacımız var"
}
```

### 📝 Scenario 5: Enterprise Scale

#### 5.1 Large Team Management
```http
GET /api/account/team/members
Authorization: Bearer {{ACCESS_TOKEN}}
```

#### 5.2 Enterprise Upgrade
```http
PUT /api/account/subscriptions/upgrade
Authorization: Bearer {{ACCESS_TOKEN}}
```
```json
{
  "subscriptionPlanId": "44444444-4444-4444-4444-444444444444",
  "paymentTransactionId": "stripe_pi_enterprise_999",
  "notes": "100+ kullanıcı için Enterprise plan gerekli"
}
```

## 🔐 AUTHENTICATION FLOW TESTING

### Login with Subscription Validation
```http
POST /api/account/login
```
```json
{
  "userNameOrEmailAddress": "owner@elithali.com",
  "password": "SecurePass123!"
}
```

### Check Login Status
```http
GET /api/account/login-status
Authorization: Bearer {{ACCESS_TOKEN}}
```

## 🚫 ERROR SCENARIOS

### 1. Exceeded User Limit
Try to invite more users than subscription allows:
```http
POST /api/account/team/invite
Authorization: Bearer {{ACCESS_TOKEN}}
```
**Expected Error:** 400 Bad Request - "User limit exceeded for current subscription plan"

### 2. Expired Trial Access
Try to access after trial expires:
```http
GET /api/account/subscriptions/usage
Authorization: Bearer {{EXPIRED_TOKEN}}
```
**Expected Error:** 402 Payment Required - "Subscription expired"

### 3. Invalid Invitation Token
```http
POST /api/account/team/accept-invitation
```
```json
{
  "invitationToken": "invalid_token"
}
```
**Expected Error:** 400 Bad Request - "Invalid or expired invitation token"

## 📊 DATABASE QUERIES FOR VERIFICATION

### Check Subscription Status
```sql
SELECT
    ts.Id,
    sp.Name as PlanName,
    ts.Status,
    ts.StartDate,
    ts.EndDate,
    ts.IsTrialUsed,
    DATEDIFF(day, GETDATE(), ts.EndDate) as DaysRemaining
FROM TenantSubscriptions ts
JOIN SubscriptionPlans sp ON ts.SubscriptionPlanId = sp.Id
WHERE ts.TenantId = 'YOUR_TENANT_ID';
```

### Check Team Members
```sql
SELECT
    ui.Email,
    ui.Status,
    ui.InvitationDate,
    ui.ExpirationDate,
    ui.RoleNames
FROM UserInvitations ui
WHERE ui.TenantId = 'YOUR_TENANT_ID';
```

### Check Subscription History
```sql
SELECT
    sh.Action,
    sh.ActionDate,
    sh.OldValue,
    sh.NewValue,
    sh.Amount,
    sh.PaymentStatus
FROM SubscriptionHistories sh
JOIN TenantSubscriptions ts ON sh.TenantSubscriptionId = ts.Id
WHERE ts.TenantId = 'YOUR_TENANT_ID'
ORDER BY sh.ActionDate DESC;
```

## 🎯 SUCCESS CRITERIA

### ✅ Registration Flow
- [ ] Yeni tenant başarıyla oluşturuldu
- [ ] 7-day trial otomatik aktif edildi
- [ ] Owner primary owner olarak atandı
- [ ] JWT token ve subscription bilgileri döndü

### ✅ Team Management
- [ ] Owner kullanıcı davet edebildi
- [ ] Invitation email gönderildi (simulated)
- [ ] Davet edilen kullanıcı daveti doğrulayabildi
- [ ] Davet kabul edildi ve kullanıcı tenant'a eklendi

### ✅ Subscription Upgrades
- [ ] Trial'dan Starter'a upgrade başarılı
- [ ] Payment transaction ID kaydedildi
- [ ] User limit arttı (2→5)
- [ ] Subscription history kaydı oluşturuldu

### ✅ Limit Enforcement
- [ ] User limit aşıldığında davet reddedildi
- [ ] Trial süresi dolduğunda access engellendi
- [ ] Subscription validation middleware çalıştı

## 🔗 USEFUL ENDPOINTS

### Health Check
```http
GET /health
```

### Swagger Documentation
```
https://localhost:5001/swagger
```

### Database Connection Test
```http
GET /api/app/companies
Authorization: Bearer {{ACCESS_TOKEN}}
```

## 🎉 CONGRATULATIONS!

Eğer tüm test senaryoları başarılı ise, WebCarpetApp Subscription System tam olarak çalışıyor demektir!

🚀 **Production'a hazır SaaS platform!**