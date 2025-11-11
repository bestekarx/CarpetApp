# WebCarpetApp - Halı Yıkama Takip Sistemi
## İş Analizi ve Teknik Döküman

### Proje Genel Bakış
WebCarpetApp, ABP Framework (.NET 9) üzerine inşa edilmiş, multi-tenant mimarili halı yıkama işletmesi yönetim sistemidir. Sistem, müşteri siparişlerinden halı alımı, yıkama ve teslimat süreçlerinin tamamını yönetir.

## 1. İş Varlıkları (Business Entities)

### 1.1 Müşteri Yönetimi
- **Customer**: Müşteri bilgileri (ad, telefon, adres, koordinat, bakiye, doğrulama durumu)
- **Areas**: Coğrafi bölge kategorileri
- **Companies**: Multi-tenant şirket yapısı (isim, açıklama, renk)

### 1.2 Sipariş İş Akışı
- **Received**: Halı alım kayıtları (durum takibi, teslimat tarihleri, fiş numarası)
- **Order**: Alım içindeki bireysel siparişler (fiyat, indirim, durum)
  - Sipariş Durumları: Passive → Active → InProcess → Completed → ReadyForDelivery → Delivered → Cancelled
- **OrderedProduct**: Sipariş kalemleri (ürün, miktar, metrekare)
- **OrderImage**: Sipariş görselleri (blob storage)

### 1.3 Ürün ve Hizmet Yönetimi
- **Product**: Hizmet/ürün kataloğu
  - Ürün Tipleri: Service, Product, Fason, SeatClean
- **Vehicle**: Teslimat araçları (plaka numaraları)

### 1.4 Mali Yönetim
- **Invoice**: Fatura yapısı (toplam fiyat, ödenen tutar, ödeme tipi)

### 1.5 Operasyonel Destek
- **Printer**: Yazıcı yapılandırması (MAC adresleri)
- **UserTenantMapping**: Kullanıcı-tenant ilişki yönetimi

## 2. Mesajlaşma Sistemi

### 2.1 Mesaj Mimarisi
- **MessageConfiguration**: Şirket özel mesaj ayarları
- **MessageUser**: SMS servis kimlik bilgileri
- **MessageTemplate**: Şablonlu mesajlar (placeholder mapping, kültür desteği)
- **MessageTask**: Otomatik mesaj tetikleyicileri
- **MessageBehavior**: Gönderim mantığı (AlwaysSend, NeverSend, AskBeforeSend)

### 2.2 Mesaj Tetikleyici Tipleri
- ReceivedCreated/Cancelled
- OrderCreated/Completed/Cancelled
- InvoiceCreated/Paid

## 3. İş Süreçleri

### 3.1 Halı İşleme İş Akışı
1. **Alım (Received)**: Müşteri, araç, tarihlerle alım kaydı
2. **Sipariş Oluşturma**: Alım içinde ürün/hizmetlerle siparişler
3. **İşleme**: Sipariş durumunu yıkama aşamalarında güncelleme
4. **Teslimat**: Teslimata hazır ve teslim edildi işaretleme
5. **Faturalama**: Tamamlanan siparişler için fatura oluşturma
6. **Bildirim**: Önemli aşamalarda otomatik SMS bildirimleri

### 3.2 Fiş Numarası Yönetimi
- Tenant özel fiş numarası üretimi
- Sıralı numaralama ve sıfırlama özelliği
- Thread-safe implementasyon

## 4. Multi-Tenancy Yapısı

### 4.1 Tenant Mimarisi
- ABP Framework ile tam multi-tenancy desteği
- Her iş varlığı `IMultiTenant` uygular
- Tenant özel ayarlar (fiş numaralama, mesaj yapılandırmaları)

### 4.2 Tenant Yönetim Özellikleri
- Programatik tenant oluşturma
- Kullanıcı çoklu tenant ataması
- Tenant başına rol bazlı erişim kontrolü

## 5. Teknik Mimari

### 5.1 Domain Katmanı
- Domain servisleri: `OrderManager`, `ReceivedManager`, `MessageManager`, `FicheNoManager`
- Repository pattern ile generic repositories
- Domain events ve iş kuralı zorlaması

### 5.2 Application Katmanı
- CRUD servisleri (`CrudAppService` kullanarak)
- Gelişmiş filtreleme ve sayfalama
- Kapsamlı DTO'lar ve mapping profilleri

### 5.3 API Katmanı
- ABP'nin otomatik API üretimi ile RESTful API'ler
- Tenant yönetimi ve blob storage için özel kontrolcüler
- Permission bazlı yetkilendirme

### 5.4 Data Katmanı
- Entity Framework Core ve migration desteği
- Entity ilişkileri ve foreign key kısıtlamaları
- JSON field storage (mesaj template mappings)

## 6. Yetki Sistemi
Tüm major entity'ler için kapsamlı permission yapısı:
- Books, Areas, Companies, Products, Customers, Vehicles
- Receiveds, Orders, Invoices
- Mesaj sistem bileşenleri
- Blob storage operasyonları
- Standart CRUD yetkiler (Default, Create, Edit, Delete)

## 7. Eksik/Geliştirilmemiş Modüller

### 7.1 Bilinen Eksik Özellikler
1. **Order Update Method**: `OrderAppService.UpdateAsync` metodu `NotImplementedException` fırlatıyor
2. **SMS Entegrasyonu**: `MessageSender` servisi gerçek SMS API entegrasyonu için placeholder kod
3. **Ödeme İşleme**: Fatura ödeme takibi temel seviyede - ödeme gateway entegrasyonu yok
4. **Raporlama Modülü**: İş analitiği için özel raporlama servisleri yok
5. **Müşteri Bakiye Yönetimi**: Müşteri bakiye takibi var ama ödeme/kredi iş akışı yok

### 7.2 Potansiyel Geliştirme Alanları
1. **Gelişmiş Zamanlama**: Teslimat zamanlama sistemi yok
2. **GPS Takip**: Araç takibi implement edilmemiş
3. **Fotoğraf Dokümantasyonu**: Temel resim depolama var ama gelişmiş fotoğraf yönetimi yok
4. **Müşteri Portalı**: Müşterilerin sipariş takibi için arayüz yok
5. **Mobil Uygulama**: Native mobil uygulama bileşenleri yok
6. **Gelişmiş Fiyatlandırma**: Sabit fiyat modeli, dinamik fiyat kuralları yok
7. **Envanter Yönetimi**: Temizlik malzemeleri için stok takibi yok
8. **Kalite Kontrol**: Kalite kontrolleri veya müşteri geri bildirim sistemi yok

## 8. İş Hazırlığı Değerlendirmesi

### 8.1 Üretime Hazır Bileşenler
- ✅ Müşteri yönetimi
- ✅ Sipariş iş akışı (oluşturma, takip, durum güncellemeleri)
- ✅ Ürün kataloğu
- ✅ Araç yönetimi
- ✅ Multi-tenancy
- ✅ Kullanıcı yönetimi ve yetkiler
- ✅ Temel faturalama
- ✅ Fiş numarası üretimi
- ✅ Mesaj şablon sistemi

### 8.2 Geliştirme Gereken
- ❌ Sipariş güncellemeleri (kritik iş operasyonu)
- ❌ SMS servis entegrasyonu
- ❌ Ödeme işleme
- ❌ Müşteri self-servis portalı
- ❌ Gelişmiş raporlama ve analitik
- ❌ Mobil saha uygulamaları

## Sonuç

WebCarpetApp, domain modelleme, multi-tenancy ve iş süreci otomasyonunda sağlam temellere sahip, iyi tasarlanmış bir halı yıkama işletmesi yönetim sistemidir. Halı alımından teslimat sürecine kadar temel halı yıkama iş akışı kapsamlı durum takibi ile iyi implement edilmiştir.

Mesajlaşma sistemi, şablon bazlı bildirimler ve yapılandırılabilir davranışlarla sofistike bir tasım sergiler. Multi-tenant mimari, ortak işlevselliği paylaşırken iş verilerini düzgün bir şekilde izole eder.

Sistemi tam operasyonel hale getirmek için sipariş güncelleme işlevselliği ve SMS servis entegrasyonunun tamamlanması gereken temel alanlar. Mobil uygulamalar, müşteri portalları ve gelişmiş analitik için genişletme konusunda sağlam bir temel mevcuttur.