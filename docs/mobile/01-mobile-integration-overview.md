# WebCarpetApp Mobil Entegrasyon Genel Bakış

## Proje Amacı
WebCarpetApp API'si kullanarak mobil uygulama geliştirmek için kapsamlı entegrasyon rehberi.

## API Mimarisi Özeti

### 🏗️ Temel Mimari Bileşenler

#### **1. Multi-Tenant Yapı**
- Her kullanıcı belirli bir tenant'a bağlıdır
- Tüm API operasyonları otomatik olarak kullanıcının tenant'ına göre filtrelenir
- Tenant değiştirme sunucu tarafında otomatik olarak yönetilir

#### **2. Standardize API Yanıt Formatı**
```json
{
  "success": true|false,
  "data": <actual_data>,
  "message": "Lokalize edilmiş mesaj",
  "error": {
    "code": "MACHINE_READABLE_ERROR_CODE",
    "details": {}
  },
  "timestamp": "2025-11-15T08:00:00Z"
}
```

#### **3. Kimlik Doğrulama**
- **OAuth 2.0 / OpenID Connect** protokolü
- **JWT Bearer Token** tabanlı kimlik doğrulama
- **8 saat** token süresi
- **İki aşamalı giriş** süreci

### 🌍 Çoklu Dil Desteği
- **Türkçe (tr)**, **İngilizce (en)**, **Arapça (ar)**
- `Accept-Language` header'ı ile kontrol
- Tüm hata mesajları ve kullanıcı mesajları lokalize

### 🔄 Offline Sync Desteği
- Mobil cihazlarda çevrimdışı çalışma
- Otomatik senkronizasyon
- Çakışma çözümleme mekanizması

## Ana Fonksiyonel Alanlar

### **1. Hesap ve Kimlik Yönetimi**
- Tenant kaydı ve bulma
- Kullanıcı kayıt/giriş
- Team üyesi davet etme

### **2. İş Süreci Yönetimi**
- Sipariş (Order) yönetimi
- Müşteri (Customer) yönetimi
- Ürün (Product) kataloğu
- Araç (Vehicle) filosu

### **3. Lojistik ve Teslimat**
- Alınan mallar (Received)
- Fatura (Invoice) yönetimi
- GPS tabanlı konum takibi
- En yakın teslimat siparişleri

### **4. Analitik ve Raporlama**
- Dashboard metrikleri
- Kullanım istatistikleri
- İş performans göstergeleri

## Teknik Özellikler

### **🔐 Güvenlik**
- Bearer token ile API erişimi
- Tenant bazlı veri izolasyonu
- Rol tabanlı yetkilendirme
- HTTPS zorunlu

### **📱 Mobil Optimizasyonlar**
- RESTful API tasarımı
- JSON veri formatı
- Sayfalama desteği
- Filtreleme ve sıralama

### **🌐 Çevrimdışı Çalışma**
- Local operation logging
- Conflict resolution
- Sync session management
- Auto-retry mechanisms

## Geliştirme Ortamı

### **API Base URL**
```
Development: https://localhost:44302
Production: [TBD]
```

### **Gerekli Header'lar**
```http
Authorization: Bearer {jwt_token}
Accept-Language: tr|en|ar
Content-Type: application/json
```

## Sonraki Adımlar

1. **[02-authentication-flow.md]** - Kimlik doğrulama akışı
2. **[03-api-integration-patterns.md]** - API entegrasyon kalıpları
3. **[04-error-handling-guide.md]** - Hata yönetimi rehberi
4. **[05-offline-sync-implementation.md]** - Çevrimdışı senkronizasyon
5. **[06-mobile-best-practices.md]** - Mobil geliştirme en iyi uygulamalar
6. **[07-implementation-examples.md]** - Kod örnekleri

---

**⚠️ Önemli Not**: Bu API şu anda geliştirme aşamasındadır. Production kullanımı için önce staging ortamında kapsamlı testler yapılmalıdır.