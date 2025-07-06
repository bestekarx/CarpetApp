# SMS Sistemi Kullanım Rehberi - Çok Dilli Dinamik Yapı

## 1. Çok Dilli PlaceholderMappings Sistemi

### Desteklenen Diller
- 🇹🇷 **Türkçe (tr-TR)**: `{isim}`, `{siparisNo}`, `{tutar}` 
- 🇺🇸 **İngilizce (en-US)**: `{name}`, `{orderNumber}`, `{amount}`
- 🇩🇪 **Almanca (de-DE)**: `{name}`, `{bestellnummer}`, `{betrag}`
- 🇫🇷 **Fransızca (fr-FR)**: `{nom}`, `{numeroCommande}`, `{montant}`

## 2. PlaceholderMappings Nasıl Çalışır?

### Adım 1: Dinamik Şablon ve Mapping Oluşturma
```csharp
// Türkçe kullanıcı şablonu:
string templateTR = "Sayın {isim}, siparişiniz {siparisNo} numarası ile kaydedildi. {companyName}";

// İngilizce kullanıcı şablonu:
string templateEN = "Dear {name}, your order {orderNumber} has been registered. {companyName}";

// Almanca kullanıcı şablonu:
string templateDE = "Liebe/r {name}, Ihre Bestellung {bestellnummer} wurde registriert. {firmaName}";

// PlaceholderMappings (otomatik kültüre göre oluşturuluyor)
var currentCulture = System.Globalization.CultureInfo.CurrentUICulture.Name;
var placeholderMappings = Consts.GetPlaceholderMappingsForTaskType(MessageTaskType.OrderCreated, currentCulture);

// Türkçe için: { "{isim}", "CustomerName" }, { "{siparisNo}", "OrderNumber" }
// İngilizce için: { "{name}", "CustomerName" }, { "{orderNumber}", "OrderNumber" }
// Almanca için: { "{name}", "CustomerName" }, { "{bestellnummer}", "OrderNumber" }
```

### Adım 2: Gerçek Değerleri Hazırlama
```csharp
// Sipariş oluşturulduğunda bu değerler toplanır
var values = new Dictionary<string, object>
{
    { "CustomerName", "Ahmet Yılmaz" },
    { "OrderNumber", "SIP-2024-001" },
    { "CompanyName", "Halı Yıkama Ltd." }
};
```

### Adım 3: Mesajı Formatla ve Gönder
```csharp
// Consts.FormatSmsMessage kullanarak formatla
string formattedMessage = Consts.FormatSmsMessage(template, placeholderMappings, values);
// Sonuç: "Sayın Ahmet Yılmaz, siparişiniz SIP-2024-001 numarası ile kaydedildi. Halı Yıkama Ltd."
```

## 2. Sipariş Oluşturulduğunda SMS Gönderme

```csharp
// OrderService içinde sipariş oluşturulduktan sonra:
public async Task<bool> CreateOrderAsync(OrderModel order)
{
    // Sipariş kaydet
    var result = await SaveOrderAsync(order);
    
    if (result)
    {
        // SMS gönder
        var smsService = new SmsService();
        var smsConfig = await smsService.GetActiveSmsConfigurationAsync(order.CompanyId);
        
        if (smsConfig != null)
        {
            var customer = await GetCustomerAsync(order.CustomerId);
            var company = await GetCompanyAsync(order.CompanyId);
            
            await smsService.SendOrderCreatedSmsAsync(smsConfig, customer, company, order.OrderNumber);
        }
    }
    
    return result;
}
```

## 3. Tüm MessageTaskType'lar İçin Mapping'ler

### MessageTaskType.OrderCreated
```
Placeholder: {isim} → Data Key: CustomerName
Placeholder: {siparisNo} → Data Key: OrderNumber  
Placeholder: {companyName} → Data Key: CompanyName
Placeholder: {companyPhone} → Data Key: CompanyPhone
```

### MessageTaskType.OrderCompleted
```
Placeholder: {isim} → Data Key: CustomerName
Placeholder: {siparisNo} → Data Key: OrderNumber
Placeholder: {siparisAdet} → Data Key: OrderQuantity
Placeholder: {tutar} → Data Key: OrderAmount
Placeholder: {companyName} → Data Key: CompanyName
```

### MessageTaskType.ReceivedCreated
```
Placeholder: {isim} → Data Key: CustomerName
Placeholder: {tarih} → Data Key: ReceivedDate
Placeholder: {companyName} → Data Key: CompanyName
Placeholder: {companyPhone} → Data Key: CompanyPhone
```

## 4. Gerçek Kullanım Senaryosu

### Kullanıcı Arayüzünde:
1. Kullanıcı "Sipariş Oluşturuldu" task'ı için şablon girer:
   ```
   Sayın {isim}, siparişiniz {siparisNo} ile alınmıştır. Detay için {companyPhone}
   ```

2. Sistem otomatik PlaceholderMappings oluşturur:
   ```csharp
   {
       "{isim}" = "CustomerName",
       "{siparisNo}" = "OrderNumber", 
       "{companyPhone}" = "CompanyPhone"
   }
   ```

### Sipariş Oluşturulunca:
1. Sistem ilgili verileri toplar:
   ```csharp
   var values = new Dictionary<string, object>
   {
       { "CustomerName", "Mehmet Demir" },
       { "OrderNumber", "SIP-2024-045" },
       { "CompanyPhone", "0212 555 12 34" }
   };
   ```

2. Mesaj formatlanır:
   ```
   Sayın Mehmet Demir, siparişiniz SIP-2024-045 ile alınmıştır. Detay için 0212 555 12 34
   ```

3. SMS gönderilir.

## 5. Çok Dilli Sistem Avantajları

### ✅ Dinamik Dil Desteği
- Uygulama dili değiştiğinde placeholder'lar otomatik güncellenir
- Yeni dil eklemek sadece `MultiLanguagePlaceholderMappings`'e eklenmesi kadar kolay
- If-else zincirleri yok, tamamen veri tabanlı

### ✅ Genişletilebilir Yapı
```csharp
// Yeni dil eklemek için sadece mapping'e ekleme yap:
"es-ES", new Dictionary<string, string>
{
    { "{nombre}", "CustomerName" },
    { "{numeroOrden}", "OrderNumber" },
    { "{nombreEmpresa}", "CompanyName" }
}
```

### ✅ Fallback Mekanizması
- Desteklenmeyen dil kodu gelirse en yakın dili bulur
- Hiçbir eşleşme yoksa Türkçe'ye döner
- Sistem asla crash olmaz

## 6. Yapman Gerekenler

1. **SmsService'i MauiProgram.cs'e kaydet:**
   ```csharp
   builder.Services.AddTransient<ISmsService, SmsService>();
   ```

2. **Sipariş oluşturma ViewModel'inde SmsService'i kullan:**
   ```csharp
   private readonly ISmsService _smsService;
   
   // Sipariş kaydedildikten sonra:
   await _smsService.SendOrderCreatedSmsAsync(smsConfig, customer, company, orderNumber);
   ```

3. **API'den SMS ayarlarını getir:**
   ```csharp
   // GetActiveSmsConfigurationAsync metodunu API ile entegre et
   ```

4. **Yeni dil eklemek için:**
   ```csharp
   // Sadece Consts.cs'deki MultiLanguagePlaceholderMappings'e yeni dil ekle
   // Sistem otomatik olarak yeni dili destekleyecek
   ```

## 6. Test İçin
Konsola yazdırılan mesajları kontrol et:
```
SMS Gönderildi: 05551234567 - Sayın Ahmet Yılmaz, siparişiniz SIP-2024-001 numarası ile kaydedildi.
```

Bu yapı ile tüm TaskType'lar için otomatik SMS gönderimi sağlanabilir! 