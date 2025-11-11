# WebCarpetApp - Product Requirements Document (PRD)

## 📁 Dokümantasyon Genel Bakışı

Bu klasör, WebCarpetApp (Halı Yıkama Takip Sistemi) için kapsamlı ürün gereksinim dokümantasyonunu içerir.

### 📋 Dokümantasyon İçeriği

#### 1. [WebCarpetApp_Business_Analysis.md](./WebCarpetApp_Business_Analysis.md)
- **İçerik**: Proje genel bakışı ve derinlemesine iş analizi
- **Kapsamı**:
  - İş varlıkları (Business Entities) detaylı açıklamaları
  - Halı yıkama iş akışı süreçleri
  - Mesajlaşma sistemi mimarisi
  - Multi-tenancy yapısı
  - Teknik mimari analizi
  - Eksik ve geliştirilmemiş modüller
  - İş hazırlığı değerlendirmesi

#### 2. [Entity_Relationships.md](./Entity_Relationships.md)
- **İçerik**: Veritabanı şeması ve entity ilişkileri
- **Kapsamı**:
  - Entity Relationship Diagram (ERD)
  - Detaylı entity açıklamaları ve alan tanımları
  - Foreign key kısıtlamaları
  - İş kuralları ve validation'lar
  - İndeks ve performans optimizasyonları
  - Multi-tenant data isolation stratejisi

#### 3. [Development_Roadmap.md](./Development_Roadmap.md)
- **İçerik**: Geliştirme yol haritası ve öncelikler
- **Kapsamı**:
  - Kritik eksikler ve acil düzeltilmesi gerekenler
  - Prioriteli geliştirme planı (P0, P1, P2, P3)
  - Sprint planlaması ve zaman çizelgesi
  - Teknik debt ve refactoring ihtiyaçları
  - Risk analizi ve azaltma stratejileri
  - Başarı metrikleri (KPI'lar)

#### 4. [API_Endpoints.md](./API_Endpoints.md)
- **İçerik**: Kapsamlı API dokümantasyonu
- **Kapsamı**:
  - Tüm endpoint'lerin detaylı açıklamaları
  - Request/Response örnekleri
  - Authentication ve authorization
  - Query parametreleri ve filtreleme
  - Error handling ve status kodları
  - Rate limiting ve versioning bilgileri

## 🎯 WebCarpetApp Nedir?

WebCarpetApp, ABP Framework (.NET 9) üzerine inşa edilmiş, halı yıkama işletmeleri için tasarlanmış kapsamlı bir takip ve yönetim sistemidir.

### Temel Özellikler:
- **Multi-tenant mimari**: Çoklu şirket desteği
- **Müşteri yönetimi**: Kapsamlı müşteri bilgi sistemi
- **Sipariş takibi**: Alımdan teslimata kadar tam süreç takibi
- **Araç yönetimi**: Teslimat araçları ve rotalar
- **Faturalama sistemi**: Mali yönetim ve ödeme takibi
- **Mesajlaşma sistemi**: Otomatik SMS bildirimleri
- **Raporlama**: İş analitiği ve operasyonel raporlar

### İş Akışı:
1. **Halı Alımı**: Müşteriden halı alım kaydı
2. **Sipariş Oluşturma**: Alınan halılar için hizmet siparişleri
3. **İşleme Süreci**: Yıkama aşamalarında durum takibi
4. **Teslimat**: Hazır halıların müşteriye teslimi
5. **Faturalama**: Ödeme işlemleri ve mali takip
6. **Bildirimler**: Süreç boyunca otomatik müşteri bildirimleri

## 🚀 Mevcut Durum ve Öncelikler

### ✅ Hazır Modüller:
- Müşteri, ürün, araç yönetimi
- Temel sipariş iş akışı
- Multi-tenancy altyapısı
- Kullanıcı yönetimi ve yetkiler
- Mesaj şablon sistemi
- Fiş numarası üretimi

### ⚠️ Kritik Eksikler:
- **Order Update Functionality**: Sipariş güncelleme çalışmıyor
- **SMS Integration**: Gerçek SMS servis entegrasyonu yok
- **Payment Processing**: Ödeme gateway entegrasyonu eksik

### 📈 Gelecek Geliştirmeler:
- Mobil alan uygulaması
- Müşteri self-servis portalı
- Gelişmiş raporlama ve analitik
- AI/ML entegrasyonları
- IoT cihaz entegrasyonları

## 👥 Hedef Kullanıcılar

1. **İşletme Sahipleri**: Genel yönetim ve raporlama
2. **Operasyon Yöneticileri**: Günlük operasyon takibi
3. **Saha Çalışanları**: Alım/teslimat işlemleri
4. **Müşteri Hizmetleri**: Müşteri sorguları ve destek
5. **Mali İşler**: Faturalama ve ödeme takibi
6. **Müşteriler**: Sipariş takibi ve self-servis

## 🏗️ Teknik Mimari

- **Framework**: ABP Framework 8.0 (.NET 9)
- **Database**: SQL Server 2019+
- **Authentication**: JWT Bearer + OpenIddict
- **Frontend**: API-first (RESTful services)
- **File Storage**: Blob storage (Azure/Local)
- **Messaging**: SMS integration ready
- **Deployment**: Docker-ready, cloud-native

## 📞 İletişim ve Destek

Bu dokümantasyon, WebCarpetApp projesinin mevcut durumunu ve gelecek planlarını kapsamlı bir şekilde açıklamaktadır. Proje geliştirme sürecinde bu dokümantasyon güncel tutulacak ve yeni özellikler eklendikçe genişletilecektir.

### Dokümantasyon Versiyonu: 1.0
### Son Güncelleme: 2025-01-04
### Hazırlayan: Claude AI (WebCarpetApp Project Analysis)