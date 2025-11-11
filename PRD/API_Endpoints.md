# WebCarpetApp - API Endpoints Documentation

## Base URL
- Development: `https://localhost:5001`
- Production: `[To be configured]`

## Authentication
Tüm API endpoint'leri JWT Bearer token authentication kullanır.
```
Authorization: Bearer <your-jwt-token>
```

## API Endpoint'leri

### 🔐 Authentication & Authorization

#### POST /api/account/login
Kullanıcı girişi
```json
Request:
{
  "userNameOrEmailAddress": "string",
  "password": "string",
  "rememberMe": true
}

Response:
{
  "accessToken": "string",
  "refreshToken": "string",
  "encryptedAccessToken": "string",
  "expireInSeconds": 0,
  "userId": "guid"
}
```

#### POST /api/account/register
Yeni kullanıcı kaydı
```json
Request:
{
  "userName": "string",
  "name": "string",
  "surname": "string",
  "emailAddress": "string",
  "password": "string"
}
```

### 🏢 Tenant Management

#### GET /api/multi-tenancy/tenants
Tenant listesi
```json
Response:
{
  "items": [
    {
      "id": "guid",
      "name": "string",
      "normalizedName": "string",
      "isActive": true
    }
  ]
}
```

#### POST /api/multi-tenancy/tenants
Yeni tenant oluşturma
```json
Request:
{
  "name": "string",
  "adminEmailAddress": "string",
  "adminPassword": "string"
}
```

### 🏠 Areas (Bölgeler)

#### GET /api/app/areas
Bölge listesi
```json
Query Parameters:
- Sorting: string (optional)
- SkipCount: int (default: 0)
- MaxResultCount: int (default: 10)
- Filter: string (optional)

Response:
{
  "items": [
    {
      "id": "guid",
      "tenantId": "guid",
      "name": "string",
      "active": true,
      "creationTime": "datetime",
      "creatorId": "guid"
    }
  ],
  "totalCount": 0
}
```

#### POST /api/app/areas
Yeni bölge oluşturma
```json
Request:
{
  "name": "string",
  "active": true
}

Response:
{
  "id": "guid",
  "name": "string",
  "active": true
}
```

#### PUT /api/app/areas/{id}
Bölge güncelleme
```json
Request:
{
  "name": "string",
  "active": true
}
```

#### DELETE /api/app/areas/{id}
Bölge silme

### 🏢 Companies (Şirketler)

#### GET /api/app/companies
Şirket listesi
```json
Response:
{
  "items": [
    {
      "id": "guid",
      "tenantId": "guid",
      "name": "string",
      "description": "string",
      "color": "string",
      "active": true
    }
  ]
}
```

#### POST /api/app/companies
Yeni şirket oluşturma
```json
Request:
{
  "name": "string",
  "description": "string",
  "color": "#FF0000",
  "active": true
}
```

### 👥 Customers (Müşteriler)

#### GET /api/app/customers
Müşteri listesi
```json
Query Parameters:
- Filter: string (optional)
- AreaId: guid (optional)
- CompanyId: guid (optional)
- Active: bool (optional)
- Sorting: string (optional)
- SkipCount: int (default: 0)
- MaxResultCount: int (default: 10)

Response:
{
  "items": [
    {
      "id": "guid",
      "areaId": "guid",
      "areaName": "string",
      "companyId": "guid",
      "companyName": "string",
      "fullName": "string",
      "phone": "string",
      "gsm": "string",
      "address": "string",
      "coordinate": "string",
      "balance": 0.0,
      "active": true,
      "isConfirmed": true,
      "confirmedAt": "datetime"
    }
  ],
  "totalCount": 0
}
```

#### POST /api/app/customers
Yeni müşteri oluşturma
```json
Request:
{
  "areaId": "guid",
  "companyId": "guid",
  "fullName": "string",
  "phone": "string",
  "countryCode": "+90",
  "gsm": "string",
  "address": "string",
  "coordinate": "string",
  "balance": 0.0,
  "active": true,
  "companyPermission": false
}
```

#### PUT /api/app/customers/{id}
Müşteri güncelleme

#### DELETE /api/app/customers/{id}
Müşteri silme

#### POST /api/app/customers/{id}/confirm
Müşteri onaylama

### 📦 Products (Ürünler)

#### GET /api/app/products
Ürün listesi
```json
Query Parameters:
- ProductType: int (optional) // 0=Service, 1=Product, 2=Fason, 3=SeatClean
- Active: bool (optional)

Response:
{
  "items": [
    {
      "id": "guid",
      "price": 0.0,
      "name": "string",
      "productType": 0,
      "productTypeName": "Service",
      "active": true
    }
  ]
}
```

#### POST /api/app/products
Yeni ürün oluşturma
```json
Request:
{
  "price": 100.0,
  "name": "Halı Yıkama",
  "productType": 0, // Service
  "active": true
}
```

### 🚛 Vehicles (Araçlar)

#### GET /api/app/vehicles
Araç listesi
```json
Response:
{
  "items": [
    {
      "id": "guid",
      "vehicleName": "string",
      "plateNumber": "string",
      "active": true
    }
  ]
}
```

#### POST /api/app/vehicles
Yeni araç oluşturma
```json
Request:
{
  "vehicleName": "Mercedes Sprinter",
  "plateNumber": "34 ABC 123",
  "active": true
}
```

### 📋 Receiveds (Alım Kayıtları)

#### GET /api/app/receiveds
Alım kayıtları listesi
```json
Query Parameters:
- CustomerId: guid (optional)
- VehicleId: guid (optional)
- Status: int (optional) // 0=Active, 1=Passive
- StartDate: datetime (optional)
- EndDate: datetime (optional)

Response:
{
  "items": [
    {
      "id": "guid",
      "vehicleId": "guid",
      "vehicleName": "string",
      "customerId": "guid",
      "customerName": "string",
      "status": 0,
      "statusName": "Active",
      "type": 0, // 0=Pickup, 1=Delivery
      "typeName": "Pickup",
      "note": "string",
      "rowNumber": 1,
      "active": true,
      "pickupDate": "datetime",
      "deliveryDate": "datetime",
      "ficheNo": "string"
    }
  ]
}
```

#### POST /api/app/receiveds
Yeni alım kaydı oluşturma
```json
Request:
{
  "vehicleId": "guid",
  "customerId": "guid",
  "status": 0,
  "type": 0,
  "note": "string",
  "pickupDate": "datetime",
  "deliveryDate": "datetime",
  "active": true
}
```

#### GET /api/app/receiveds/{id}/generate-fiche-no
Fiş numarası üretme

### 📄 Orders (Siparişler)

#### GET /api/app/orders
Sipariş listesi
```json
Query Parameters:
- ReceivedId: guid (optional)
- OrderStatus: int (optional) // 0=Passive, 1=Active, 2=InProcess, 3=Completed, 4=ReadyForDelivery, 5=Delivered, 6=Cancelled
- StartDate: datetime (optional)
- EndDate: datetime (optional)

Response:
{
  "items": [
    {
      "id": "guid",
      "userId": "guid",
      "receivedId": "guid",
      "ficheNo": "string",
      "customerName": "string",
      "orderDiscount": 0,
      "orderAmount": 0.0,
      "orderTotalPrice": 0.0,
      "orderStatus": 1,
      "orderStatusName": "Active",
      "orderRowNumber": 1,
      "active": true,
      "calculatedUsed": false,
      "orderedProducts": [
        {
          "id": "guid",
          "productId": "guid",
          "productName": "string",
          "productPrice": 0.0,
          "number": 1,
          "squareMeter": 10
        }
      ]
    }
  ]
}
```

#### POST /api/app/orders
Yeni sipariş oluşturma
```json
Request:
{
  "receivedId": "guid",
  "orderDiscount": 0,
  "orderAmount": 100.0,
  "orderTotalPrice": 100.0,
  "orderStatus": 1,
  "active": true,
  "orderedProducts": [
    {
      "productId": "guid",
      "productName": "Halı Yıkama",
      "productPrice": 50.0,
      "number": 2,
      "squareMeter": 20
    }
  ]
}
```

#### PUT /api/app/orders/{id}
❌ **ŞU ANDA ÇALIŞMIYOR** - NotImplementedException
Sipariş güncelleme

#### PUT /api/app/orders/{id}/status
Sipariş durumu güncelleme
```json
Request:
{
  "orderStatus": 2 // InProcess
}
```

### 🧾 Invoices (Faturalar)

#### GET /api/app/invoices
Fatura listesi
```json
Query Parameters:
- CustomerId: guid (optional)
- OrderId: guid (optional)
- PaymentType: int (optional) // 0=Cash, 1=CreditCard, 2=BankTransfer, 3=Check
- StartDate: datetime (optional)
- EndDate: datetime (optional)

Response:
{
  "items": [
    {
      "id": "guid",
      "orderId": "guid",
      "customerId": "guid",
      "customerName": "string",
      "totalPrice": 0.0,
      "paidPrice": 0.0,
      "remainingAmount": 0.0,
      "paymentType": 0,
      "paymentTypeName": "Cash",
      "invoiceNote": "string",
      "active": true
    }
  ]
}
```

#### POST /api/app/invoices
Yeni fatura oluşturma
```json
Request:
{
  "orderId": "guid",
  "customerId": "guid",
  "totalPrice": 100.0,
  "paidPrice": 50.0,
  "paymentType": 0, // Cash
  "invoiceNote": "string",
  "active": true
}
```

### 📨 Message System (Mesajlaşma)

#### GET /api/app/message-users
Mesaj kullanıcıları
```json
Response:
{
  "items": [
    {
      "id": "guid",
      "username": "string",
      "title": "string",
      "active": true
    }
  ]
}
```

#### GET /api/app/message-configurations
Mesaj yapılandırmaları
```json
Response:
{
  "items": [
    {
      "id": "guid",
      "companyId": "guid",
      "messageUserId": "guid",
      "name": "string",
      "description": "string",
      "active": true
    }
  ]
}
```

#### GET /api/app/message-templates
Mesaj şablonları
```json
Response:
{
  "items": [
    {
      "id": "guid",
      "messageConfigurationId": "guid",
      "taskType": 0, // ReceivedCreated
      "name": "string",
      "template": "string",
      "placeholderMappings": {},
      "active": true,
      "cultureCode": "tr-TR"
    }
  ]
}
```

#### GET /api/app/message-tasks
Mesaj görevleri
```json
Response:
{
  "items": [
    {
      "id": "guid",
      "messageConfigurationId": "guid",
      "taskType": 0,
      "behavior": 0, // AlwaysSend
      "customMessage": "string",
      "active": true
    }
  ]
}
```

### 🖨️ Printers (Yazıcılar)

#### GET /api/app/printers
Yazıcı listesi
```json
Response:
{
  "items": [
    {
      "id": "guid",
      "name": "string",
      "macAddress": "string"
    }
  ]
}
```

#### POST /api/app/printers
Yeni yazıcı ekleme
```json
Request:
{
  "name": "HP LaserJet",
  "macAddress": "00:11:22:33:44:55"
}
```

### 📁 File Management (Dosya Yönetimi)

#### POST /api/app/order-images
Sipariş resmi yükleme
```json
Content-Type: multipart/form-data

Form Data:
- OrderId: guid
- File: file

Response:
{
  "id": "guid",
  "orderId": "guid",
  "blobId": "guid",
  "fileName": "string",
  "fileSize": 0
}
```

#### GET /api/app/order-images/{orderId}
Siparişe ait resimleri getirme

#### DELETE /api/app/order-images/{id}
Sipariş resmini silme

## Error Responses

Tüm endpoint'ler hata durumunda standart ABP error response formatını kullanır:

```json
{
  "error": {
    "code": "string",
    "message": "string",
    "details": "string",
    "data": {},
    "validationErrors": [
      {
        "message": "string",
        "members": ["string"]
      }
    ]
  }
}
```

## Status Codes

- **200 OK**: Başarılı işlem
- **201 Created**: Kaynak başarıyla oluşturuldu
- **204 No Content**: Başarılı işlem, content yok
- **400 Bad Request**: Geçersiz istek
- **401 Unauthorized**: Kimlik doğrulama gerekli
- **403 Forbidden**: Yetki yok
- **404 Not Found**: Kaynak bulunamadı
- **500 Internal Server Error**: Sunucu hatası

## Rate Limiting

API rate limiting henüz implement edilmemiş. Gelecek versiyonlarda eklenecek.

## Versioning

API versioning henüz implement edilmemiş. Mevcut versiyon v1.0 olarak kabul edilebilir.