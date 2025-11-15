# WebCarpetApp Mobil Entegrasyon Dokümantasyonu

## 📚 Doküman İçeriği

Bu klasörde WebCarpetApp API'sini mobil uygulamalarınıza entegre etmek için gereken tüm dokümantasyon bulunmaktadır.

### 📋 Doküman Listesi

| Dosya | Açıklama | Hedef Kitle |
|-------|----------|-------------|
| **[01-mobile-integration-overview.md](./01-mobile-integration-overview.md)** | Genel bakış ve mimari özet | Product Manager, Lead Developer |
| **[02-authentication-flow.md](./02-authentication-flow.md)** | Kimlik doğrulama ve güvenlik | Mobile Developer, Security |
| **[03-api-integration-patterns.md](./03-api-integration-patterns.md)** | API entegrasyon kalıpları | Mobile Developer |
| **[04-error-handling-guide.md](./04-error-handling-guide.md)** | Hata yönetimi ve lokalizasyon | Mobile Developer, QA |
| **[05-offline-sync-implementation.md](./05-offline-sync-implementation.md)** | Çevrimdışı senkronizasyon | Senior Mobile Developer |
| **[06-mobile-best-practices.md](./06-mobile-best-practices.md)** | Performans ve güvenlik | Lead Developer, Architect |
| **[07-implementation-examples.md](./07-implementation-examples.md)** | Kod örnekleri ve implementasyon | Mobile Developer |
| **[08-testing-scenarios.md](./08-testing-scenarios.md)** | Test senaryoları ve QA | QA Engineer, Mobile Developer |

## 🎯 Hızlı Başlangıç

### 1. İlk Okuma Sırası
Yeni başlayan geliştiriciler için önerilen okuma sırası:

1. **[Genel Bakış](./01-mobile-integration-overview.md)** - Projeyi anlayın
2. **[Kimlik Doğrulama](./02-authentication-flow.md)** - Güvenlik temellerini öğrenin
3. **[Kod Örnekleri](./07-implementation-examples.md)** - Pratik uygulamaları görün
4. **[Error Handling](./04-error-handling-guide.md)** - Hata yönetimini öğrenin

### 2. İleri Seviye
Deneyimli geliştiriciler için:

1. **[API Kalıpları](./03-api-integration-patterns.md)** - Advanced patterns
2. **[Offline Sync](./05-offline-sync-implementation.md)** - Complex scenarios
3. **[Best Practices](./06-mobile-best-practices.md)** - Performance tuning
4. **[Testing](./08-testing-scenarios.md)** - Quality assurance

## 🔧 Teknik Gereksinimler

### API Endpoint
```
Development: https://localhost:44302
Production: [Henüz belirtilmedi]
```

### Desteklenen Platformlar
- **iOS**: 13.0+
- **Android**: API Level 21+ (Android 5.0)
- **React Native**: 0.68+
- **Flutter**: 3.0+
- **Xamarin**: Forms 5.0+

### Minimum SDK Gereksinimleri
```json
{
  "dependencies": {
    "@react-native-async-storage/async-storage": "^1.17.0",
    "@react-native-community/netinfo": "^9.0.0",
    "react-native-biometrics": "^3.0.0",
    "react-native-keychain": "^8.0.0"
  }
}
```

## 📱 Desteklenen Özellikler

### ✅ Temel Özellikler
- [x] **Multi-tenant authentication** - Çoklu kiracı kimlik doğrulama
- [x] **Standardized API responses** - Tutarlı yanıt formatı
- [x] **Multilingual support** - Türkçe, İngilizce, Arapça
- [x] **Offline-first architecture** - Çevrimdışı öncelikli mimari
- [x] **Real-time sync** - Gerçek zamanlı senkronizasyon
- [x] **GPS integration** - Konum tabanlı özellikler

### 🔐 Güvenlik Özellikleri
- [x] **JWT Bearer Authentication** - Güvenli token tabanlı kimlik doğrulama
- [x] **SSL Certificate Pinning** - Sertifika sabitleme (production için)
- [x] **Biometric Authentication** - Biyometrik kimlik doğrulama desteği
- [x] **Secure Storage** - Güvenli veri depolama
- [x] **Auto Token Refresh** - Otomatik token yenileme

### 📊 İş Süreçleri
- [x] **Customer Management** - Müşteri yönetimi
- [x] **Order Processing** - Sipariş işlemleri
- [x] **Vehicle Fleet** - Araç filosu yönetimi
- [x] **Product Catalog** - Ürün kataloğu
- [x] **Invoice Management** - Fatura yönetimi
- [x] **Analytics Dashboard** - Analitik dashboard

## 🛠️ Geliştirme Ortamı Kurulumu

### 1. API Ayarları
```typescript
// config.ts
export const API_CONFIG = {
  baseURL: __DEV__
    ? 'https://localhost:44302/api'
    : 'https://api.webcarpetapp.com/api',

  timeout: 30000,

  headers: {
    'Accept-Language': 'tr', // tr|en|ar
    'Content-Type': 'application/json'
  }
};
```

### 2. Güvenlik Ayarları
```typescript
// security.ts
export const SECURITY_CONFIG = {
  enableSSLPinning: !__DEV__,
  enableBiometric: true,
  tokenRefreshThreshold: 300000, // 5 minutes before expiry
  maxRetryAttempts: 3
};
```

### 3. Offline Ayarları
```typescript
// offline.ts
export const OFFLINE_CONFIG = {
  syncIntervalMs: 300000, // 5 minutes
  maxCacheSize: 1000, // items per entity type
  cacheExpiryMs: 600000, // 10 minutes
  enableOptimisticUpdates: true
};
```

## 📞 Destek ve İletişim

### 🐛 Hata Bildirimi
Hata bulduğunuzda lütfen aşağıdaki bilgileri içeren bir rapor oluşturun:

```markdown
**Hata Türü**: [Bug/Performance/Security/Documentation]
**Platform**: [iOS/Android/Web]
**Versiyon**: [App version]
**API Endpoint**: [Hangi endpoint'te oluştu]
**Hata Mesajı**: [Tam hata mesajı]
**Adımlar**: [Hatayı tekrar oluşturma adımları]
**Beklenen Sonuç**: [Ne olması gerekiyordu]
**Gerçek Sonuç**: [Ne oldu]
**Screenshots**: [Varsa ekran görüntüleri]
```

### 🔍 Debug Bilgileri
Development ortamında debug modunu açmak için:

```typescript
// Enable debug mode
window.__WEBCARPETAPP_DEBUG__ = true;

// API calls will be logged
// Network requests will be detailed
// Error stack traces will be shown
```

## 📈 Performans Metrikleri

### Benchmark Hedefleri
| Metric | Target | Measurement |
|--------|--------|-------------|
| API Response Time | < 2s | 95th percentile |
| App Launch Time | < 3s | Cold start |
| List Loading | < 1s | 100 items |
| Sync Duration | < 10s | 1000 operations |
| Memory Usage | < 150MB | Peak usage |
| Battery Impact | < 5% | Per hour usage |

### İzleme
```typescript
// Performance monitoring
import { PerformanceMonitor } from './utils/performance';

const monitor = new PerformanceMonitor();
monitor.trackApiCall('customer-list', () => customerService.getList());
monitor.trackUserAction('customer-create', customerData);
monitor.trackMemoryUsage();
```

## 🚀 Deployment Checklist

### Pre-Production
- [ ] Tüm testler geçiyor
- [ ] SSL pinning aktif
- [ ] Debug mode kapalı
- [ ] Analytics entegrasyonu aktif
- [ ] Crash reporting aktif
- [ ] Performance monitoring aktif

### Production Release
- [ ] API endpoint production'a değişti
- [ ] Security review tamamlandı
- [ ] Load testing yapıldı
- [ ] Backup/recovery planı hazır
- [ ] Monitoring dashboard'ları aktif
- [ ] Support dokümantasyonu güncel

## 📋 API Referansı

### Hızlı Referans
```typescript
// Authentication
POST /api/subscription-account/register-with-trial
POST /api/subscription-account/find-tenant
POST /connect/token

// Customer Management
GET /api/app/customer/list-with-response
POST /api/app/customer/create-with-response
PUT /api/app/customer/update-with-response/{id}
DELETE /api/app/customer/delete-with-response/{id}

// Order Management
GET /api/app/order/list-with-response
POST /api/app/order/create-with-response
POST /api/app/order/nearest-with-response

// Vehicle Management
GET /api/app/vehicle/list-with-response
POST /api/app/vehicle/create-with-response
PUT /api/app/vehicle/update-with-response/{id}
```

### Postman Collection
```bash
# Import collection for testing
curl -O https://raw.githubusercontent.com/webcarpetapp/api-docs/main/WebCarpetApp-Smart-API-Collection.postman_collection.json
```

---

**📝 Not**: Bu dokümantasyon sürekli güncellenmektedir. En güncel versiyonu için repository'yi kontrol edin.

**🔄 Son Güncelleme**: 15 Kasım 2025
**📄 Versiyon**: 1.0.0