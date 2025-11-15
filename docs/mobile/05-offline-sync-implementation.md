# WebCarpetApp Çevrimdışı Senkronizasyon Uygulama Rehberi

## Offline Sync Architecture

### 🔄 Senkronizasyon Stratejisi

WebCarpetApp API'si kapsamlı offline sync desteği sunar:
- **Offline Operation Logging**: Çevrimdışı işlemleri kaydetme
- **Conflict Resolution**: Çakışma çözümleme
- **Sync Sessions**: Senkronizasyon oturum yönetimi
- **Auto Retry**: Otomatik yeniden deneme

### **API Endpoints**
```
POST /api/offline-sync/start-session
POST /api/offline-sync/upload-operations
POST /api/offline-sync/download-changes
POST /api/offline-sync/resolve-conflicts
POST /api/offline-sync/complete-session
```

## 1. Offline Data Storage

### **Local Database Schema**
```typescript
// SQLite/IndexedDB Schema for mobile
interface LocalDatabase {
  // Cached API data
  customers: Customer[];
  orders: Order[];
  products: Product[];
  vehicles: Vehicle[];

  // Offline operations queue
  pendingOperations: OfflineOperation[];

  // Sync metadata
  syncSessions: SyncSession[];
  lastSyncTimestamp: string;

  // Conflict resolution
  conflicts: DataConflict[];
}

interface OfflineOperation {
  id: string;
  type: 'CREATE' | 'UPDATE' | 'DELETE';
  entityType: 'Customer' | 'Order' | 'Product' | 'Vehicle';
  entityId: string;
  data: any;
  timestamp: string;
  status: 'PENDING' | 'SYNCING' | 'COMPLETED' | 'FAILED';
  retryCount: number;
}

interface SyncSession {
  id: string;
  startTime: string;
  endTime?: string;
  status: 'IN_PROGRESS' | 'COMPLETED' | 'FAILED';
  operationsCount: number;
  conflictsCount: number;
}
```

### **Local Storage Manager**
```typescript
class OfflineStorageManager {
  private db: Database;

  async initialize() {
    this.db = await this.openDatabase();
    await this.createTables();
  }

  // Cache API responses for offline access
  async cacheEntities<T>(entityType: string, entities: T[], timestamp?: string) {
    await this.db.transaction(async (tx) => {
      // Clear existing cache for this entity type
      await tx.execute(`DELETE FROM ${entityType}_cache WHERE tenant_id = ?`, [this.getCurrentTenant()]);

      // Insert new data
      for (const entity of entities) {
        await tx.execute(
          `INSERT INTO ${entityType}_cache (id, data, cached_at, tenant_id) VALUES (?, ?, ?, ?)`,
          [entity.id, JSON.stringify(entity), timestamp || new Date().toISOString(), this.getCurrentTenant()]
        );
      }
    });
  }

  // Get cached entities
  async getCachedEntities<T>(entityType: string): Promise<T[]> {
    const result = await this.db.query(
      `SELECT data FROM ${entityType}_cache WHERE tenant_id = ? ORDER BY cached_at DESC`,
      [this.getCurrentTenant()]
    );

    return result.rows.map(row => JSON.parse(row.data));
  }

  // Add offline operation to queue
  async addOfflineOperation(operation: Omit<OfflineOperation, 'id' | 'timestamp' | 'status' | 'retryCount'>) {
    const offlineOp: OfflineOperation = {
      ...operation,
      id: this.generateId(),
      timestamp: new Date().toISOString(),
      status: 'PENDING',
      retryCount: 0
    };

    await this.db.execute(
      `INSERT INTO offline_operations (id, type, entity_type, entity_id, data, timestamp, status, retry_count, tenant_id)
       VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)`,
      [offlineOp.id, offlineOp.type, offlineOp.entityType, offlineOp.entityId,
       JSON.stringify(offlineOp.data), offlineOp.timestamp, offlineOp.status, offlineOp.retryCount, this.getCurrentTenant()]
    );

    // Update local cache immediately for better UX
    await this.updateLocalCache(operation);
  }

  // Get pending operations
  async getPendingOperations(): Promise<OfflineOperation[]> {
    const result = await this.db.query(
      `SELECT * FROM offline_operations WHERE status = 'PENDING' AND tenant_id = ? ORDER BY timestamp ASC`,
      [this.getCurrentTenant()]
    );

    return result.rows.map(row => ({
      id: row.id,
      type: row.type,
      entityType: row.entity_type,
      entityId: row.entity_id,
      data: JSON.parse(row.data),
      timestamp: row.timestamp,
      status: row.status,
      retryCount: row.retry_count
    }));
  }
}
```

## 2. Offline-First CRUD Operations

### **Offline-Aware API Client**
```typescript
class OfflineAwareApiClient {
  private storage: OfflineStorageManager;
  private isOnline: boolean;

  constructor(storage: OfflineStorageManager) {
    this.storage = storage;
    this.isOnline = navigator.onLine;

    // Listen for online/offline events
    window.addEventListener('online', () => {
      this.isOnline = true;
      this.syncPendingOperations();
    });

    window.addEventListener('offline', () => {
      this.isOnline = false;
    });
  }

  // Create entity (offline-first)
  async createCustomer(customerData: CreateCustomerDto): Promise<CustomerDto> {
    const tempId = `temp_${Date.now()}`;
    const customerWithTempId = { ...customerData, id: tempId };

    if (this.isOnline) {
      try {
        // Try online create first
        const response = await this.onlineApiClient.post('/api/app/customer/create-with-response', customerData);

        if (response.success) {
          // Cache the created entity
          await this.storage.cacheEntities('customers', [response.data]);
          return response.data;
        }
      } catch (error) {
        // Fall back to offline mode
        console.warn('Online create failed, falling back to offline:', error);
      }
    }

    // Offline create
    await this.storage.addOfflineOperation({
      type: 'CREATE',
      entityType: 'Customer',
      entityId: tempId,
      data: customerData
    });

    // Return optimistic result
    return customerWithTempId;
  }

  // Update entity (offline-first)
  async updateCustomer(customerId: string, updates: Partial<CustomerDto>): Promise<CustomerDto> {
    if (this.isOnline) {
      try {
        const response = await this.onlineApiClient.put(`/api/app/customer/update-with-response/${customerId}`, updates);

        if (response.success) {
          await this.storage.cacheEntities('customers', [response.data]);
          return response.data;
        }
      } catch (error) {
        console.warn('Online update failed, falling back to offline:', error);
      }
    }

    // Offline update
    await this.storage.addOfflineOperation({
      type: 'UPDATE',
      entityType: 'Customer',
      entityId: customerId,
      data: updates
    });

    // Update local cache and return optimistic result
    const cachedCustomers = await this.storage.getCachedEntities<CustomerDto>('customers');
    const existingCustomer = cachedCustomers.find(c => c.id === customerId);
    const updatedCustomer = { ...existingCustomer, ...updates };

    await this.storage.cacheEntities('customers',
      cachedCustomers.map(c => c.id === customerId ? updatedCustomer : c)
    );

    return updatedCustomer;
  }

  // Get entities (cache-first)
  async getCustomers(params?: any): Promise<PagedResultDto<CustomerDto>> {
    if (this.isOnline) {
      try {
        const response = await this.onlineApiClient.get('/api/app/customer/list-with-response', { params });

        if (response.success) {
          // Cache fresh data
          await this.storage.cacheEntities('customers', response.data.items);
          return response.data;
        }
      } catch (error) {
        console.warn('Online fetch failed, using cached data:', error);
      }
    }

    // Return cached data
    const cachedCustomers = await this.storage.getCachedEntities<CustomerDto>('customers');

    // Apply client-side filtering if needed
    let filteredCustomers = cachedCustomers;
    if (params?.name) {
      filteredCustomers = cachedCustomers.filter(c =>
        c.name.toLowerCase().includes(params.name.toLowerCase())
      );
    }

    // Apply pagination
    const startIndex = params?.skipCount || 0;
    const pageSize = params?.maxResultCount || 10;
    const paginatedItems = filteredCustomers.slice(startIndex, startIndex + pageSize);

    return {
      items: paginatedItems,
      totalCount: filteredCustomers.length
    };
  }
}
```

## 3. Sync Process Implementation

### **Sync Manager**
```typescript
class SyncManager {
  private apiClient: ApiClient;
  private storage: OfflineStorageManager;
  private conflictResolver: ConflictResolver;

  async startSync(): Promise<SyncResult> {
    if (!navigator.onLine) {
      throw new Error('Cannot sync while offline');
    }

    try {
      // 1. Start sync session
      const sessionResponse = await this.apiClient.post('/api/offline-sync/start-session');
      const sessionId = sessionResponse.data.sessionId;

      // 2. Upload pending operations
      const pendingOps = await this.storage.getPendingOperations();

      if (pendingOps.length > 0) {
        await this.uploadOperations(sessionId, pendingOps);
      }

      // 3. Download server changes
      const serverChanges = await this.downloadChanges(sessionId);

      // 4. Resolve conflicts
      const conflicts = await this.detectConflicts(pendingOps, serverChanges);
      if (conflicts.length > 0) {
        await this.resolveConflicts(sessionId, conflicts);
      }

      // 5. Apply clean changes to local cache
      await this.applyServerChanges(serverChanges);

      // 6. Mark operations as completed
      await this.markOperationsCompleted(pendingOps.map(op => op.id));

      // 7. Complete sync session
      await this.apiClient.post(`/api/offline-sync/complete-session/${sessionId}`);

      return {
        success: true,
        operationsUploaded: pendingOps.length,
        changesDownloaded: serverChanges.length,
        conflictsResolved: conflicts.length
      };

    } catch (error) {
      console.error('Sync failed:', error);
      return {
        success: false,
        error: error.message
      };
    }
  }

  private async uploadOperations(sessionId: string, operations: OfflineOperation[]) {
    // Group operations by entity type for better performance
    const groupedOps = operations.reduce((acc, op) => {
      acc[op.entityType] = acc[op.entityType] || [];
      acc[op.entityType].push(op);
      return acc;
    }, {} as Record<string, OfflineOperation[]>);

    for (const [entityType, ops] of Object.entries(groupedOps)) {
      await this.apiClient.post('/api/offline-sync/upload-operations', {
        sessionId,
        entityType,
        operations: ops.map(op => ({
          id: op.id,
          type: op.type,
          entityId: op.entityId,
          data: op.data,
          timestamp: op.timestamp
        }))
      });

      // Mark as syncing
      await this.storage.updateOperationsStatus(ops.map(op => op.id), 'SYNCING');
    }
  }

  private async downloadChanges(sessionId: string): Promise<ServerChange[]> {
    const lastSyncTimestamp = await this.storage.getLastSyncTimestamp();

    const response = await this.apiClient.post('/api/offline-sync/download-changes', {
      sessionId,
      lastSyncTimestamp
    });

    return response.data.changes;
  }

  private async detectConflicts(localOps: OfflineOperation[], serverChanges: ServerChange[]): Promise<DataConflict[]> {
    const conflicts: DataConflict[] = [];

    for (const localOp of localOps) {
      const conflictingServerChange = serverChanges.find(change =>
        change.entityType === localOp.entityType &&
        change.entityId === localOp.entityId &&
        change.timestamp > localOp.timestamp
      );

      if (conflictingServerChange) {
        conflicts.push({
          id: this.generateConflictId(),
          entityType: localOp.entityType,
          entityId: localOp.entityId,
          localChange: localOp,
          serverChange: conflictingServerChange,
          status: 'PENDING'
        });
      }
    }

    return conflicts;
  }
}

interface SyncResult {
  success: boolean;
  operationsUploaded?: number;
  changesDownloaded?: number;
  conflictsResolved?: number;
  error?: string;
}

interface ServerChange {
  entityType: string;
  entityId: string;
  type: 'CREATE' | 'UPDATE' | 'DELETE';
  data: any;
  timestamp: string;
}

interface DataConflict {
  id: string;
  entityType: string;
  entityId: string;
  localChange: OfflineOperation;
  serverChange: ServerChange;
  status: 'PENDING' | 'RESOLVED';
}
```

## 4. Conflict Resolution

### **Conflict Resolver**
```typescript
class ConflictResolver {

  // Automatic conflict resolution strategies
  async resolveConflict(conflict: DataConflict, strategy: ConflictStrategy): Promise<ConflictResolution> {
    switch (strategy) {
      case 'SERVER_WINS':
        return {
          resolution: 'ACCEPT_SERVER',
          resolvedData: conflict.serverChange.data
        };

      case 'CLIENT_WINS':
        return {
          resolution: 'ACCEPT_CLIENT',
          resolvedData: conflict.localChange.data
        };

      case 'MERGE':
        return {
          resolution: 'MERGE',
          resolvedData: this.mergeChanges(conflict.localChange.data, conflict.serverChange.data)
        };

      case 'MANUAL':
        // Store for manual resolution
        await this.storage.saveConflictForManualResolution(conflict);
        return {
          resolution: 'MANUAL_REQUIRED',
          requiresUserInput: true
        };
    }
  }

  private mergeChanges(localData: any, serverData: any): any {
    // Smart merge logic - prefer non-null values
    const merged = { ...serverData };

    for (const [key, value] of Object.entries(localData)) {
      // Keep local changes for specific fields
      if (this.isClientPreferredField(key) && value != null) {
        merged[key] = value;
      }

      // For timestamps, keep the latest
      if (key.includes('Date') || key.includes('Time')) {
        if (new Date(value) > new Date(serverData[key])) {
          merged[key] = value;
        }
      }
    }

    return merged;
  }

  private isClientPreferredField(fieldName: string): boolean {
    // Fields where client changes should take precedence
    const clientPreferredFields = [
      'coordinates',  // GPS updates
      'lastContactDate',
      'notes',
      'customFields'
    ];

    return clientPreferredFields.includes(fieldName);
  }
}

type ConflictStrategy = 'SERVER_WINS' | 'CLIENT_WINS' | 'MERGE' | 'MANUAL';

interface ConflictResolution {
  resolution: 'ACCEPT_SERVER' | 'ACCEPT_CLIENT' | 'MERGE' | 'MANUAL_REQUIRED';
  resolvedData?: any;
  requiresUserInput?: boolean;
}
```

## 5. Background Sync

### **Background Sync Service**
```typescript
class BackgroundSyncService {
  private syncInterval?: number;
  private isSyncing = false;

  startBackgroundSync(intervalMinutes: number = 5) {
    this.syncInterval = setInterval(async () => {
      if (!this.isSyncing && navigator.onLine) {
        await this.performBackgroundSync();
      }
    }, intervalMinutes * 60 * 1000);
  }

  stopBackgroundSync() {
    if (this.syncInterval) {
      clearInterval(this.syncInterval);
      this.syncInterval = undefined;
    }
  }

  private async performBackgroundSync() {
    this.isSyncing = true;

    try {
      const syncManager = new SyncManager(this.apiClient, this.storage);
      const result = await syncManager.startSync();

      if (result.success) {
        this.notifyUser(`Senkronizasyon tamamlandı: ${result.operationsUploaded} işlem yüklendi`);
      } else {
        console.error('Background sync failed:', result.error);
      }
    } catch (error) {
      console.error('Background sync error:', error);
    } finally {
      this.isSyncing = false;
    }
  }

  private notifyUser(message: string) {
    // Use platform-specific notification
    if ('serviceWorker' in navigator) {
      // Web notification
      new Notification('WebCarpetApp Sync', { body: message });
    } else {
      // Mobile push notification
      PushNotification.localNotification({
        title: 'Sync Complete',
        message: message
      });
    }
  }
}
```

## 6. Offline State Management

### **Offline State Manager**
```typescript
class OfflineStateManager {
  private subscribers: Array<(isOnline: boolean) => void> = [];
  private isOnline = navigator.onLine;

  constructor() {
    window.addEventListener('online', () => this.updateOnlineStatus(true));
    window.addEventListener('offline', () => this.updateOnlineStatus(false));
  }

  private updateOnlineStatus(online: boolean) {
    this.isOnline = online;
    this.notifySubscribers(online);

    if (online) {
      // Trigger sync when coming back online
      this.triggerSync();
    }
  }

  subscribe(callback: (isOnline: boolean) => void) {
    this.subscribers.push(callback);

    // Return unsubscribe function
    return () => {
      this.subscribers = this.subscribers.filter(sub => sub !== callback);
    };
  }

  private notifySubscribers(isOnline: boolean) {
    this.subscribers.forEach(callback => callback(isOnline));
  }

  getStatus() {
    return {
      isOnline: this.isOnline,
      hasPendingOperations: this.storage.getPendingOperationsCount() > 0,
      lastSyncTime: this.storage.getLastSyncTimestamp()
    };
  }

  private async triggerSync() {
    // Delay sync a bit to ensure stable connection
    setTimeout(async () => {
      try {
        const syncManager = new SyncManager(this.apiClient, this.storage);
        await syncManager.startSync();
      } catch (error) {
        console.error('Auto-sync failed:', error);
      }
    }, 2000);
  }
}
```

## 7. UI Integration

### **Offline Status Component**
```typescript
// React example
const OfflineStatusIndicator: React.FC = () => {
  const [isOnline, setIsOnline] = useState(navigator.onLine);
  const [pendingOpsCount, setPendingOpsCount] = useState(0);

  useEffect(() => {
    const offlineManager = new OfflineStateManager();

    const unsubscribe = offlineManager.subscribe((online) => {
      setIsOnline(online);
    });

    return unsubscribe;
  }, []);

  if (!isOnline) {
    return (
      <View style={styles.offlineBar}>
        <Text>Çevrimdışı - Değişiklikler cihazda saklanıyor</Text>
        {pendingOpsCount > 0 && (
          <Text style={styles.pendingCount}>{pendingOpsCount} bekleyen işlem</Text>
        )}
      </View>
    );
  }

  return null;
};

const styles = StyleSheet.create({
  offlineBar: {
    backgroundColor: '#f39c12',
    padding: 8,
    alignItems: 'center'
  },
  pendingCount: {
    fontSize: 12,
    fontWeight: 'bold'
  }
});
```

### **Manual Sync Button**
```typescript
const SyncButton: React.FC = () => {
  const [isSyncing, setIsSyncing] = useState(false);

  const handleManualSync = async () => {
    if (!navigator.onLine) {
      Alert.alert('Hata', 'Senkronizasyon için internet bağlantısı gerekli');
      return;
    }

    setIsSyncing(true);

    try {
      const syncManager = new SyncManager(apiClient, storage);
      const result = await syncManager.startSync();

      if (result.success) {
        Alert.alert('Başarılı', `${result.operationsUploaded} işlem senkronize edildi`);
      } else {
        Alert.alert('Hata', 'Senkronizasyon başarısız: ' + result.error);
      }
    } catch (error) {
      Alert.alert('Hata', 'Senkronizasyon sırasında hata oluştu');
    } finally {
      setIsSyncing(false);
    }
  };

  return (
    <Button
      title={isSyncing ? 'Senkronize ediliyor...' : 'Senkronize Et'}
      onPress={handleManualSync}
      disabled={isSyncing || !navigator.onLine}
    />
  );
};
```

---

**💡 Offline Sync Best Practices:**

1. **Cache Critical Data** - Müşteriler, ürünler, aktif siparişler
2. **Optimistic Updates** - UI'yi hemen güncelle, sonra sync et
3. **Conflict Resolution Strategy** - Merge logic'i business rules'a göre ayarla
4. **Background Sync** - Kullanıcı fark etmeden otomatik sync
5. **Storage Limits** - Mobil cihazda storage limitlerini göz önünde bulundur
6. **Data Validation** - Offline data'yı sync öncesi validate et
7. **User Feedback** - Sync durumu hakkında kullanıcıyı bilgilendir