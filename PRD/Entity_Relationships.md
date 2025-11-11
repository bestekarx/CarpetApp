# WebCarpetApp - Entity İlişkileri ve Veritabanı Şeması

## Entity Relationship Diagram (ERD)

### Ana İş Varlıkları ve İlişkileri

```
Companies (1) -----> (n) Customers
    |                      |
    |                      |
    v                      v
MessageConfigurations   Areas (1) -----> (n) Customers
    |
    |
    v
MessageUsers (1) -----> (n) MessageConfigurations
    |
    v
MessageTemplates (n) -----> (1) MessageConfigurations
    |
    v
MessageTasks (n) -----> (1) MessageConfigurations

Customers (1) -----> (n) Receiveds
    |                      |
    |                      v
    |                  Vehicles (1) -----> (n) Receiveds
    |                      |
    v                      v
Orders (n) -----> (1) Receiveds
    |
    |
    v
OrderedProducts (n) -----> (1) Orders
    |                      |
    v                      v
Products (1) -----> (n) OrderedProducts
                           |
                           v
                    OrderImages (n) -----> (1) Orders

Orders (1) -----> (1) Invoices
    |
    v
Customers (1) -----> (n) Invoices

UserTenantMappings (n) -----> (1) AbpUsers
```

## Detaylı Entity Açıklamaları

### 1. Müşteri ve Coğrafi Yapı

#### Areas
```sql
- Id (PK): uniqueidentifier
- TenantId: uniqueidentifier (FK to AbpTenants)
- Name: nvarchar (Bölge adı)
- Active: bit (Aktif/Pasif)
- Audit fields: CreationTime, CreatorId, LastModificationTime, LastModifierId
```

#### Companies
```sql
- Id (PK): uniqueidentifier
- TenantId: uniqueidentifier (FK to AbpTenants)
- Name: nvarchar (Şirket adı)
- Description: nvarchar (Açıklama)
- Color: nvarchar (Tema rengi)
- Active: bit (Aktif/Pasif)
- Audit fields: CreationTime, CreatorId, LastModificationTime, LastModifierId
```

#### Customers
```sql
- Id (PK): uniqueidentifier
- TenantId: uniqueidentifier (FK to AbpTenants)
- AreaId: uniqueidentifier (FK to Areas)
- CompanyId: uniqueidentifier (FK to Companies)
- UserId: uniqueidentifier (FK to AbpUsers - nullable)
- FullName: nvarchar (Müşteri tam adı)
- Phone: nvarchar (Telefon)
- CountryCode: nvarchar (Ülke kodu)
- Gsm: nvarchar (GSM)
- Address: nvarchar (Adres)
- Coordinate: nvarchar (GPS koordinatları - nullable)
- Balance: decimal (Müşteri bakiye)
- Active: bit (Aktif/Pasif)
- CompanyPermission: bit (Şirket iznine sahip mi?)
- IsConfirmed: bit (Onaylanmış mı?)
- ConfirmedAt: datetime2 (Onaylanma tarihi - nullable)
- Audit fields: CreationTime, CreatorId, LastModificationTime, LastModifierId
```

### 2. Sipariş ve Ürün Yönetimi

#### Products
```sql
- Id (PK): uniqueidentifier
- TenantId: uniqueidentifier (FK to AbpTenants)
- Price: decimal (Ürün fiyatı)
- Name: nvarchar (Ürün adı)
- ProductType: int (Enum: Service=0, Product=1, Fason=2, SeatClean=3)
- Active: bit (Aktif/Pasif)
- Audit fields: CreationTime, CreatorId, LastModificationTime, LastModifierId
```

#### Vehicles
```sql
- Id (PK): uniqueidentifier
- TenantId: uniqueidentifier (FK to AbpTenants)
- VehicleName: nvarchar (Araç adı)
- PlateNumber: nvarchar (Plaka numarası)
- Active: bit (Aktif/Pasif)
- Audit fields: CreationTime, CreatorId, LastModificationTime, LastModifierId
```

#### Receiveds (Alım Kayıtları)
```sql
- Id (PK): uniqueidentifier
- TenantId: uniqueidentifier (FK to AbpTenants)
- VehicleId: uniqueidentifier (FK to Vehicles)
- CustomerId: uniqueidentifier (FK to Customers)
- Status: int (Enum: Active=0, Passive=1)
- Type: int (Enum: Pickup=0, Delivery=1)
- Note: nvarchar (Not - nullable)
- RowNumber: int (Sıra numarası)
- Active: bit (Aktif/Pasif)
- PickupDate: datetime2 (Alım tarihi)
- DeliveryDate: datetime2 (Teslimat tarihi)
- FicheNo: nvarchar (Fiş numarası - nullable)
- ExtraProperties: nvarchar (JSON ek özellikler)
- ConcurrencyStamp: nvarchar (Eşzamanlılık damgası)
- Audit fields: CreationTime, CreatorId, LastModificationTime, LastModifierId
```

#### Orders
```sql
- Id (PK): uniqueidentifier
- TenantId: uniqueidentifier (FK to AbpTenants)
- UserId: uniqueidentifier (FK to AbpUsers)
- ReceivedId: uniqueidentifier (FK to Receiveds - nullable)
- OrderDiscount: int (Sipariş indirimi)
- OrderAmount: decimal (Sipariş tutarı)
- OrderTotalPrice: decimal (Sipariş toplam fiyatı)
- OrderStatus: int (Enum: Passive=0, Active=1, InProcess=2, Completed=3, ReadyForDelivery=4, Delivered=5, Cancelled=6)
- OrderRowNumber: int (Sipariş sıra numarası)
- Active: bit (Aktif/Pasif)
- CalculatedUsed: bit (Hesaplama kullanıldı mı?)
- ExtraProperties: nvarchar (JSON ek özellikler)
- ConcurrencyStamp: nvarchar (Eşzamanlılık damgası)
- Audit fields: CreationTime, CreatorId, LastModificationTime, LastModifierId
```

#### OrderedProducts (Sipariş Kalemleri)
```sql
- Id (PK): uniqueidentifier
- TenantId: uniqueidentifier (FK to AbpTenants)
- OrderId: uniqueidentifier (FK to Orders)
- ProductId: uniqueidentifier (FK to Products)
- ProductName: nvarchar (Ürün adı snapshot)
- ProductPrice: decimal (Ürün fiyatı snapshot)
- Number: int (Adet)
- SquareMeter: int (Metrekare)
```

#### OrderImages
```sql
- Id (PK): uniqueidentifier
- TenantId: uniqueidentifier (FK to AbpTenants)
- OrderId: uniqueidentifier (FK to Orders)
- BlobId: uniqueidentifier (FK to AbpBlobs)
```

### 3. Mali Yapı

#### Invoices
```sql
- Id (PK): uniqueidentifier
- TenantId: uniqueidentifier (FK to AbpTenants)
- OrderId: uniqueidentifier (FK to Orders)
- UserId: uniqueidentifier (FK to AbpUsers - nullable)
- CustomerId: uniqueidentifier (FK to Customers)
- TotalPrice: decimal (Toplam fiyat)
- PaidPrice: decimal (Ödenen tutar)
- PaymentType: int (Enum: Cash=0, CreditCard=1, BankTransfer=2, Check=3)
- InvoiceNote: nvarchar (Fatura notu - nullable)
- Active: bit (Aktif/Pasif)
- UpdatedUserId: uniqueidentifier (Güncelleyen kullanıcı - nullable)
- ExtraProperties: nvarchar (JSON ek özellikler)
- ConcurrencyStamp: nvarchar (Eşzamanlılık damgası)
- Audit fields: CreationTime, CreatorId, LastModificationTime, LastModifierId
```

### 4. Mesajlaşma Sistemi

#### MessageUsers
```sql
- Id (PK): uniqueidentifier
- TenantId: uniqueidentifier (FK to AbpTenants)
- Username: nvarchar (SMS servis kullanıcı adı)
- Password: nvarchar (SMS servis şifresi)
- Title: nvarchar (Başlık)
- Active: bit (Aktif/Pasif)
```

#### MessageConfigurations
```sql
- Id (PK): uniqueidentifier
- TenantId: uniqueidentifier (FK to AbpTenants)
- CompanyId: uniqueidentifier (FK to Companies)
- MessageUserId: uniqueidentifier (FK to MessageUsers)
- Name: nvarchar (Yapılandırma adı)
- Description: nvarchar (Açıklama)
- Active: bit (Aktif/Pasif)
- ExtraProperties: nvarchar (JSON ek özellikler)
- ConcurrencyStamp: nvarchar (Eşzamanlılık damgası)
```

#### MessageTemplates
```sql
- Id (PK): uniqueidentifier
- TenantId: uniqueidentifier (FK to AbpTenants)
- MessageConfigurationId: uniqueidentifier (FK to MessageConfigurations)
- TaskType: int (MessageTaskType enum)
- Name: nvarchar (Şablon adı)
- Template: nvarchar (Mesaj şablonu)
- PlaceholderMappings: nvarchar (JSON placeholder mappings)
- Active: bit (Aktif/Pasif)
- CultureCode: nvarchar (Kültür kodu)
```

#### MessageTasks
```sql
- Id (PK): uniqueidentifier
- MessageConfigurationId: uniqueidentifier (FK to MessageConfigurations)
- TaskType: int (Enum: ReceivedCreated=0, ReceivedCancelled=1, OrderCreated=2, OrderCompleted=3, OrderCancelled=4, InvoiceCreated=5, InvoicePaid=6)
- Behavior: int (Enum: AlwaysSend=0, NeverSend=1, AskBeforeSend=2)
- CustomMessage: nvarchar (Özel mesaj)
- Active: bit (Aktif/Pasif)
```

### 5. Operasyonel Destek

#### Printers
```sql
- Id (PK): uniqueidentifier
- TenantId: uniqueidentifier (FK to AbpTenants)
- Name: nvarchar (Yazıcı adı)
- MacAddress: nvarchar (MAC adresi)
- Audit fields: CreationTime, CreatorId, LastModificationTime, LastModifierId
```

#### UserTenantMappings
```sql
- Id (PK): uniqueidentifier
- UserId: uniqueidentifier (FK to AbpUsers)
- Active: bit (Aktif/Pasif)
- CreationTime: datetime2 (Oluşturma zamanı)
- CarpetTenantId: uniqueidentifier (Carpet tenant ID - nullable)
```

## İlişki Kuralları ve Kısıtlamaları

### Foreign Key Kısıtlamaları
1. **Customers.AreaId** → Areas.Id (NOT NULL)
2. **Customers.CompanyId** → Companies.Id (NOT NULL)
3. **Customers.UserId** → AbpUsers.Id (NULLABLE)
4. **Receiveds.VehicleId** → Vehicles.Id (NOT NULL)
5. **Receiveds.CustomerId** → Customers.Id (NOT NULL)
6. **Orders.ReceivedId** → Receiveds.Id (NULLABLE)
7. **OrderedProducts.OrderId** → Orders.Id (NOT NULL)
8. **OrderedProducts.ProductId** → Products.Id (NOT NULL)
9. **OrderImages.OrderId** → Orders.Id (NOT NULL)
10. **Invoices.OrderId** → Orders.Id (NOT NULL)
11. **Invoices.CustomerId** → Customers.Id (NOT NULL)
12. **MessageConfigurations.CompanyId** → Companies.Id (NOT NULL)
13. **MessageConfigurations.MessageUserId** → MessageUsers.Id (NOT NULL)
14. **MessageTemplates.MessageConfigurationId** → MessageConfigurations.Id (NOT NULL)
15. **MessageTasks.MessageConfigurationId** → MessageConfigurations.Id (NOT NULL)

### İş Kuralları
1. **Customer Balance**: Müşteri bakiyesi negatif olabilir (borç durumu)
2. **Order Status Progression**: Belirli bir sıra takip edilmelidir
3. **FicheNo Generation**: Tenant başına benzersiz ve sıralı olmalıdır
4. **Message Template Placeholders**: Geçerli placeholder'lar kullanılmalıdır
5. **Multi-Tenancy**: Tüm entity'ler tenant izolasyonuna sahiptir
6. **Soft Delete**: Belirli entity'ler için soft delete uygulanır

### İndeksler ve Performans
1. **Customers**: AreaId, CompanyId, Phone üzerinde indeks
2. **Orders**: ReceivedId, UserId, OrderStatus üzerinde indeks
3. **Receiveds**: CustomerId, VehicleId, PickupDate üzerinde indeks
4. **OrderedProducts**: OrderId, ProductId üzerinde indeks
5. **Invoices**: CustomerId, OrderId, CreationTime üzerinde indeks
6. **MessageTemplates**: MessageConfigurationId, TaskType üzerinde indeks