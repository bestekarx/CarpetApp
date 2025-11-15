# WebCarpetApp Mobil Hata Yönetimi ve Lokalizasyon Rehberi

## Hata Yönetimi Stratejisi

### 🎯 Standardize Hata Response Formatı

**Tüm API hataları aynı yapıyı kullanır:**
```typescript
interface ApiErrorResponse {
  success: false;
  data: null;
  message: string;          // Kullanıcı dostu lokalize mesaj
  error: {
    code: string;           // Makine okunabilir hata kodu
    details?: any;          // Ek hata detayları
  };
  timestamp: string;
}
```

### 📱 Mobil Hata Kategorileri

#### **1. Network/Connection Errors**
```typescript
// Network bağlantı hataları
const NETWORK_ERRORS = {
  NO_CONNECTION: 'İnternet bağlantısı yok',
  TIMEOUT: 'İstek zaman aşımına uğradı',
  SERVER_UNREACHABLE: 'Sunucuya erişilemiyor'
};

class NetworkErrorHandler {
  static handle(error: any): string {
    if (!navigator.onLine) {
      return NETWORK_ERRORS.NO_CONNECTION;
    }

    if (error.code === 'NETWORK_ERROR') {
      return NETWORK_ERRORS.SERVER_UNREACHABLE;
    }

    if (error.name === 'AbortError') {
      return NETWORK_ERRORS.TIMEOUT;
    }

    return 'Bağlantı hatası oluştu';
  }
}
```

#### **2. HTTP Status Code Errors**
```typescript
class HttpErrorHandler {
  static getErrorMessage(status: number, responseBody: any): string {
    switch (status) {
      case 400:
        return responseBody.message || 'Geçersiz istek';
      case 401:
        return 'Oturum süreniz dolmuş. Lütfen tekrar giriş yapın';
      case 403:
        return 'Bu işlem için yetkiniz bulunmuyor';
      case 404:
        return 'İstenen kaynak bulunamadı';
      case 409:
        return 'Çakışma hatası. Veri başka bir kullanıcı tarafından değiştirilmiş';
      case 422:
        return 'Doğrulama hatası';
      case 429:
        return 'Çok fazla istek gönderildi. Lütfen bekleyin';
      case 500:
        return 'Sunucu hatası oluştu';
      case 502:
        return 'Geçici sunucu hatası';
      case 503:
        return 'Servis şu anda kullanılamıyor';
      default:
        return 'Beklenmeyen bir hata oluştu';
    }
  }
}
```

## Business Logic Error Codes

### 🏢 Customer (Müşteri) Hataları
```typescript
const CUSTOMER_ERRORS = {
  CUSTOMER_DUPLICATE_PHONE: 'Bu telefon numarası zaten kullanımda',
  CUSTOMER_NOT_FOUND: 'Müşteri bulunamadı',
  CUSTOMER_INVALID_PHONE: 'Geçersiz telefon numarası formatı',
  CUSTOMER_INVALID_EMAIL: 'Geçersiz e-posta adresi formatı',
  CUSTOMER_AREA_REQUIRED: 'Bölge seçimi zorunludur',
  CUSTOMER_GPS_INVALID: 'Geçersiz GPS koordinatları'
};

// Usage Example
if (error.code === 'CUSTOMER_DUPLICATE_PHONE') {
  showValidationError('phone', CUSTOMER_ERRORS.CUSTOMER_DUPLICATE_PHONE);
}
```

### 📦 Order (Sipariş) Hataları
```typescript
const ORDER_ERRORS = {
  ORDER_CUSTOMER_REQUIRED: 'Müşteri seçimi zorunlu',
  ORDER_AREA_REQUIRED: 'Bölge seçimi zorunlu',
  ORDER_INVALID_DELIVERY_DATE: 'Teslimat tarihi geçmişte olamaz',
  ORDER_PRODUCTS_REQUIRED: 'En az bir ürün seçimi zorunlu',
  ORDER_INVALID_QUANTITY: 'Geçersiz ürün miktarı',
  ORDER_STATUS_TRANSITION_INVALID: 'Geçersiz durum değişikliği',
  ORDER_ALREADY_COMPLETED: 'Bu sipariş zaten tamamlanmış',
  ORDER_RECEIVED_REQUIRED: 'Alınan mal kaydı zorunlu'
};
```

### 🚛 Vehicle (Araç) Hataları
```typescript
const VEHICLE_ERRORS = {
  VEHICLE_DUPLICATE_PLATE: 'Bu plaka numarası zaten kayıtlı',
  VEHICLE_INVALID_PLATE: 'Geçersiz plaka formatı',
  VEHICLE_NOT_FOUND: 'Araç bulunamadı',
  VEHICLE_ALREADY_ASSIGNED: 'Bu araç zaten başka bir işe atanmış'
};
```

### 📄 Product (Ürün) Hataları
```typescript
const PRODUCT_ERRORS = {
  PRODUCT_DUPLICATE_NAME: 'Bu ürün adı zaten kullanımda',
  PRODUCT_NOT_FOUND: 'Ürün bulunamadı',
  PRODUCT_INVALID_PRICE: 'Ürün fiyatı 0\'dan büyük olmalıdır',
  PRODUCT_CATEGORY_REQUIRED: 'Ürün kategorisi zorunlu'
};
```

### 🔐 Authentication (Kimlik) Hataları
```typescript
const AUTH_ERRORS = {
  TENANT_NOT_FOUND: 'Bu e-posta adresi ile kayıtlı şirket bulunamadı',
  INVALID_CREDENTIALS: 'E-posta adresi veya şifre hatalı',
  ACCOUNT_LOCKED: 'Hesap geçici olarak kilitlenmiş',
  PASSWORD_EXPIRED: 'Şifre süresi dolmuş',
  TOKEN_EXPIRED: 'Oturum süresi dolmuş',
  INSUFFICIENT_PERMISSIONS: 'Bu işlem için yetkiniz bulunmuyor'
};
```

## Lokalizasyon (Multi-Language Support)

### 🌍 Dil Desteği
API şu dilleri destekler:
- **Türkçe (tr)** - Varsayılan
- **İngilizce (en)**
- **Arapça (ar)**

### **Header Yapılandırması**
```typescript
const API_HEADERS = {
  'Accept-Language': 'tr',     // tr, en, ar
  'Content-Type': 'application/json',
  'Authorization': 'Bearer {token}'
};

// Dil değiştirme
class LanguageManager {
  private currentLanguage = 'tr';

  setLanguage(lang: 'tr' | 'en' | 'ar') {
    this.currentLanguage = lang;
    // Update all future API calls
    ApiClient.setDefaultHeader('Accept-Language', lang);

    // Update local error messages
    this.updateErrorMessages(lang);
  }

  getCurrentLanguage() {
    return this.currentLanguage;
  }
}
```

### **Çoklu Dil Error Messages**
```typescript
const ERROR_MESSAGES = {
  tr: {
    CUSTOMER_DUPLICATE_PHONE: 'Bu telefon numarası zaten kullanımda',
    VALIDATION_ERROR: 'Doğrulama hatası',
    NETWORK_ERROR: 'Bağlantı hatası oluştu'
  },
  en: {
    CUSTOMER_DUPLICATE_PHONE: 'This phone number is already in use',
    VALIDATION_ERROR: 'Validation error',
    NETWORK_ERROR: 'Connection error occurred'
  },
  ar: {
    CUSTOMER_DUPLICATE_PHONE: 'رقم الهاتف هذا مستخدم بالفعل',
    VALIDATION_ERROR: 'خطأ في التحقق',
    NETWORK_ERROR: 'حدث خطأ في الاتصال'
  }
};

class LocalizedErrorHandler {
  static getMessage(errorCode: string, language: string = 'tr'): string {
    return ERROR_MESSAGES[language]?.[errorCode] ||
           ERROR_MESSAGES.tr[errorCode] ||
           'Beklenmeyen hata oluştu';
  }
}
```

## Validation Errors

### **Form Validation Errors**
```typescript
interface ValidationError {
  message: string;
  members: string[];  // Field names that failed validation
}

interface ValidationResponse {
  success: false;
  message: "Your request is not valid!";
  error: {
    code: null;
    validationErrors: ValidationError[];
  };
}

// Example validation error handling
class FormValidator {
  static handleValidationErrors(response: ValidationResponse) {
    const errors: Record<string, string> = {};

    response.error.validationErrors.forEach(validationError => {
      validationError.members.forEach(fieldName => {
        errors[fieldName] = validationError.message;
      });
    });

    return errors;
  }
}

// Usage in form
const handleSubmit = async (formData: any) => {
  try {
    await apiClient.post('/api/app/customer/create-with-response', formData);
  } catch (error) {
    if (error.response.status === 400 && error.response.data.error?.validationErrors) {
      const fieldErrors = FormValidator.handleValidationErrors(error.response.data);

      // Show errors on form fields
      Object.keys(fieldErrors).forEach(fieldName => {
        setFieldError(fieldName, fieldErrors[fieldName]);
      });
    }
  }
};
```

## Comprehensive Error Handler

### **Universal Error Handler**
```typescript
class UniversalErrorHandler {
  private languageManager: LanguageManager;
  private logger: Logger;

  constructor(languageManager: LanguageManager, logger: Logger) {
    this.languageManager = languageManager;
    this.logger = logger;
  }

  async handleError(error: any): Promise<ErrorInfo> {
    // Log error for debugging
    this.logger.error('API Error:', error);

    // Network errors
    if (error.name === 'NetworkError' || !navigator.onLine) {
      return {
        type: 'NETWORK',
        message: this.getLocalizedMessage('NETWORK_ERROR'),
        canRetry: true,
        retryAfter: 5000
      };
    }

    // HTTP errors with API response
    if (error.response) {
      const { status, data } = error.response;

      // Handle validation errors specially
      if (status === 400 && data.error?.validationErrors) {
        return {
          type: 'VALIDATION',
          message: data.message,
          validationErrors: data.error.validationErrors,
          canRetry: false
        };
      }

      // Handle business logic errors
      if (data.error?.code) {
        return {
          type: 'BUSINESS',
          message: data.message || this.getLocalizedMessage(data.error.code),
          errorCode: data.error.code,
          canRetry: false
        };
      }

      // Handle HTTP status errors
      return {
        type: 'HTTP',
        message: HttpErrorHandler.getErrorMessage(status, data),
        status: status,
        canRetry: status >= 500 // Server errors can be retried
      };
    }

    // Unknown errors
    return {
      type: 'UNKNOWN',
      message: this.getLocalizedMessage('UNEXPECTED_ERROR'),
      canRetry: false
    };
  }

  private getLocalizedMessage(key: string): string {
    const lang = this.languageManager.getCurrentLanguage();
    return LocalizedErrorHandler.getMessage(key, lang);
  }
}

interface ErrorInfo {
  type: 'NETWORK' | 'HTTP' | 'BUSINESS' | 'VALIDATION' | 'UNKNOWN';
  message: string;
  canRetry: boolean;
  retryAfter?: number;
  errorCode?: string;
  status?: number;
  validationErrors?: ValidationError[];
}
```

## User Experience Patterns

### **Error Display Components**
```typescript
// Toast/Snackbar for general errors
const showError = (message: string, duration: number = 3000) => {
  Toast.show({
    type: 'error',
    text1: 'Hata',
    text2: message,
    visibilityTime: duration
  });
};

// Modal for critical errors
const showCriticalError = (title: string, message: string, actions?: Action[]) => {
  Alert.alert(
    title,
    message,
    actions || [
      { text: 'Tamam', style: 'default' },
      { text: 'Tekrar Dene', style: 'cancel', onPress: () => retry() }
    ]
  );
};

// Inline form field errors
const showFieldError = (fieldName: string, errorMessage: string) => {
  setFieldErrors(prev => ({
    ...prev,
    [fieldName]: errorMessage
  }));
};
```

### **Retry Mechanism**
```typescript
class RetryHandler {
  static async withRetry<T>(
    operation: () => Promise<T>,
    maxRetries: number = 3,
    backoffMs: number = 1000
  ): Promise<T> {
    let lastError: any;

    for (let attempt = 0; attempt <= maxRetries; attempt++) {
      try {
        return await operation();
      } catch (error) {
        lastError = error;

        // Don't retry business logic or validation errors
        if (error.response?.status < 500) {
          throw error;
        }

        // Don't retry on last attempt
        if (attempt === maxRetries) {
          break;
        }

        // Exponential backoff
        const delay = backoffMs * Math.pow(2, attempt);
        await new Promise(resolve => setTimeout(resolve, delay));
      }
    }

    throw lastError;
  }
}

// Usage
const createCustomer = async (customerData: any) => {
  try {
    return await RetryHandler.withRetry(() =>
      apiClient.post('/api/app/customer/create-with-response', customerData)
    );
  } catch (error) {
    const errorInfo = await errorHandler.handleError(error);

    if (errorInfo.type === 'VALIDATION') {
      // Show field errors
      errorInfo.validationErrors?.forEach(ve => {
        ve.members.forEach(field => {
          showFieldError(field, ve.message);
        });
      });
    } else {
      // Show general error
      showError(errorInfo.message);
    }
  }
};
```

### **Offline Error Handling**
```typescript
class OfflineErrorHandler {
  private offlineQueue: Array<{id: string, operation: () => Promise<any>}> = [];

  handleOfflineError(operation: () => Promise<any>, operationId: string) {
    if (!navigator.onLine) {
      // Add to offline queue
      this.offlineQueue.push({id: operationId, operation});

      showError('İnternet bağlantısı yok. İşlem çevrimiçi olduğunuzda tekrarlanacak.');
      return;
    }

    // Execute immediately if online
    operation().catch(error => {
      // Handle other errors normally
      errorHandler.handleError(error);
    });
  }

  async processOfflineQueue() {
    if (!navigator.onLine || this.offlineQueue.length === 0) return;

    const operations = [...this.offlineQueue];
    this.offlineQueue = [];

    for (const {id, operation} of operations) {
      try {
        await operation();
        showSuccess(`${id} işlemi başarıyla tamamlandı`);
      } catch (error) {
        // Re-add to queue if it's a network error
        if (error.name === 'NetworkError') {
          this.offlineQueue.push({id, operation});
        } else {
          // Show error for business logic issues
          await errorHandler.handleError(error);
        }
      }
    }
  }
}

// Listen for online/offline events
window.addEventListener('online', () => {
  offlineErrorHandler.processOfflineQueue();
});
```

---

**💡 En İyi Uygulamalar:**

1. **Her zaman success field'ı kontrol et** - API response'ta success: false ise hata vardır
2. **Error code'ları kullan** - Lokalizasyon ve programmatic handling için
3. **Validation hatalarını form field'larında göster** - UX için kritik
4. **Network hatalarında retry mekanizması** - Mobile connectivity için önemli
5. **Offline senaryolarını handle et** - Mobile app için zorunlu
6. **Critical hataları modal ile göster** - Kullanıcı dikkatini çekmek için
7. **Debug için comprehensive logging** - Production troubleshooting için