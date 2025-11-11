# WebCarpetApp - Hızlı Başlangıç Rehberi

## 🚀 HEMEN ŞİMDİ YAPILABİLECEKLER

### ⚡ 1 Saatte Yapılabilecekler

#### 🔧 Order Update Metodunu Düzelt
**Dosya**: `src/WebCarpetApp.Application/Orders/OrderAppService.cs`
**Satır**: 89

```csharp
// ŞU ANKİ DURUM (BROKEN):
public override async Task<OrderDto> UpdateAsync(Guid id, OrderUpdateDto input)
{
    throw new NotImplementedException(); // ← Bu satırı değiştir!
}

// HIZLI ÇÖZüM:
public override async Task<OrderDto> UpdateAsync(Guid id, OrderUpdateDto input)
{
    var order = await Repository.GetAsync(id);

    // Basic validation
    if (input.OrderStatus != order.OrderStatus)
    {
        // Status değişimi için business rules eklenebilir
        order.OrderStatus = input.OrderStatus;
    }

    if (input.OrderDiscount != order.OrderDiscount)
    {
        order.OrderDiscount = input.OrderDiscount;
        // Fiyat yeniden hesaplama
        order.OrderTotalPrice = order.OrderAmount - (order.OrderAmount * input.OrderDiscount / 100);
    }

    await Repository.UpdateAsync(order);
    return await MapToGetOutputDtoAsync(order);
}
```

#### 📝 OrderUpdateDto Oluştur
**Dosya**: `src/WebCarpetApp.Application.Contracts/Orders/OrderUpdateDto.cs`

```csharp
public class OrderUpdateDto
{
    public int OrderDiscount { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public string? Note { get; set; }
}
```

### ⚡ 2 Saatte Yapılabilecekler

#### 📱 SMS Test Implementation
**Dosya**: `src/WebCarpetApp.Domain/Messaging/MessageSender.cs`

```csharp
// HIZLI TEST ÇÖZüMü (Console log):
public async Task<bool> SendSmsAsync(string phoneNumber, string message)
{
    try
    {
        // Development ortamında console'a yazdır
        Logger.LogInformation($"SMS Sent to {phoneNumber}: {message}");

        // TODO: Production için gerçek SMS provider ekle
        // await _smsProvider.SendAsync(phoneNumber, message);

        return await Task.FromResult(true);
    }
    catch (Exception ex)
    {
        Logger.LogError(ex, "SMS sending failed");
        return false;
    }
}
```

### ⚡ 1 Günde Yapılabilecekler

#### 🔌 İletimerkezi SMS Integration
**NuGet Package**: İletimerkezi.SMS

```csharp
// appsettings.json'a ekle:
{
  "SMS": {
    "Provider": "Iletimerkezi",
    "Username": "your_username",
    "Password": "your_password",
    "Sender": "CARPET"
  }
}

// MessageSender.cs implementation:
public async Task<bool> SendSmsAsync(string phoneNumber, string message)
{
    try
    {
        var client = new IletimerkziClient(_configuration["SMS:Username"], _configuration["SMS:Password"]);
        var result = await client.SendSms(phoneNumber, message, _configuration["SMS:Sender"]);
        return result.Success;
    }
    catch (Exception ex)
    {
        Logger.LogError(ex, "SMS sending failed");
        return false;
    }
}
```

## 📊 DEMO SCENARIO'LAR

### 🎯 Demo 1: Müşteri Kayıt ve Sipariş Akışı

#### Adım 1: Şirket ve Bölge Oluştur
```bash
# POST /api/app/companies
{
  "name": "Temiz Halı A.Ş.",
  "description": "Professional halı yıkama",
  "color": "#3498db",
  "active": true
}

# POST /api/app/areas
{
  "name": "Kadıköy",
  "active": true
}
```

#### Adım 2: Müşteri Ekle
```bash
# POST /api/app/customers
{
  "areaId": "guid_from_step1",
  "companyId": "guid_from_step1",
  "fullName": "Ahmet Yılmaz",
  "phone": "02161234567",
  "gsm": "05551234567",
  "address": "Kadıköy, İstanbul",
  "balance": 0
}
```

#### Adım 3: Araç ve Ürün Ekle
```bash
# POST /api/app/vehicles
{
  "vehicleName": "Ford Transit",
  "plateNumber": "34 XYZ 123"
}

# POST /api/app/products
{
  "name": "Standart Halı Yıkama",
  "price": 15.00,
  "productType": 0
}
```

#### Adım 4: Halı Alım Kaydı
```bash
# POST /api/app/receiveds
{
  "vehicleId": "guid",
  "customerId": "guid",
  "pickupDate": "2025-01-04T10:00:00",
  "deliveryDate": "2025-01-06T15:00:00",
  "type": 0,
  "status": 0
}
```

#### Adım 5: Sipariş Oluştur
```bash
# POST /api/app/orders
{
  "receivedId": "guid",
  "orderDiscount": 10,
  "orderAmount": 150.00,
  "orderTotalPrice": 135.00,
  "orderStatus": 1,
  "orderedProducts": [
    {
      "productId": "guid",
      "productName": "Standart Halı Yıkama",
      "productPrice": 15.00,
      "number": 10,
      "squareMeter": 100
    }
  ]
}
```

### 🎯 Demo 2: Mesajlaşma Sistemi Test

#### MessageUser Oluştur
```bash
# POST /api/app/message-users
{
  "username": "sms_user",
  "password": "sms_pass",
  "title": "Ana SMS Hesabı"
}
```

#### MessageConfiguration
```bash
# POST /api/app/message-configurations
{
  "companyId": "guid",
  "messageUserId": "guid",
  "name": "Müşteri Bildirimleri",
  "description": "Sipariş durumu bildirimleri"
}
```

#### MessageTemplate
```bash
# POST /api/app/message-templates
{
  "messageConfigurationId": "guid",
  "taskType": 2,
  "name": "Sipariş Oluşturuldu",
  "template": "Sayın {{CustomerName}}, siparişiniz {{FicheNo}} numarası ile oluşturulmuştur.",
  "cultureCode": "tr-TR"
}
```

## 🔧 DEVELOPMENT ENVIRONMENT SETUP

### 📋 Gereksinimler
- ✅ .NET 9 SDK
- ✅ SQL Server 2019+
- ✅ Visual Studio 2022 / VS Code
- ✅ Git

### 🚀 Hızlı Kurulum

#### 1. Database Setup
```bash
# Connection string zaten hazır:
Server=localhost;Database=CarpetApp;User Id=sa;Password=QWEqwe123*;TrustServerCertificate=True

# Migration zaten çalıştırıldı ✅
dotnet ef database update
```

#### 2. API Çalıştır
```bash
cd src/WebCarpetApp.HttpApi.Host
dotnet run --urls="https://localhost:5001;http://localhost:5000"
```

#### 3. Test
```bash
curl https://localhost:5001/api/app/companies
# Ya da browser'da: https://localhost:5001/swagger
```

## 📱 MOBİL UYGULAMA BAŞLANGICI

### 🎯 React Native Setup (Recommended)

#### Prerequisites
```bash
npm install -g react-native-cli
npm install -g expo-cli
```

#### Project Init
```bash
npx react-native init CarpetApp
cd CarpetApp

# Required packages
npm install @react-navigation/native
npm install react-native-maps
npm install react-native-image-picker
npm install @react-native-async-storage/async-storage
```

#### API Integration
```javascript
// services/api.js
const API_BASE = 'https://localhost:5001/api';

export const carpetAPI = {
  async getOrders() {
    const response = await fetch(`${API_BASE}/app/orders`);
    return response.json();
  },

  async updateOrderStatus(orderId, status) {
    // Order update fix'i tamamlandıktan sonra çalışacak
    const response = await fetch(`${API_BASE}/app/orders/${orderId}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ orderStatus: status })
    });
    return response.json();
  }
};
```

## 🎨 FRONTEND DEVELOPMENT

### 🌐 Customer Portal (React)

#### Quick Start
```bash
npx create-react-app carpet-customer-portal
cd carpet-customer-portal

# UI Framework
npm install @mui/material @emotion/react @emotion/styled
npm install @mui/icons-material
npm install axios react-router-dom
```

#### Sample Component
```jsx
// components/OrderTracking.jsx
import React, { useState, useEffect } from 'react';
import { Card, CardContent, Typography, Stepper, Step, StepLabel } from '@mui/material';

const OrderTracking = ({ orderId }) => {
  const [order, setOrder] = useState(null);

  useEffect(() => {
    fetch(`/api/app/orders/${orderId}`)
      .then(res => res.json())
      .then(setOrder);
  }, [orderId]);

  const steps = ['Alındı', 'İşlemde', 'Tamamlandı', 'Teslimata Hazır', 'Teslim Edildi'];

  return (
    <Card>
      <CardContent>
        <Typography variant="h6">Sipariş Takibi</Typography>
        <Stepper activeStep={order?.orderStatus || 0}>
          {steps.map((label) => (
            <Step key={label}>
              <StepLabel>{label}</StepLabel>
            </Step>
          ))}
        </Stepper>
      </CardContent>
    </Card>
  );
};
```

## 📊 REPORTING DASHBOARD

### 📈 Quick Analytics

#### Basic Reports API
```csharp
// src/WebCarpetApp.Application/Reports/ReportAppService.cs
public class ReportAppService : ApplicationService
{
    public async Task<DashboardDto> GetDashboardAsync()
    {
        var orders = await _orderRepository.GetListAsync();
        var customers = await _customerRepository.GetListAsync();

        return new DashboardDto
        {
            TotalOrders = orders.Count,
            ActiveOrders = orders.Count(x => x.OrderStatus == OrderStatus.Active),
            TotalRevenue = orders.Sum(x => x.OrderTotalPrice),
            TotalCustomers = customers.Count
        };
    }
}
```

#### Chart.js Integration
```javascript
// Dashboard component
import { Line, Bar, Doughnut } from 'react-chartjs-2';

const Dashboard = () => {
  const [data, setData] = useState(null);

  const chartData = {
    labels: ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs'],
    datasets: [{
      label: 'Aylık Sipariş',
      data: [12, 19, 3, 5, 2],
      backgroundColor: 'rgba(54, 162, 235, 0.2)'
    }]
  };

  return (
    <div className="dashboard">
      <Line data={chartData} />
      <Bar data={chartData} />
    </div>
  );
};
```

## 🎉 SONUÇ

### ✅ Hemen Yapılabilecek Minimum Viable Product:
1. **Order Update fix** (1 saat)
2. **SMS console logging** (30 dakika)
3. **Basic testing** (1 saat)

### 🚀 1 Hafta İçinde Full Production:
1. **SMS provider integration**
2. **Payment processing completion**
3. **Comprehensive testing**

### 📱 1 Ay İçinde Mobile App:
1. **React Native driver app**
2. **Customer tracking portal**
3. **Real-time notifications**

**WebCarpetApp, hemen şimdi kullanılmaya başlanabilir ve adım adım geliştirilebilir!** 🎯