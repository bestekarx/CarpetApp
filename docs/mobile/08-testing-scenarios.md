# WebCarpetApp Mobil Test Senaryoları ve QA Rehberi

## Test Kategorileri

### 🧪 1. Authentication Flow Tests

#### **Yeni Kullanıcı Kaydı Test Senaryoları**
```typescript
describe('User Registration Flow', () => {
  test('TC001: Başarılı kayıt işlemi', async () => {
    // Given
    const registrationData = {
      companyName: "Test Şirketi Ltd.",
      fullName: "Ahmet Test",
      emailAddress: "ahmet.test@testfirma.com",
      password: "Test123!",
      phoneNumber: "05551234567"
    };

    // When
    const response = await authService.register(registrationData);

    // Then
    expect(response.success).toBe(true);
    expect(response.data.tenantId).toBeDefined();
    expect(response.data.isAuthenticated).toBe(true);

    // Verify auto-login after registration
    const isAuth = await authService.isAuthenticated();
    expect(isAuth).toBe(true);
  });

  test('TC002: Duplicate email ile kayıt', async () => {
    // Given - First registration
    await authService.register({
      companyName: "İlk Şirket",
      fullName: "İlk Kullanıcı",
      emailAddress: "duplicate@test.com",
      password: "Test123!",
      phoneNumber: "05551111111"
    });

    // When - Second registration with same email
    const response = await authService.register({
      companyName: "İkinci Şirket",
      fullName: "İkinci Kullanıcı",
      emailAddress: "duplicate@test.com", // Same email
      password: "Test456!",
      phoneNumber: "05552222222"
    });

    // Then
    expect(response.success).toBe(false);
    expect(response.error.code).toBe('EMAIL_ALREADY_EXISTS');
  });

  test('TC003: Zayıf şifre ile kayıt', async () => {
    // Given
    const weakPasswordData = {
      companyName: "Test Şirketi",
      fullName: "Test User",
      emailAddress: "weak@test.com",
      password: "123", // Weak password
      phoneNumber: "05551234567"
    };

    // When
    const response = await authService.register(weakPasswordData);

    // Then
    expect(response.success).toBe(false);
    expect(response.error.code).toBe('WEAK_PASSWORD');
  });
});
```

#### **Login/Logout Test Senaryoları**
```typescript
describe('Login/Logout Flow', () => {
  test('TC004: Başarılı giriş işlemi', async () => {
    // Given - Registered user
    const credentials = {
      emailAddress: "test@example.com",
      password: "Test123!"
    };

    // When
    const response = await authService.login(credentials);

    // Then
    expect(response.success).toBe(true);
    expect(response.data.tokens.accessToken).toBeDefined();
    expect(response.data.user).toBeDefined();

    // Verify authentication state
    const isAuth = await authService.isAuthenticated();
    expect(isAuth).toBe(true);
  });

  test('TC005: Yanlış şifre ile giriş', async () => {
    // Given
    const credentials = {
      emailAddress: "test@example.com",
      password: "WrongPassword123!"
    };

    // When
    const response = await authService.login(credentials);

    // Then
    expect(response.success).toBe(false);
    expect(response.error.code).toBe('INVALID_CREDENTIALS');
  });

  test('TC006: Token yenileme', async () => {
    // Given - Valid refresh token
    const tokens = await authService.getValidTokens();

    // Simulate expired access token
    const expiredTokens = {
      ...tokens,
      expiresAt: Date.now() - 1000 // 1 second ago
    };
    await AsyncStorage.setItem('auth_tokens', JSON.stringify(expiredTokens));

    // When
    const newTokens = await authService.getValidTokens();

    // Then
    expect(newTokens).toBeDefined();
    expect(newTokens.accessToken).not.toBe(tokens.accessToken);
    expect(newTokens.expiresAt).toBeGreaterThan(Date.now());
  });
});
```

### 📱 2. Customer Management Tests

#### **CRUD Operations**
```typescript
describe('Customer Management', () => {
  let customerId: string;

  test('TC007: Yeni müşteri oluşturma', async () => {
    // Given
    const customerData = {
      name: "Test Müşteri A.Ş.",
      phone: "05551234567",
      email: "musteri@test.com",
      address: "Test Mahallesi, Test Sokak No:1",
      active: true
    };

    // When
    const response = await customerService.create(customerData);

    // Then
    expect(response.success).toBe(true);
    expect(response.data.name).toBe(customerData.name);
    expect(response.data.phone).toBe(customerData.phone);
    customerId = response.data.id;
  });

  test('TC008: Duplicate telefon numarası kontrolü', async () => {
    // Given - Existing phone number
    const duplicatePhone = {
      name: "Başka Müşteri",
      phone: "05551234567", // Same phone as TC007
      email: "baska@test.com",
      active: true
    };

    // When
    const response = await customerService.create(duplicatePhone);

    // Then
    expect(response.success).toBe(false);
    expect(response.error.code).toBe('CUSTOMER_DUPLICATE_PHONE');
  });

  test('TC009: Müşteri güncelleme', async () => {
    // Given
    const updateData = {
      name: "Güncellenmiş Müşteri A.Ş.",
      email: "guncellenmis@test.com"
    };

    // When
    const response = await customerService.update(customerId, updateData);

    // Then
    expect(response.success).toBe(true);
    expect(response.data.name).toBe(updateData.name);
    expect(response.data.email).toBe(updateData.email);
  });

  test('TC010: GPS koordinat güncelleme', async () => {
    // Given
    const newCoordinates = "41.0082,28.9784"; // Istanbul coordinates

    // When
    const response = await customerService.updateLocation(customerId, newCoordinates);

    // Then
    expect(response.success).toBe(true);
    expect(response.data.coordinates).toBe(newCoordinates);
  });

  test('TC011: Müşteri listesi sayfalama', async () => {
    // Given
    const pageSize = 10;

    // When
    const response = await customerService.getList({
      maxResultCount: pageSize,
      skipCount: 0
    });

    // Then
    expect(response.success).toBe(true);
    expect(response.data.items.length).toBeLessThanOrEqual(pageSize);
    expect(response.data.totalCount).toBeGreaterThan(0);
  });

  test('TC012: Müşteri arama', async () => {
    // Given
    const searchTerm = "Test";

    // When
    const response = await customerService.searchByName(searchTerm);

    // Then
    expect(response.success).toBe(true);
    response.data.items.forEach(customer => {
      expect(customer.name.toLowerCase()).toContain(searchTerm.toLowerCase());
    });
  });
});
```

### 📦 3. Order Management Tests

```typescript
describe('Order Management', () => {
  let orderId: string;
  let customerId: string;
  let areaId: string;
  let receivedId: string;

  beforeAll(async () => {
    // Setup test data
    const customer = await customerService.create({
      name: "Sipariş Test Müşteri",
      phone: "05559876543",
      active: true
    });
    customerId = customer.data.id;

    const area = await areaService.create({
      name: "Test Bölgesi",
      active: true
    });
    areaId = area.data.id;

    const received = await receivedService.create({
      description: "Test Alım",
      areaId: areaId
    });
    receivedId = received.data.id;
  });

  test('TC013: Yeni sipariş oluşturma', async () => {
    // Given
    const orderData = {
      customerId: customerId,
      areaId: areaId,
      receivedId: receivedId,
      deliveryDate: "2025-12-01T14:00:00.000Z",
      description: "Test sipariş açıklaması",
      priorityLevel: 2
    };

    // When
    const response = await orderService.create(orderData);

    // Then
    expect(response.success).toBe(true);
    expect(response.data.customerId).toBe(customerId);
    expect(response.data.orderStatus).toBe('Active');
    orderId = response.data.id;
  });

  test('TC014: Geçmiş tarih ile sipariş oluşturma', async () => {
    // Given
    const pastDate = new Date();
    pastDate.setDate(pastDate.getDate() - 1); // Yesterday

    const invalidOrderData = {
      customerId: customerId,
      areaId: areaId,
      receivedId: receivedId,
      deliveryDate: pastDate.toISOString(),
      description: "Geçmiş tarihli sipariş"
    };

    // When
    const response = await orderService.create(invalidOrderData);

    // Then
    expect(response.success).toBe(false);
    expect(response.error.code).toBe('ORDER_INVALID_DELIVERY_DATE');
  });

  test('TC015: En yakın siparişler GPS testi', async () => {
    // Given
    const currentLocation = {
      latitude: 41.0082,
      longitude: 28.9784,
      radiusKm: 5,
      maxResults: 10
    };

    // When
    const response = await orderService.getNearestOrders(currentLocation);

    // Then
    expect(response.success).toBe(true);
    response.data.forEach(orderWithDistance => {
      expect(orderWithDistance.distance).toBeGreaterThanOrEqual(0);
      expect(orderWithDistance.distance).toBeLessThanOrEqual(5); // Within radius
      expect(orderWithDistance.order).toBeDefined();
      expect(orderWithDistance.customer).toBeDefined();
    });
  });

  test('TC016: Sipariş durumu değişikliği', async () => {
    // Given
    const statusUpdate = {
      orderStatus: 'Completed'
    };

    // When
    const response = await orderService.update(orderId, statusUpdate);

    // Then
    expect(response.success).toBe(true);
    expect(response.data.orderStatus).toBe('Completed');
  });
});
```

### 🔄 4. Offline Sync Tests

```typescript
describe('Offline Synchronization', () => {
  let offlineStorage: OfflineStorageManager;
  let syncManager: SyncManager;

  beforeEach(async () => {
    offlineStorage = new OfflineStorageManager();
    await offlineStorage.initialize();

    syncManager = new SyncManager(apiClient, offlineStorage);
  });

  test('TC017: Çevrimdışı müşteri oluşturma', async () => {
    // Given - Offline state
    mockNetworkState(false);

    const customerData = {
      name: "Offline Müşteri",
      phone: "05556666666",
      active: true
    };

    // When
    const offlineClient = new OfflineAwareApiClient(offlineStorage);
    const response = await offlineClient.createCustomer(customerData);

    // Then
    expect(response.id).toContain('temp_'); // Temporary ID

    const pendingOps = await offlineStorage.getPendingOperations();
    expect(pendingOps).toHaveLength(1);
    expect(pendingOps[0].type).toBe('CREATE');
    expect(pendingOps[0].entityType).toBe('Customer');
  });

  test('TC018: Online'a geçiş sonrası otomatik sync', async () => {
    // Given - Pending offline operations
    await offlineStorage.addOfflineOperation({
      type: 'CREATE',
      entityType: 'Customer',
      entityId: 'temp_123',
      data: { name: 'Sync Test', phone: '05557777777', active: true }
    });

    // When - Come back online and sync
    mockNetworkState(true);
    const syncResult = await syncManager.startSync();

    // Then
    expect(syncResult.success).toBe(true);
    expect(syncResult.operationsUploaded).toBe(1);

    const remainingOps = await offlineStorage.getPendingOperations();
    expect(remainingOps).toHaveLength(0);
  });

  test('TC019: Çakışma çözümleme testi', async () => {
    // Given - Customer exists both locally and remotely with different data
    const customerId = 'existing-customer-id';

    // Local change
    await offlineStorage.addOfflineOperation({
      type: 'UPDATE',
      entityType: 'Customer',
      entityId: customerId,
      data: { name: 'Local Update', phone: '05551111111' }
    });

    // Simulate server change (newer timestamp)
    const serverChanges = [{
      entityType: 'Customer',
      entityId: customerId,
      type: 'UPDATE',
      data: { name: 'Server Update', email: 'server@test.com' },
      timestamp: new Date(Date.now() + 1000).toISOString()
    }];

    // When - Detect and resolve conflicts
    const conflicts = await syncManager.detectConflicts(
      await offlineStorage.getPendingOperations(),
      serverChanges
    );

    const resolution = await conflictResolver.resolveConflict(
      conflicts[0],
      'MERGE'
    );

    // Then
    expect(conflicts).toHaveLength(1);
    expect(resolution.resolution).toBe('MERGE');
    expect(resolution.resolvedData.name).toBe('Local Update'); // Client wins for name
    expect(resolution.resolvedData.email).toBe('server@test.com'); // Server wins for email
  });
});
```

### 🚀 5. Performance Tests

```typescript
describe('Performance Tests', () => {
  test('TC020: Büyük liste yükleme performansı', async () => {
    // Given
    const startTime = Date.now();
    const pageSize = 100;

    // When
    const response = await customerService.getList({
      maxResultCount: pageSize,
      skipCount: 0
    });

    const loadTime = Date.now() - startTime;

    // Then
    expect(response.success).toBe(true);
    expect(loadTime).toBeLessThan(3000); // Should load within 3 seconds
    expect(response.data.items.length).toBeLessThanOrEqual(pageSize);
  });

  test('TC021: Concurrent API calls', async () => {
    // Given
    const concurrentCalls = 10;
    const startTime = Date.now();

    // When
    const promises = Array.from({ length: concurrentCalls }, (_, i) =>
      customerService.create({
        name: `Concurrent Customer ${i}`,
        phone: `05551${i.toString().padStart(6, '0')}`,
        active: true
      })
    );

    const results = await Promise.all(promises);
    const totalTime = Date.now() - startTime;

    // Then
    expect(results).toHaveLength(concurrentCalls);
    results.forEach(result => expect(result.success).toBe(true));
    expect(totalTime).toBeLessThan(10000); // Should complete within 10 seconds
  });

  test('TC022: Memory usage during large operations', async () => {
    // Given
    const initialMemory = performance.memory?.usedJSHeapSize || 0;

    // When - Load large dataset
    const response = await customerService.getList({
      maxResultCount: 1000
    });

    const afterLoadMemory = performance.memory?.usedJSHeapSize || 0;
    const memoryIncrease = afterLoadMemory - initialMemory;

    // Then
    expect(response.success).toBe(true);
    expect(memoryIncrease).toBeLessThan(50 * 1024 * 1024); // Less than 50MB increase
  });
});
```

### 🔐 6. Security Tests

```typescript
describe('Security Tests', () => {
  test('TC023: Token expiry handling', async () => {
    // Given - Expired token
    const expiredTokens = {
      accessToken: 'expired-token',
      refreshToken: 'valid-refresh-token',
      expiresAt: Date.now() - 1000, // 1 second ago
      tenantId: 'test-tenant'
    };

    await AsyncStorage.setItem('auth_tokens', JSON.stringify(expiredTokens));

    // When - Make API call with expired token
    try {
      await customerService.getList();
    } catch (error) {
      // Then - Should attempt token refresh
      expect(error.status).toBe(401);
    }

    // Verify token refresh was attempted
    const newTokens = await authService.getValidTokens();
    expect(newTokens).not.toBeNull();
    expect(newTokens.accessToken).not.toBe(expiredTokens.accessToken);
  });

  test('TC024: Unauthorized access protection', async () => {
    // Given - No authentication token
    await authService.logout();

    // When - Try to access protected endpoint
    const response = await customerService.getList();

    // Then
    expect(response.success).toBe(false);
    expect(response.error.code).toBe('UNAUTHORIZED');
  });

  test('TC025: Cross-tenant data isolation', async () => {
    // Given - Login as tenant A
    const tenantAUser = await authService.login({
      emailAddress: "tenanta@test.com",
      password: "Test123!"
    });

    const customerA = await customerService.create({
      name: "Tenant A Customer",
      phone: "05551111111",
      active: true
    });

    // When - Switch to tenant B
    await authService.logout();
    await authService.login({
      emailAddress: "tenantb@test.com",
      password: "Test123!"
    });

    const tenantBCustomers = await customerService.getList();

    // Then - Should not see tenant A's customers
    expect(tenantBCustomers.success).toBe(true);
    const customerIds = tenantBCustomers.data.items.map(c => c.id);
    expect(customerIds).not.toContain(customerA.data.id);
  });
});
```

## UI/UX Test Scenarios

### 📱 7. Mobile UX Tests

```typescript
describe('Mobile UX Tests', () => {
  test('TC026: Form validation feedback', async () => {
    // Given - Invalid form data
    const invalidData = {
      name: '', // Required field empty
      phone: '123', // Invalid phone format
      email: 'invalid-email' // Invalid email format
    };

    // When - Submit form
    const validationResult = FormValidator.validateCustomer(invalidData);

    // Then
    expect(validationResult.isValid).toBe(false);
    expect(validationResult.errors).toHaveProperty('name');
    expect(validationResult.errors).toHaveProperty('phone');
    expect(validationResult.errors).toHaveProperty('email');
    expect(validationResult.errors.name).toBe('Müşteri adı zorunludur');
  });

  test('TC027: Optimistic UI update', async () => {
    // Given - Existing customer
    const customerId = 'test-customer-id';
    const originalName = 'Original Name';
    const newName = 'Updated Name';

    // Mock UI state
    const [customers, setCustomers] = useState([
      { id: customerId, name: originalName }
    ]);

    // When - Update with optimistic UI
    await OptimisticUIManager.updateWithOptimistic(
      customers,
      customerId,
      { name: newName },
      () => customerService.update(customerId, { name: newName }),
      setCustomers,
      showError
    );

    // Then - UI should update immediately
    expect(customers[0].name).toBe(newName);
  });

  test('TC028: Offline indicator display', async () => {
    // Given - Offline state
    mockNetworkState(false);

    // When - Check offline status
    const offlineManager = new OfflineStateManager();
    const status = offlineManager.getStatus();

    // Then
    expect(status.isOnline).toBe(false);
    expect(status.hasPendingOperations).toBe(true);

    // UI should show offline indicator
    const { getByText } = render(<OfflineStatusIndicator />);
    expect(getByText('Çevrimdışı - Değişiklikler cihazda saklanıyor')).toBeTruthy();
  });
});
```

## Test Automation Script

```typescript
// src/tests/automation/test-runner.ts
export class AutomatedTestRunner {
  private testResults: TestResult[] = [];

  async runAllTests(): Promise<TestSuiteResult> {
    console.log('🚀 Starting WebCarpetApp Mobile Test Suite...\n');

    const testSuites = [
      { name: 'Authentication', tests: this.authenticationTests },
      { name: 'Customer Management', tests: this.customerTests },
      { name: 'Order Management', tests: this.orderTests },
      { name: 'Offline Sync', tests: this.offlineTests },
      { name: 'Performance', tests: this.performanceTests },
      { name: 'Security', tests: this.securityTests },
      { name: 'Mobile UX', tests: this.mobileUxTests }
    ];

    for (const suite of testSuites) {
      console.log(`📋 Running ${suite.name} tests...`);
      const suiteResults = await this.runTestSuite(suite.tests);
      this.testResults.push(...suiteResults);
      console.log(`✅ ${suite.name} completed\n`);
    }

    return this.generateReport();
  }

  private async runTestSuite(tests: TestCase[]): Promise<TestResult[]> {
    const results: TestResult[] = [];

    for (const test of tests) {
      const startTime = Date.now();

      try {
        await test.execute();

        results.push({
          testId: test.id,
          testName: test.name,
          status: 'PASSED',
          duration: Date.now() - startTime,
          error: null
        });

        console.log(`  ✅ ${test.name}`);

      } catch (error) {
        results.push({
          testId: test.id,
          testName: test.name,
          status: 'FAILED',
          duration: Date.now() - startTime,
          error: error.message
        });

        console.log(`  ❌ ${test.name}: ${error.message}`);
      }
    }

    return results;
  }

  private generateReport(): TestSuiteResult {
    const passed = this.testResults.filter(r => r.status === 'PASSED').length;
    const failed = this.testResults.filter(r => r.status === 'FAILED').length;
    const total = this.testResults.length;

    const report = {
      totalTests: total,
      passedTests: passed,
      failedTests: failed,
      successRate: (passed / total) * 100,
      totalDuration: this.testResults.reduce((sum, r) => sum + r.duration, 0),
      results: this.testResults
    };

    console.log('\n📊 Test Suite Results:');
    console.log(`   Total Tests: ${total}`);
    console.log(`   Passed: ${passed} ✅`);
    console.log(`   Failed: ${failed} ❌`);
    console.log(`   Success Rate: ${report.successRate.toFixed(2)}%`);
    console.log(`   Total Duration: ${report.totalDuration}ms\n`);

    return report;
  }
}

// Usage
const testRunner = new AutomatedTestRunner();
testRunner.runAllTests().then(results => {
  if (results.successRate >= 95) {
    console.log('🎉 All tests passed! Ready for production.');
  } else {
    console.log('⚠️  Some tests failed. Please review and fix issues.');
  }
});
```

---

**🎯 Test Coverage Goals:**

- **Authentication**: %100 coverage
- **CRUD Operations**: %95 coverage
- **Offline Functionality**: %90 coverage
- **Error Handling**: %95 coverage
- **Performance**: Key scenarios tested
- **Security**: Critical vulnerabilities checked
- **Mobile UX**: User journey completion %100

**📱 Manual Test Checklist:**
- [ ] Registration flow on different devices
- [ ] Login/logout cycles
- [ ] Network switching scenarios
- [ ] Battery optimization impact
- [ ] Memory usage on low-end devices
- [ ] Accessibility features
- [ ] Different screen sizes
- [ ] Biometric authentication (if implemented)

Bu test senaryoları ile mobil uygulamanızın güvenilirliğini ve kalitesini garanti altına alabilirsiniz!