# WebCarpetApp Mobil Geliştirme En İyi Uygulamaları

## Performance Optimizations

### 🚀 API Call Optimizations

#### **Batch API Requests**
```typescript
class BatchApiClient {
  private batchQueue: Array<{
    id: string;
    request: Promise<any>;
    resolve: (value: any) => void;
    reject: (error: any) => void;
  }> = [];

  private batchTimeout?: NodeJS.Timeout;
  private readonly BATCH_SIZE = 5;
  private readonly BATCH_DELAY = 100; // ms

  async batchRequest<T>(requestFn: () => Promise<T>): Promise<T> {
    return new Promise((resolve, reject) => {
      const batchItem = {
        id: `req_${Date.now()}_${Math.random()}`,
        request: requestFn(),
        resolve,
        reject
      };

      this.batchQueue.push(batchItem);

      // Clear existing timeout
      if (this.batchTimeout) {
        clearTimeout(this.batchTimeout);
      }

      // Process batch after delay or when batch size reached
      if (this.batchQueue.length >= this.BATCH_SIZE) {
        this.processBatch();
      } else {
        this.batchTimeout = setTimeout(() => this.processBatch(), this.BATCH_DELAY);
      }
    });
  }

  private async processBatch() {
    const currentBatch = this.batchQueue.splice(0, this.BATCH_SIZE);

    // Execute all requests in parallel
    const results = await Promise.allSettled(
      currentBatch.map(item => item.request)
    );

    // Resolve/reject individual promises
    results.forEach((result, index) => {
      const batchItem = currentBatch[index];

      if (result.status === 'fulfilled') {
        batchItem.resolve(result.value);
      } else {
        batchItem.reject(result.reason);
      }
    });

    // Process remaining items if any
    if (this.batchQueue.length > 0) {
      this.batchTimeout = setTimeout(() => this.processBatch(), this.BATCH_DELAY);
    }
  }
}

// Usage
const batchClient = new BatchApiClient();

// Multiple customer updates will be batched
const updatePromises = customers.map(customer =>
  batchClient.batchRequest(() =>
    apiClient.put(`/api/app/customer/update-with-response/${customer.id}`, customer)
  )
);

const results = await Promise.all(updatePromises);
```

#### **Smart Pagination**
```typescript
class SmartPagination {
  private pageSize = 20;
  private prefetchThreshold = 0.8; // Prefetch when 80% scrolled

  async loadPage<T>(
    endpoint: string,
    pageNumber: number,
    filters?: any
  ): Promise<PagedResultDto<T>> {
    const params = {
      skipCount: (pageNumber - 1) * this.pageSize,
      maxResultCount: this.pageSize,
      ...filters
    };

    const response = await apiClient.get(endpoint, { params });

    // Prefetch next page if user is likely to scroll
    this.prefetchNextPageIfNeeded(endpoint, pageNumber, filters, response.data.totalCount);

    return response.data;
  }

  private prefetchNextPageIfNeeded<T>(
    endpoint: string,
    currentPage: number,
    filters: any,
    totalCount: number
  ) {
    const hasNextPage = (currentPage * this.pageSize) < totalCount;

    if (hasNextPage) {
      // Prefetch in background
      setTimeout(() => {
        this.loadPage(endpoint, currentPage + 1, filters).catch(console.error);
      }, 500);
    }
  }
}
```

### **Memory Management**

```typescript
class MemoryManager {
  private cacheMaxSize = 100; // Maximum cached entities per type
  private cacheExpiry = 10 * 60 * 1000; // 10 minutes

  async manageCache() {
    // Clear expired cache entries
    await this.clearExpiredCache();

    // Limit cache size
    await this.limitCacheSize();

    // Clear unused image cache
    await this.clearUnusedImages();
  }

  private async clearExpiredCache() {
    const entityTypes = ['customers', 'orders', 'products', 'vehicles'];

    for (const entityType of entityTypes) {
      const cached = await storage.getCachedEntities(entityType);
      const now = Date.now();

      const validEntries = cached.filter(entry =>
        (now - new Date(entry.cachedAt).getTime()) < this.cacheExpiry
      );

      if (validEntries.length < cached.length) {
        await storage.cacheEntities(entityType, validEntries);
      }
    }
  }

  private async limitCacheSize() {
    const entityTypes = ['customers', 'orders', 'products', 'vehicles'];

    for (const entityType of entityTypes) {
      const cached = await storage.getCachedEntities(entityType);

      if (cached.length > this.cacheMaxSize) {
        // Keep most recently accessed items
        const trimmed = cached
          .sort((a, b) => new Date(b.lastAccessedAt).getTime() - new Date(a.lastAccessedAt).getTime())
          .slice(0, this.cacheMaxSize);

        await storage.cacheEntities(entityType, trimmed);
      }
    }
  }
}

// Auto-cleanup on app state changes
class AppStateManager {
  constructor(memoryManager: MemoryManager) {
    // React Native example
    AppState.addEventListener('change', (nextAppState) => {
      if (nextAppState === 'background') {
        memoryManager.manageCache();
      }
    });

    // Web example
    document.addEventListener('visibilitychange', () => {
      if (document.hidden) {
        memoryManager.manageCache();
      }
    });
  }
}
```

## Security Best Practices

### 🔐 Token Security

```typescript
class SecureTokenManager {
  private readonly TOKEN_KEY = 'auth_tokens';
  private readonly BIOMETRIC_KEY = 'biometric_auth';

  async storeTokens(tokens: AuthTokens) {
    // Encrypt tokens before storage
    const encrypted = await this.encrypt(JSON.stringify(tokens));

    // Use secure storage (Keychain on iOS, Keystore on Android)
    await SecureStore.setItemAsync(this.TOKEN_KEY, encrypted);

    // Enable biometric protection if available
    if (await this.isBiometricAvailable()) {
      await this.enableBiometricProtection();
    }
  }

  async getTokens(): Promise<AuthTokens | null> {
    // Check biometric authentication if enabled
    if (await this.isBiometricEnabled()) {
      const biometricResult = await this.authenticateWithBiometric();
      if (!biometricResult.success) {
        throw new Error('Biometric authentication failed');
      }
    }

    const encrypted = await SecureStore.getItemAsync(this.TOKEN_KEY);
    if (!encrypted) return null;

    const decrypted = await this.decrypt(encrypted);
    return JSON.parse(decrypted);
  }

  private async encrypt(data: string): Promise<string> {
    // Use platform-specific encryption
    if (Platform.OS === 'ios') {
      return await Crypto.encryptAES(data);
    } else {
      return await AndroidCrypto.encrypt(data);
    }
  }

  private async isBiometricAvailable(): Promise<boolean> {
    const biometricType = await LocalAuthentication.supportedAuthenticationTypesAsync();
    return biometricType.length > 0;
  }
}
```

### **SSL Pinning**
```typescript
class SecureApiClient {
  private pinnedCertificates = [
    'sha256/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=', // Production cert
    'sha256/BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB='  // Backup cert
  ];

  async makeSecureRequest(url: string, options: RequestOptions) {
    // Certificate pinning for production
    if (process.env.NODE_ENV === 'production') {
      const certificateValidation = await this.validateCertificate(url);
      if (!certificateValidation.valid) {
        throw new SecurityError('Certificate validation failed');
      }
    }

    return fetch(url, {
      ...options,
      // Additional security headers
      headers: {
        ...options.headers,
        'X-Requested-With': 'XMLHttpRequest',
        'X-Frame-Options': 'DENY'
      }
    });
  }

  private async validateCertificate(url: string): Promise<{valid: boolean}> {
    // Platform-specific SSL pinning implementation
    return NetworkingModule.validateSSLPinning(url, this.pinnedCertificates);
  }
}
```

### **Data Protection**
```typescript
class DataProtection {
  // Prevent screenshots in sensitive screens
  enableScreenProtection() {
    if (Platform.OS === 'ios') {
      // iOS - Hide content when app goes to background
      AppState.addEventListener('change', (nextAppState) => {
        if (nextAppState === 'background') {
          this.hidePrivacySensitiveData();
        } else if (nextAppState === 'active') {
          this.showPrivacySensitiveData();
        }
      });
    } else {
      // Android - Prevent screenshots
      this.setSecureFlag(true);
    }
  }

  // Sanitize logs
  sanitizeLogs() {
    // Override console methods in production
    if (process.env.NODE_ENV === 'production') {
      const originalLog = console.log;
      console.log = (...args) => {
        const sanitized = args.map(arg => this.sanitizeLogData(arg));
        originalLog(...sanitized);
      };
    }
  }

  private sanitizeLogData(data: any): any {
    if (typeof data === 'string') {
      // Remove potential tokens and sensitive data
      return data
        .replace(/Bearer\s+[A-Za-z0-9\-_]+\.[A-Za-z0-9\-_]+\.[A-Za-z0-9\-_]*/g, 'Bearer ***')
        .replace(/password["\s]*[:=]["\s]*[^"\\s,}]+/gi, 'password":"***"')
        .replace(/token["\s]*[:=]["\s]*[^"\\s,}]+/gi, 'token":"***"');
    }

    return data;
  }
}
```

## UI/UX Best Practices

### 📱 Responsive Design

```typescript
class ResponsiveDesign {
  // Screen size breakpoints
  static getScreenCategory(): 'small' | 'medium' | 'large' {
    const { width } = Dimensions.get('window');

    if (width < 600) return 'small';    // Phone
    if (width < 900) return 'medium';   // Tablet portrait
    return 'large';                     // Tablet landscape
  }

  static getAdaptivePageSize(): number {
    const category = this.getScreenCategory();

    switch (category) {
      case 'small': return 10;   // Fewer items on phone
      case 'medium': return 20;  // More items on tablet
      case 'large': return 30;   // Most items on large screens
    }
  }

  static getAdaptiveColumnCount(): number {
    const category = this.getScreenCategory();

    switch (category) {
      case 'small': return 1;    // Single column on phone
      case 'medium': return 2;   // Two columns on tablet
      case 'large': return 3;    // Three columns on large screens
    }
  }
}

// Usage in component
const CustomerGrid: React.FC = () => {
  const [customers, setCustomers] = useState<CustomerDto[]>([]);
  const columnCount = ResponsiveDesign.getAdaptiveColumnCount();
  const pageSize = ResponsiveDesign.getAdaptivePageSize();

  return (
    <FlatList
      data={customers}
      numColumns={columnCount}
      key={columnCount} // Re-render when column count changes
      // ... other props
    />
  );
};
```

### **Loading States**
```typescript
class LoadingStateManager {
  static showSkeletonLoading(itemCount: number = 5): JSX.Element {
    return (
      <View>
        {Array.from({ length: itemCount }).map((_, index) => (
          <SkeletonPlaceholder key={index}>
            <SkeletonPlaceholder.Item flexDirection="row" alignItems="center">
              <SkeletonPlaceholder.Item width={60} height={60} borderRadius={50} />
              <SkeletonPlaceholder.Item marginLeft={20}>
                <SkeletonPlaceholder.Item width={120} height={20} />
                <SkeletonPlaceholder.Item marginTop={6} width={80} height={16} />
              </SkeletonPlaceholder.Item>
            </SkeletonPlaceholder.Item>
          </SkeletonPlaceholder>
        ))}
      </View>
    );
  }

  static showProgressiveLoading<T>(
    data: T[],
    isLoading: boolean,
    renderItem: (item: T) => JSX.Element
  ): JSX.Element {
    return (
      <View>
        {data.map(renderItem)}
        {isLoading && this.showSkeletonLoading(3)}
      </View>
    );
  }
}
```

### **Optimistic UI Updates**
```typescript
class OptimisticUIManager {
  static async updateWithOptimistic<T>(
    items: T[],
    itemId: string,
    updates: Partial<T>,
    apiCall: () => Promise<T>,
    onUpdate: (newItems: T[]) => void,
    onError: (error: string) => void
  ) {
    // 1. Apply optimistic update immediately
    const optimisticItems = items.map(item =>
      (item as any).id === itemId ? { ...item, ...updates } : item
    );
    onUpdate(optimisticItems);

    try {
      // 2. Make API call
      const serverResult = await apiCall();

      // 3. Apply server response
      const finalItems = items.map(item =>
        (item as any).id === itemId ? serverResult : item
      );
      onUpdate(finalItems);

    } catch (error) {
      // 4. Revert optimistic update on error
      onUpdate(items);
      onError('Güncelleme başarısız oldu');
    }
  }
}

// Usage
const updateCustomer = async (customerId: string, updates: Partial<CustomerDto>) => {
  await OptimisticUIManager.updateWithOptimistic(
    customers,
    customerId,
    updates,
    () => apiClient.updateCustomer(customerId, updates),
    setCustomers,
    showError
  );
};
```

## Testing Strategies

### 🧪 Unit Testing

```typescript
// API Client Tests
describe('OfflineAwareApiClient', () => {
  let client: OfflineAwareApiClient;
  let mockStorage: jest.Mocked<OfflineStorageManager>;

  beforeEach(() => {
    mockStorage = createMockStorageManager();
    client = new OfflineAwareApiClient(mockStorage);
  });

  describe('createCustomer', () => {
    it('should create customer online when network available', async () => {
      // Mock online state
      Object.defineProperty(navigator, 'onLine', { value: true, writable: true });

      const customerData = { name: 'Test Customer', phone: '123456789' };
      const mockResponse = { success: true, data: { id: '123', ...customerData } };

      mockApiClient.post.mockResolvedValue(mockResponse);

      const result = await client.createCustomer(customerData);

      expect(result.id).toBe('123');
      expect(mockStorage.addOfflineOperation).not.toHaveBeenCalled();
    });

    it('should queue operation when offline', async () => {
      // Mock offline state
      Object.defineProperty(navigator, 'onLine', { value: false, writable: true });

      const customerData = { name: 'Test Customer', phone: '123456789' };

      const result = await client.createCustomer(customerData);

      expect(result.id).toContain('temp_');
      expect(mockStorage.addOfflineOperation).toHaveBeenCalledWith({
        type: 'CREATE',
        entityType: 'Customer',
        entityId: expect.stringContaining('temp_'),
        data: customerData
      });
    });
  });
});
```

### **Integration Testing**
```typescript
describe('End-to-End Customer Management', () => {
  it('should handle complete customer workflow', async () => {
    // 1. Register new tenant
    const registrationData = {
      companyName: 'Test Company',
      fullName: 'Test User',
      emailAddress: 'test@test.com',
      password: 'Test123!',
      phoneNumber: '123456789'
    };

    const registrationResponse = await testApiClient.post(
      '/api/subscription-account/register-with-trial',
      registrationData
    );

    expect(registrationResponse.success).toBe(true);
    expect(registrationResponse.data.tenantId).toBeDefined();

    // 2. Login and get token
    const tokenResponse = await getAuthToken(
      registrationData.emailAddress,
      registrationData.password,
      registrationResponse.data.tenantId
    );

    // 3. Create customer
    const customerData = {
      name: 'Test Customer',
      phone: '05551234567',
      email: 'customer@test.com',
      active: true
    };

    const createResponse = await authenticatedApiClient.post(
      '/api/app/customer/create-with-response',
      customerData
    );

    expect(createResponse.success).toBe(true);
    const customerId = createResponse.data.id;

    // 4. Update customer
    const updateData = { name: 'Updated Customer' };
    const updateResponse = await authenticatedApiClient.put(
      `/api/app/customer/update-with-response/${customerId}`,
      updateData
    );

    expect(updateResponse.success).toBe(true);
    expect(updateResponse.data.name).toBe('Updated Customer');

    // 5. Get customer list
    const listResponse = await authenticatedApiClient.get(
      '/api/app/customer/list-with-response'
    );

    expect(listResponse.success).toBe(true);
    expect(listResponse.data.items).toHaveLength(1);
    expect(listResponse.data.items[0].name).toBe('Updated Customer');
  });
});
```

### **Performance Testing**
```typescript
describe('Performance Tests', () => {
  it('should handle large customer lists efficiently', async () => {
    const startTime = Date.now();

    // Create 1000 customers
    const createPromises = Array.from({ length: 1000 }, (_, index) =>
      apiClient.createCustomer({
        name: `Customer ${index}`,
        phone: `055512${index.toString().padStart(5, '0')}`,
        active: true
      })
    );

    await Promise.all(createPromises);

    const creationTime = Date.now() - startTime;
    expect(creationTime).toBeLessThan(30000); // Should complete within 30 seconds

    // Test pagination performance
    const listStartTime = Date.now();
    const listResponse = await apiClient.get('/api/app/customer/list-with-response?maxResultCount=1000');

    const listTime = Date.now() - listStartTime;
    expect(listTime).toBeLessThan(5000); // List should load within 5 seconds
    expect(listResponse.data.items).toHaveLength(1000);
  });

  it('should handle concurrent operations efficiently', async () => {
    const concurrentOperations = 50;
    const startTime = Date.now();

    // Simulate concurrent user actions
    const operations = Array.from({ length: concurrentOperations }, async (_, index) => {
      const customer = await apiClient.createCustomer({
        name: `Concurrent Customer ${index}`,
        phone: `055513${index.toString().padStart(5, '0')}`,
        active: true
      });

      // Update immediately after creation
      return apiClient.updateCustomer(customer.id, {
        name: `Updated Concurrent Customer ${index}`
      });
    });

    await Promise.all(operations);

    const totalTime = Date.now() - startTime;
    expect(totalTime).toBeLessThan(15000); // Should complete within 15 seconds
  });
});
```

## Monitoring and Analytics

### 📊 Error Tracking
```typescript
class ErrorTracker {
  private analytics: AnalyticsService;

  trackApiError(error: ApiError, context: string) {
    this.analytics.track('api_error', {
      errorCode: error.code,
      errorMessage: error.message,
      context: context,
      timestamp: new Date().toISOString(),
      userId: getCurrentUserId(),
      tenantId: getCurrentTenantId()
    });
  }

  trackPerformance(operation: string, duration: number, metadata?: any) {
    this.analytics.track('performance_metric', {
      operation,
      duration,
      ...metadata,
      timestamp: new Date().toISOString()
    });
  }

  trackUserAction(action: string, data?: any) {
    this.analytics.track('user_action', {
      action,
      data,
      timestamp: new Date().toISOString()
    });
  }
}

// Usage with API calls
const timedApiCall = async <T>(
  operation: string,
  apiCall: () => Promise<T>
): Promise<T> => {
  const startTime = Date.now();

  try {
    const result = await apiCall();
    const duration = Date.now() - startTime;

    errorTracker.trackPerformance(operation, duration, { success: true });
    return result;

  } catch (error) {
    const duration = Date.now() - startTime;

    errorTracker.trackPerformance(operation, duration, { success: false });
    errorTracker.trackApiError(error as ApiError, operation);

    throw error;
  }
};
```

---

**🎯 Key Takeaways:**

1. **Batch API calls** - Reduce network overhead
2. **Implement smart caching** - Balance performance and memory
3. **Use secure storage** - Protect sensitive data
4. **Optimize for mobile networks** - Handle poor connectivity
5. **Implement proper error tracking** - Monitor production issues
6. **Test thoroughly** - Unit, integration, and performance tests
7. **Monitor real usage** - Track performance and user behavior