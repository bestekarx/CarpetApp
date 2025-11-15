# WebCarpetApp Mobil Kimlik Doğrulama Akışı

## Kimlik Doğrulama Mimarisi

### 🔐 Güvenlik Protokolü
- **OAuth 2.0 / OpenID Connect**
- **JWT Bearer Token** tabanlı
- **8 saat** token geçerlilik süresi
- **Multi-tenant** yapı desteği

## 1. Yeni Kullanıcı Kaydı (Registration Flow)

### **Adım 1: Tenant + Admin Kullanıcı Oluşturma**

```http
POST /api/subscription-account/register-with-trial
Content-Type: application/json
Accept-Language: tr

{
  "companyName": "Şirket Adı",
  "fullName": "İsim Soyisim",
  "emailAddress": "admin@sirket.com",
  "password": "GüçlüŞifre123!",
  "phoneNumber": "05551234567"
}
```

**Başarılı Yanıt:**
```json
{
  "success": true,
  "data": {
    "tenantId": "abc123-def456-ghi789",
    "isAuthenticated": true,
    "user": {
      "id": "user-guid",
      "email": "admin@sirket.com",
      "name": "İsim Soyisim"
    },
    "subscription": {
      "planName": "Trial",
      "expiryDate": "2025-12-15T00:00:00Z",
      "isActive": true
    }
  },
  "message": "Hesap başarıyla oluşturuldu",
  "timestamp": "2025-11-15T08:00:00Z"
}
```

## 2. Mevcut Kullanıcı Girişi (Login Flow)

### **Adım 1: Tenant Bulma**

```http
POST /api/subscription-account/find-tenant
Content-Type: application/json

{
  "emailAddress": "user@sirket.com"
}
```

**Başarılı Yanıt:**
```json
{
  "success": true,
  "data": {
    "tenantId": "abc123-def456-ghi789",
    "tenantName": "Şirket Adı",
    "userExists": true
  },
  "message": "Tenant bulundu"
}
```

### **Adım 2: OAuth Token Alma**

```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded
__tenant: abc123-def456-ghi789

grant_type=password&
client_id=WebCarpetApp_App&
client_secret=1q2w3e*&
username=user@sirket.com&
password=UserPassword123!&
scope=WebCarpetApp
```

**Başarılı Yanıt:**
```json
{
  "access_token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expires_in": 28800,
  "token_type": "Bearer",
  "scope": "WebCarpetApp",
  "refresh_token": "def456ghi789..."
}
```

## 3. Token Yönetimi

### **Token Depolama (Mobil)**
```javascript
// Güvenli depolama
const tokenData = {
  accessToken: response.access_token,
  refreshToken: response.refresh_token,
  expiresAt: Date.now() + (response.expires_in * 1000),
  tenantId: tenantId
};

// Encrypted storage'a kaydet
await SecureStorage.setItem('auth_tokens', JSON.stringify(tokenData));
```

### **Token Doğrulama**
```javascript
// Her API çağrısı öncesi token kontrolü
function isTokenValid() {
  const tokens = JSON.parse(await SecureStorage.getItem('auth_tokens'));
  return tokens && tokens.expiresAt > Date.now();
}
```

### **Otomatik Token Yenileme**
```javascript
async function refreshToken() {
  const tokens = JSON.parse(await SecureStorage.getItem('auth_tokens'));

  const response = await fetch('/connect/token', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded',
      '__tenant': tokens.tenantId
    },
    body: new URLSearchParams({
      grant_type: 'refresh_token',
      client_id: 'WebCarpetApp_App',
      client_secret: '1q2w3e*',
      refresh_token: tokens.refreshToken
    })
  });

  const newTokens = await response.json();
  // Yeni token'ları güvenli depolamaya kaydet
}
```

## 4. API Çağrıları için Kimlik Doğrulama

### **Standart Header Formatı**
```http
GET /api/app/customer/list-with-response
Authorization: Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9...
Accept-Language: tr
Content-Type: application/json
```

### **Mobil HTTP Client Yapılandırması**
```javascript
class ApiClient {
  constructor(baseUrl) {
    this.baseUrl = baseUrl;
    this.defaultHeaders = {
      'Content-Type': 'application/json',
      'Accept-Language': 'tr' // Kullanıcı tercihi
    };
  }

  async request(endpoint, options = {}) {
    const tokens = await this.getValidTokens();

    const headers = {
      ...this.defaultHeaders,
      'Authorization': `Bearer ${tokens.accessToken}`,
      ...options.headers
    };

    const response = await fetch(`${this.baseUrl}${endpoint}`, {
      ...options,
      headers
    });

    return this.handleResponse(response);
  }

  async getValidTokens() {
    let tokens = JSON.parse(await SecureStorage.getItem('auth_tokens'));

    // Token süresi dolmuşsa yenile
    if (tokens.expiresAt <= Date.now()) {
      tokens = await this.refreshToken();
    }

    return tokens;
  }
}
```

## 5. Hata Yönetimi

### **Kimlik Doğrulama Hataları**

```javascript
// 401 Unauthorized - Token geçersiz
if (response.status === 401) {
  // Kullanıcıyı login ekranına yönlendir
  await this.logout();
  navigationService.navigateToLogin();
}

// 403 Forbidden - Yetki yok
if (response.status === 403) {
  showError('Bu işlem için yetkiniz bulunmuyor');
}
```

### **Tenant Bulunamadı Hatası**
```json
{
  "success": false,
  "message": "Bu e-posta adresi ile kayıtlı tenant bulunamadı",
  "error": {
    "code": "TENANT_NOT_FOUND"
  }
}
```

## 6. Logout İşlemi

### **Token Temizleme**
```javascript
async function logout() {
  // Local token'ları temizle
  await SecureStorage.removeItem('auth_tokens');
  await SecureStorage.removeItem('user_data');

  // Sunucu tarafında token iptal et (opsiyonel)
  try {
    await fetch('/connect/logout', {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${currentToken}`
      }
    });
  } catch (e) {
    // Network hatası - local cleanup yeterli
  }

  // Login ekranına yönlendir
  navigationService.navigateToLogin();
}
```

## 7. Güvenlik En İyi Uygulamaları

### **Token Güvenliği**
- ✅ Token'ları encrypted storage'da sakla
- ✅ Token'ları log'larda gösterme
- ✅ HTTPS zorunlu kullan
- ✅ Token expiry süresini kontrol et

### **Mobil Specific Güvenlik**
- ✅ Biometric authentication entegrasyonu
- ✅ App backgrounda geçince auto-logout
- ✅ Screen recording/screenshot koruması
- ✅ SSL pinning (production için)

### **Multi-Tenant Güvenlik**
- ✅ Tenant ID'yi token'dan otomatik al
- ✅ Manuel tenant switching engelle
- ✅ Cross-tenant data access kontrolü

## 8. Test Senaryoları

### **Registration Test**
```javascript
// Test data
const testUser = {
  companyName: "Test Şirketi",
  fullName: "Test User",
  emailAddress: "test@example.com",
  password: "Test123!",
  phoneNumber: "05551234567"
};

// Test registration
const response = await apiClient.post('/api/subscription-account/register-with-trial', testUser);
assert(response.success === true);
assert(response.data.tenantId !== null);
```

### **Login Test**
```javascript
// Test login flow
const tenantResponse = await apiClient.post('/api/subscription-account/find-tenant', {
  emailAddress: "test@example.com"
});

const tokenResponse = await oauthClient.getToken({
  username: "test@example.com",
  password: "Test123!",
  tenantId: tenantResponse.data.tenantId
});

assert(tokenResponse.access_token !== null);
```

---

**💡 Önemli Notlar:**
- Token süresini her API çağrısı öncesi kontrol et
- Network bağlantısı olmadığında offline mode'a geç
- Biometric authentication'ı token yenilemede de kullan
- Production'da SSL certificate pinning uygula