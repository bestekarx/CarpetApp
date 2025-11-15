# WebCarpetApp API Entegrasyon Kalıpları ve En İyi Uygulamalar

## API İstek Kalıpları

### 🎯 Standardize API Yanıt Formatı

**Tüm API endpoint'leri aynı format kullanır:**
```typescript
interface ApiResponse<T> {
  success: boolean;
  data: T | null;
  message: string;
  error?: {
    code: string;
    details?: any;
  };
  timestamp: string;
}
```

### **📋 Dual Method Pattern**
Her entity için iki tip method mevcuttur:

1. **Legacy Methods** (Geriye uyumluluk için)
   - `GetListAsync()`, `CreateAsync()`, `UpdateAsync()`
   - Direkt entity dönüş yapar

2. **Standardize Methods** (Yeni geliştirme için **önerilen**)
   - `GetListWithResponseAsync()`, `CreateWithResponseAsync()`
   - `ApiResponse<T>` formatında dönüş yapar

## Business Entity Integration Patterns

### 🏢 Customer Management

#### **Müşteri Listesi Alma**
```typescript
GET /api/app/customer/list-with-response?maxResultCount=20&skipCount=0

// Query Parameters
interface CustomerListRequest {
  maxResultCount?: number;  // Default: 10
  skipCount?: number;      // Default: 0
  sorting?: string;        // "Name asc", "CreationTime desc"
}

// Response
interface CustomerListResponse {
  items: CustomerDto[];
  totalCount: number;
}
```

#### **Filtered Müşteri Listesi**
```typescript
POST /api/app/customer/filtered-list-with-response
Content-Type: application/json

{
  "name": "arama terimi",
  "active": true,
  "maxResultCount": 20,
  "skipCount": 0
}
```

#### **Müşteri Oluşturma (Business Validation ile)**
```typescript
POST /api/app/customer/create-with-response
Content-Type: application/json

{
  "name": "Müşteri Adı",
  "phone": "05551234567",
  "email": "customer@example.com",
  "address": "Adres bilgisi",
  "coordinates": "41.0082,28.9784", // GPS koordinatları
  "areaId": "area-guid-here",
  "companyId": "company-guid-here",
  "active": true
}

// Success Response
{
  "success": true,
  "data": {
    "id": "new-customer-guid",
    "name": "Müşteri Adı",
    "phone": "05551234567",
    // ... diğer fields
  },
  "message": "Müşteri başarıyla oluşturuldu"
}

// Validation Error Response
{
  "success": false,
  "message": "Bu telefon numarası zaten kullanımda",
  "error": {
    "code": "CUSTOMER_DUPLICATE_PHONE"
  }
}
```

#### **GPS Konum Güncelleme**
```typescript
PUT /api/app/customer/update-with-response/{customerId}
Content-Type: application/json

{
  // Sadece güncellenecek alanlar
  "coordinates": "41.0123,28.9756"
}
```

### 📦 Order Management

#### **Sipariş Listesi (Sayfalama ile)**
```typescript
GET /api/app/order/list-with-response?sorting=CreationTime desc&maxResultCount=50

// Response
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "order-guid",
        "customerId": "customer-guid",
        "customerName": "Müşteri Adı",
        "areaId": "area-guid",
        "areaName": "Bölge Adı",
        "orderStatus": "Active",
        "deliveryDate": "2025-11-15T15:00:00Z",
        "totalAmount": 1250.00,
        "products": [
          {
            "productId": "product-guid",
            "productName": "Ürün Adı",
            "quantity": 5,
            "unitPrice": 250.00
          }
        ]
      }
    ],
    "totalCount": 156
  }
}
```

#### **En Yakın Siparişler (GPS Tabanlı)**
```typescript
POST /api/app/order/nearest-with-response
Content-Type: application/json

{
  "latitude": 41.0082,
  "longitude": 28.9784,
  "radiusKm": 5,
  "maxResults": 10
}

// Response
{
  "success": true,
  "data": [
    {
      "order": { /* Order details */ },
      "distance": 1.2,
      "customer": { /* Customer details */ }
    }
  ]
}
```

#### **Sipariş Oluşturma (Multi-Product)**
```typescript
POST /api/app/order/create-with-response
Content-Type: application/json

{
  "customerId": "customer-guid",
  "areaId": "area-guid",
  "receivedId": "received-guid",
  "deliveryDate": "2025-11-16T14:00:00Z",
  "description": "Sipariş notları",
  "priorityLevel": 2,
  "products": [
    {
      "productId": "product-1-guid",
      "quantity": 3,
      "unitPrice": 150.00
    },
    {
      "productId": "product-2-guid",
      "quantity": 1,
      "unitPrice": 500.00
    }
  ]
}
```

### 🚛 Vehicle Management

#### **Araç Filosu Listesi**
```typescript
GET /api/app/vehicle/filtered-list-with-response
Content-Type: application/json

{
  "name": "plaka veya araç adı",
  "active": true
}

// Response
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "vehicle-guid",
        "plateNumber": "34ABC123",
        "vehicleName": "Kamyonet 1",
        "driverName": "Şoför Adı",
        "active": true
      }
    ]
  }
}
```

### 📄 Invoice Management

#### **Fatura Oluşturma**
```typescript
POST /api/app/invoice/create-with-response
Content-Type: application/json

{
  "orderId": "order-guid",
  "vehicleId": "vehicle-guid",
  "deliveryDate": "2025-11-15T16:30:00Z",
  "notes": "Teslimat notları"
}
```

## Pagination ve Filtering

### **Sayfalama Pattern'i**
```typescript
// Request
interface PagedRequest {
  maxResultCount?: number; // Default: 10, Max: 1000
  skipCount?: number;      // Default: 0
  sorting?: string;        // "field asc|desc"
}

// Response
interface PagedResult<T> {
  items: T[];
  totalCount: number;
}

// Usage
const page = 2;
const pageSize = 20;
const request = {
  maxResultCount: pageSize,
  skipCount: (page - 1) * pageSize,
  sorting: "CreationTime desc"
};
```

### **Filtreleme Pattern'i**
```typescript
// Filtered liste için her entity kendi filter DTO'su kullanır
interface CustomerFilterDto extends PagedRequest {
  name?: string;
  active?: boolean;
}

interface OrderFilterDto extends PagedRequest {
  customerId?: string;
  areaId?: string;
  status?: OrderStatus;
  startDate?: string;
  endDate?: string;
}

interface ProductFilterDto extends PagedRequest {
  name?: string;
  type?: ProductType;
  active?: boolean;
}
```

## Caching Strategies

### **Client-Side Cache Patterns**
```typescript
class ApiCache {
  private cache = new Map();
  private cacheTimeout = 5 * 60 * 1000; // 5 dakika

  async get<T>(key: string, fetchFn: () => Promise<T>): Promise<T> {
    const cached = this.cache.get(key);

    if (cached && Date.now() - cached.timestamp < this.cacheTimeout) {
      return cached.data;
    }

    const data = await fetchFn();
    this.cache.set(key, {
      data,
      timestamp: Date.now()
    });

    return data;
  }

  invalidate(keyPattern: string) {
    for (const key of this.cache.keys()) {
      if (key.includes(keyPattern)) {
        this.cache.delete(key);
      }
    }
  }
}

// Usage
const apiCache = new ApiCache();

// Cache customer list
const customers = await apiCache.get('customers-page-1', () =>
  apiClient.get('/api/app/customer/list-with-response?maxResultCount=50')
);

// Cache invalidation after create/update
await apiClient.post('/api/app/customer/create-with-response', customerData);
apiCache.invalidate('customers'); // Tüm customer cache'lerini temizle
```

## Error Handling Patterns

### **Standart Error Response Handling**
```typescript
interface ApiError {
  code: string;
  message: string;
  details?: any;
}

class ApiResponseHandler {
  static handle<T>(response: ApiResponse<T>): T {
    if (!response.success) {
      throw new ApiError(response.error?.code || 'UNKNOWN_ERROR', response.message);
    }
    return response.data!;
  }

  static async safeCall<T>(apiCall: () => Promise<ApiResponse<T>>): Promise<T | null> {
    try {
      const response = await apiCall();
      return this.handle(response);
    } catch (error) {
      console.error('API call failed:', error);
      return null;
    }
  }
}

// Usage
const customer = ApiResponseHandler.handle(
  await apiClient.get('/api/app/customer/get-with-response/123')
);

// Safe call - return null on error
const customerOrNull = await ApiResponseHandler.safeCall(() =>
  apiClient.get('/api/app/customer/get-with-response/123')
);
```

### **Business Logic Error Codes**
```typescript
// Common error codes to handle in mobile app
const ERROR_CODES = {
  // Customer errors
  CUSTOMER_DUPLICATE_PHONE: 'Bu telefon numarası zaten kullanımda',
  CUSTOMER_NOT_FOUND: 'Müşteri bulunamadı',

  // Order errors
  ORDER_INVALID_DELIVERY_DATE: 'Geçersiz teslimat tarihi',
  ORDER_CUSTOMER_REQUIRED: 'Müşteri seçimi zorunlu',

  // Product errors
  PRODUCT_DUPLICATE_NAME: 'Bu ürün adı zaten kullanımda',
  PRODUCT_INVALID_PRICE: 'Geçersiz fiyat bilgisi',

  // Vehicle errors
  VEHICLE_DUPLICATE_PLATE: 'Bu plaka numarası zaten kayıtlı',
  VEHICLE_INVALID_PLATE: 'Geçersiz plaka formatı',

  // General errors
  VALIDATION_ERROR: 'Doğrulama hatası',
  AUTHORIZATION_ERROR: 'Yetki hatası',
  UNEXPECTED_ERROR: 'Beklenmeyen hata oluştu'
};
```

## Real-Time Data Patterns

### **Polling for Updates**
```typescript
class DataPolling {
  private intervals: Map<string, NodeJS.Timeout> = new Map();

  startPolling(key: string, fetchFn: () => Promise<any>, intervalMs: number = 30000) {
    const interval = setInterval(async () => {
      try {
        await fetchFn();
      } catch (error) {
        console.error(`Polling failed for ${key}:`, error);
      }
    }, intervalMs);

    this.intervals.set(key, interval);
  }

  stopPolling(key: string) {
    const interval = this.intervals.get(key);
    if (interval) {
      clearInterval(interval);
      this.intervals.delete(key);
    }
  }

  stopAllPolling() {
    for (const [key, interval] of this.intervals) {
      clearInterval(interval);
    }
    this.intervals.clear();
  }
}

// Usage - Sipariş durumu güncellemelerini takip et
const polling = new DataPolling();

polling.startPolling('active-orders', async () => {
  const orders = await apiClient.get('/api/app/order/filtered-list-with-response', {
    status: 'Active'
  });
  updateOrdersInStore(orders.data.items);
}, 30000); // Her 30 saniyede bir kontrol et
```

### **Optimistic Updates**
```typescript
class OptimisticUpdates {
  async updateCustomer(customerId: string, updates: Partial<CustomerDto>) {
    // 1. UI'yi hemen güncelle (optimistic)
    updateCustomerInUI(customerId, updates);

    try {
      // 2. API call yap
      const response = await apiClient.put(
        `/api/app/customer/update-with-response/${customerId}`,
        updates
      );

      // 3. Sunucu yanıtı ile UI'yi sync et
      updateCustomerInUI(customerId, response.data);

    } catch (error) {
      // 4. Hata durumunda geri al
      revertCustomerInUI(customerId);
      showError('Müşteri güncellenemedi');
    }
  }
}
```

## Mobile-Specific Optimizations

### **Batch Operations**
```typescript
class BatchProcessor {
  private batchQueue: Array<{id: string, operation: () => Promise<any>}> = [];
  private processing = false;

  addToBatch(id: string, operation: () => Promise<any>) {
    this.batchQueue.push({id, operation});
    this.processBatchIfNeeded();
  }

  private async processBatchIfNeeded() {
    if (this.processing || this.batchQueue.length === 0) return;

    this.processing = true;

    // Process in chunks of 5
    while (this.batchQueue.length > 0) {
      const batch = this.batchQueue.splice(0, 5);
      await Promise.allSettled(
        batch.map(item => item.operation())
      );

      // Small delay between batches to avoid overwhelming the server
      await new Promise(resolve => setTimeout(resolve, 100));
    }

    this.processing = false;
  }
}

// Usage
const batchProcessor = new BatchProcessor();

// Multiple customer updates
customerUpdates.forEach(update => {
  batchProcessor.addToBatch(`customer-${update.id}`, () =>
    apiClient.put(`/api/app/customer/update-with-response/${update.id}`, update)
  );
});
```

### **Network Status Aware Operations**
```typescript
class NetworkAwareApi {
  private isOnline = navigator.onLine;

  constructor() {
    window.addEventListener('online', () => this.isOnline = true);
    window.addEventListener('offline', () => this.isOnline = false);
  }

  async makeRequest<T>(request: () => Promise<T>): Promise<T> {
    if (!this.isOnline) {
      // Offline durumunda local cache'ten dön veya offline queue'ya ekle
      throw new Error('No network connection');
    }

    return await request();
  }
}
```

---

**💡 Performance Tips:**
- Liste endpoint'leri için `maxResultCount=50` optimal
- Heavy data operations için pagination kullan
- Frequent updates için polling interval'ı 30s+ tut
- Mobile'da simultaneous connection sayısını 4 ile sınırla
- Cache invalidation stratejini dikkatli planla