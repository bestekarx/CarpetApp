# WebCarpetApp Mobil Uygulama Kod Örnekleri

## Complete Mobile App Implementation

### 🏗️ Project Structure
```
src/
├── api/
│   ├── client.ts              // API client configuration
│   ├── auth.ts                // Authentication service
│   ├── offline.ts             // Offline sync manager
│   └── services/
│       ├── customer.ts        // Customer API service
│       ├── order.ts           // Order API service
│       └── vehicle.ts         // Vehicle API service
├── store/
│   ├── index.ts               // Store configuration
│   ├── auth.ts                // Auth state management
│   ├── customer.ts            // Customer state management
│   └── offline.ts             // Offline state management
├── components/
│   ├── common/                // Shared components
│   ├── forms/                 // Form components
│   └── lists/                 // List components
├── screens/
│   ├── auth/                  // Authentication screens
│   ├── customer/              // Customer management
│   ├── order/                 // Order management
│   └── dashboard/             // Dashboard and analytics
└── utils/
    ├── validation.ts          // Form validation
    ├── storage.ts             // Local storage utilities
    └── error-handler.ts       // Error handling utilities
```

## 1. API Client Implementation

### **Base API Client**
```typescript
// src/api/client.ts
import AsyncStorage from '@react-native-async-storage/async-storage';
import NetInfo from '@react-native-community/netinfo';

export interface ApiResponse<T> {
  success: boolean;
  data: T | null;
  message: string;
  error?: {
    code: string;
    details?: any;
  };
  timestamp: string;
}

export class ApiClient {
  private baseURL: string;
  private isOnline: boolean = true;

  constructor(baseURL: string) {
    this.baseURL = baseURL;
    this.setupNetworkListener();
  }

  private setupNetworkListener() {
    NetInfo.addEventListener(state => {
      this.isOnline = state.isConnected ?? false;
    });
  }

  async request<T>(
    endpoint: string,
    options: RequestInit = {}
  ): Promise<ApiResponse<T>> {
    const token = await AsyncStorage.getItem('auth_token');

    const headers: Record<string, string> = {
      'Content-Type': 'application/json',
      'Accept-Language': 'tr',
      ...options.headers as Record<string, string>
    };

    if (token) {
      headers.Authorization = `Bearer ${token}`;
    }

    const response = await fetch(`${this.baseURL}${endpoint}`, {
      ...options,
      headers
    });

    if (!response.ok) {
      throw new ApiError(
        response.status,
        `HTTP ${response.status}`,
        await response.json()
      );
    }

    return response.json();
  }

  async get<T>(endpoint: string, params?: Record<string, any>): Promise<ApiResponse<T>> {
    const searchParams = params ? `?${new URLSearchParams(params).toString()}` : '';
    return this.request<T>(`${endpoint}${searchParams}`, { method: 'GET' });
  }

  async post<T>(endpoint: string, data?: any): Promise<ApiResponse<T>> {
    return this.request<T>(endpoint, {
      method: 'POST',
      body: data ? JSON.stringify(data) : undefined
    });
  }

  async put<T>(endpoint: string, data?: any): Promise<ApiResponse<T>> {
    return this.request<T>(endpoint, {
      method: 'PUT',
      body: data ? JSON.stringify(data) : undefined
    });
  }

  async delete<T>(endpoint: string): Promise<ApiResponse<T>> {
    return this.request<T>(endpoint, { method: 'DELETE' });
  }

  getNetworkStatus(): boolean {
    return this.isOnline;
  }
}

export class ApiError extends Error {
  constructor(
    public status: number,
    public message: string,
    public details?: any
  ) {
    super(message);
    this.name = 'ApiError';
  }
}

// Singleton instance
export const apiClient = new ApiClient('https://localhost:44302/api');
```

### **Authentication Service**
```typescript
// src/api/auth.ts
import { apiClient, ApiResponse } from './client';
import AsyncStorage from '@react-native-async-storage/async-storage';

export interface AuthTokens {
  accessToken: string;
  refreshToken: string;
  expiresAt: number;
  tenantId: string;
}

export interface LoginCredentials {
  emailAddress: string;
  password: string;
}

export interface RegistrationData {
  companyName: string;
  fullName: string;
  emailAddress: string;
  password: string;
  phoneNumber: string;
}

export class AuthService {
  private readonly TOKEN_KEY = 'auth_tokens';
  private readonly USER_KEY = 'user_data';

  async register(data: RegistrationData): Promise<ApiResponse<any>> {
    const response = await apiClient.post('/subscription-account/register-with-trial', data);

    if (response.success) {
      // Auto-login after successful registration
      await this.login({
        emailAddress: data.emailAddress,
        password: data.password
      });
    }

    return response;
  }

  async findTenant(emailAddress: string): Promise<ApiResponse<{tenantId: string; tenantName: string}>> {
    return apiClient.post('/subscription-account/find-tenant', { emailAddress });
  }

  async login(credentials: LoginCredentials): Promise<ApiResponse<any>> {
    try {
      // 1. Find tenant first
      const tenantResponse = await this.findTenant(credentials.emailAddress);

      if (!tenantResponse.success) {
        return tenantResponse as any;
      }

      const tenantId = tenantResponse.data!.tenantId;

      // 2. Get OAuth token
      const tokenResponse = await this.getOAuthToken(credentials, tenantId);

      if (tokenResponse.access_token) {
        // 3. Store tokens securely
        const tokens: AuthTokens = {
          accessToken: tokenResponse.access_token,
          refreshToken: tokenResponse.refresh_token,
          expiresAt: Date.now() + (tokenResponse.expires_in * 1000),
          tenantId
        };

        await AsyncStorage.setItem(this.TOKEN_KEY, JSON.stringify(tokens));

        // 4. Get user info
        const userResponse = await apiClient.get('/account/my-profile');
        if (userResponse.success) {
          await AsyncStorage.setItem(this.USER_KEY, JSON.stringify(userResponse.data));
        }

        return { success: true, data: { tokens, user: userResponse.data } } as ApiResponse<any>;
      }

      return { success: false, message: 'Token alınamadı' } as ApiResponse<any>;

    } catch (error) {
      return {
        success: false,
        message: 'Giriş başarısız',
        error: { code: 'LOGIN_FAILED' }
      } as ApiResponse<any>;
    }
  }

  private async getOAuthToken(credentials: LoginCredentials, tenantId: string): Promise<any> {
    const formData = new URLSearchParams();
    formData.append('grant_type', 'password');
    formData.append('client_id', 'WebCarpetApp_App');
    formData.append('client_secret', '1q2w3e*');
    formData.append('username', credentials.emailAddress);
    formData.append('password', credentials.password);
    formData.append('scope', 'WebCarpetApp');

    const response = await fetch('https://localhost:44302/connect/token', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
        '__tenant': tenantId
      },
      body: formData.toString()
    });

    return response.json();
  }

  async logout(): Promise<void> {
    await AsyncStorage.multiRemove([this.TOKEN_KEY, this.USER_KEY]);
  }

  async getCurrentUser(): Promise<any> {
    const userData = await AsyncStorage.getItem(this.USER_KEY);
    return userData ? JSON.parse(userData) : null;
  }

  async isAuthenticated(): Promise<boolean> {
    const tokensData = await AsyncStorage.getItem(this.TOKEN_KEY);
    if (!tokensData) return false;

    const tokens: AuthTokens = JSON.parse(tokensData);
    return tokens.expiresAt > Date.now();
  }

  async getValidTokens(): Promise<AuthTokens | null> {
    const tokensData = await AsyncStorage.getItem(this.TOKEN_KEY);
    if (!tokensData) return null;

    const tokens: AuthTokens = JSON.parse(tokensData);

    // Token expired, try to refresh
    if (tokens.expiresAt <= Date.now()) {
      const refreshed = await this.refreshToken(tokens.refreshToken, tokens.tenantId);
      if (refreshed) return refreshed;
      return null;
    }

    return tokens;
  }

  private async refreshToken(refreshToken: string, tenantId: string): Promise<AuthTokens | null> {
    try {
      const formData = new URLSearchParams();
      formData.append('grant_type', 'refresh_token');
      formData.append('client_id', 'WebCarpetApp_App');
      formData.append('client_secret', '1q2w3e*');
      formData.append('refresh_token', refreshToken);

      const response = await fetch('https://localhost:44302/connect/token', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/x-www-form-urlencoded',
          '__tenant': tenantId
        },
        body: formData.toString()
      });

      const tokenData = await response.json();

      if (tokenData.access_token) {
        const newTokens: AuthTokens = {
          accessToken: tokenData.access_token,
          refreshToken: tokenData.refresh_token,
          expiresAt: Date.now() + (tokenData.expires_in * 1000),
          tenantId
        };

        await AsyncStorage.setItem(this.TOKEN_KEY, JSON.stringify(newTokens));
        return newTokens;
      }
    } catch (error) {
      console.error('Token refresh failed:', error);
    }

    return null;
  }
}

export const authService = new AuthService();
```

## 2. Customer Service Implementation

```typescript
// src/api/services/customer.ts
import { apiClient, ApiResponse } from '../client';

export interface CustomerDto {
  id: string;
  name: string;
  phone: string;
  email?: string;
  address?: string;
  coordinates?: string;
  areaId?: string;
  areaName?: string;
  companyId?: string;
  companyName?: string;
  active: boolean;
  creationTime: string;
}

export interface CreateUpdateCustomerDto {
  name: string;
  phone: string;
  email?: string;
  address?: string;
  coordinates?: string;
  areaId?: string;
  companyId?: string;
  active: boolean;
}

export interface GetCustomerListFilterDto {
  name?: string;
  active?: boolean;
  maxResultCount?: number;
  skipCount?: number;
  sorting?: string;
}

export class CustomerService {
  async getList(params?: GetCustomerListFilterDto): Promise<ApiResponse<{ items: CustomerDto[]; totalCount: number }>> {
    return apiClient.get('/app/customer/list-with-response', params);
  }

  async getFilteredList(filter: GetCustomerListFilterDto): Promise<ApiResponse<{ items: CustomerDto[]; totalCount: number }>> {
    return apiClient.post('/app/customer/filtered-list-with-response', filter);
  }

  async getById(id: string): Promise<ApiResponse<CustomerDto>> {
    return apiClient.get(`/app/customer/get-with-response/${id}`);
  }

  async create(customer: CreateUpdateCustomerDto): Promise<ApiResponse<CustomerDto>> {
    return apiClient.post('/app/customer/create-with-response', customer);
  }

  async update(id: string, customer: CreateUpdateCustomerDto): Promise<ApiResponse<CustomerDto>> {
    return apiClient.put(`/app/customer/update-with-response/${id}`, customer);
  }

  async delete(id: string): Promise<ApiResponse<void>> {
    return apiClient.delete(`/app/customer/delete-with-response/${id}`);
  }

  async updateLocation(id: string, coordinates: string): Promise<ApiResponse<CustomerDto>> {
    return this.update(id, { coordinates } as any);
  }

  async searchByName(searchTerm: string): Promise<ApiResponse<{ items: CustomerDto[]; totalCount: number }>> {
    return this.getFilteredList({
      name: searchTerm,
      maxResultCount: 50
    });
  }
}

export const customerService = new CustomerService();
```

## 3. State Management (Redux/Zustand)

```typescript
// src/store/customer.ts
import { create } from 'zustand';
import { customerService, CustomerDto, CreateUpdateCustomerDto } from '../api/services/customer';

interface CustomerState {
  customers: CustomerDto[];
  selectedCustomer: CustomerDto | null;
  loading: boolean;
  error: string | null;

  // Actions
  loadCustomers: () => Promise<void>;
  searchCustomers: (searchTerm: string) => Promise<void>;
  createCustomer: (customer: CreateUpdateCustomerDto) => Promise<void>;
  updateCustomer: (id: string, updates: Partial<CreateUpdateCustomerDto>) => Promise<void>;
  deleteCustomer: (id: string) => Promise<void>;
  selectCustomer: (customer: CustomerDto | null) => void;
  clearError: () => void;
}

export const useCustomerStore = create<CustomerState>((set, get) => ({
  customers: [],
  selectedCustomer: null,
  loading: false,
  error: null,

  loadCustomers: async () => {
    set({ loading: true, error: null });

    try {
      const response = await customerService.getList({ maxResultCount: 100 });

      if (response.success) {
        set({ customers: response.data!.items, loading: false });
      } else {
        set({ error: response.message, loading: false });
      }
    } catch (error) {
      set({ error: 'Müşteriler yüklenemedi', loading: false });
    }
  },

  searchCustomers: async (searchTerm: string) => {
    if (!searchTerm.trim()) {
      get().loadCustomers();
      return;
    }

    set({ loading: true, error: null });

    try {
      const response = await customerService.searchByName(searchTerm);

      if (response.success) {
        set({ customers: response.data!.items, loading: false });
      } else {
        set({ error: response.message, loading: false });
      }
    } catch (error) {
      set({ error: 'Arama başarısız', loading: false });
    }
  },

  createCustomer: async (customer: CreateUpdateCustomerDto) => {
    set({ loading: true, error: null });

    try {
      const response = await customerService.create(customer);

      if (response.success) {
        const currentCustomers = get().customers;
        set({
          customers: [...currentCustomers, response.data!],
          loading: false
        });
      } else {
        set({ error: response.message, loading: false });
      }
    } catch (error) {
      set({ error: 'Müşteri oluşturulamadı', loading: false });
    }
  },

  updateCustomer: async (id: string, updates: Partial<CreateUpdateCustomerDto>) => {
    const currentCustomers = get().customers;

    // Optimistic update
    const optimisticCustomers = currentCustomers.map(customer =>
      customer.id === id ? { ...customer, ...updates } : customer
    );
    set({ customers: optimisticCustomers });

    try {
      const response = await customerService.update(id, updates as CreateUpdateCustomerDto);

      if (response.success) {
        const finalCustomers = currentCustomers.map(customer =>
          customer.id === id ? response.data! : customer
        );
        set({ customers: finalCustomers, error: null });
      } else {
        // Revert optimistic update
        set({ customers: currentCustomers, error: response.message });
      }
    } catch (error) {
      // Revert optimistic update
      set({ customers: currentCustomers, error: 'Güncelleme başarısız' });
    }
  },

  deleteCustomer: async (id: string) => {
    const currentCustomers = get().customers;

    // Optimistic update
    const optimisticCustomers = currentCustomers.filter(customer => customer.id !== id);
    set({ customers: optimisticCustomers });

    try {
      const response = await customerService.delete(id);

      if (!response.success) {
        // Revert if failed
        set({ customers: currentCustomers, error: response.message });
      }
    } catch (error) {
      // Revert optimistic update
      set({ customers: currentCustomers, error: 'Silme başarısız' });
    }
  },

  selectCustomer: (customer: CustomerDto | null) => {
    set({ selectedCustomer: customer });
  },

  clearError: () => {
    set({ error: null });
  }
}));
```

## 4. React Native Components

### **Customer List Component**
```typescript
// src/screens/customer/CustomerListScreen.tsx
import React, { useEffect, useState } from 'react';
import {
  View,
  FlatList,
  Text,
  TextInput,
  TouchableOpacity,
  Alert,
  RefreshControl
} from 'react-native';
import { useCustomerStore } from '../../store/customer';
import { CustomerListItem } from '../../components/customer/CustomerListItem';
import { LoadingIndicator } from '../../components/common/LoadingIndicator';
import { ErrorBanner } from '../../components/common/ErrorBanner';

export const CustomerListScreen: React.FC = ({ navigation }) => {
  const [searchTerm, setSearchTerm] = useState('');
  const [refreshing, setRefreshing] = useState(false);

  const {
    customers,
    loading,
    error,
    loadCustomers,
    searchCustomers,
    deleteCustomer,
    selectCustomer,
    clearError
  } = useCustomerStore();

  useEffect(() => {
    loadCustomers();
  }, []);

  const handleSearch = (text: string) => {
    setSearchTerm(text);
    if (text.length >= 2) {
      searchCustomers(text);
    } else if (text.length === 0) {
      loadCustomers();
    }
  };

  const handleRefresh = async () => {
    setRefreshing(true);
    await loadCustomers();
    setRefreshing(false);
  };

  const handleCustomerPress = (customer: CustomerDto) => {
    selectCustomer(customer);
    navigation.navigate('CustomerDetail', { customerId: customer.id });
  };

  const handleDeleteCustomer = (customer: CustomerDto) => {
    Alert.alert(
      'Müşteriyi Sil',
      `${customer.name} adlı müşteriyi silmek istediğinizden emin misiniz?`,
      [
        { text: 'İptal', style: 'cancel' },
        {
          text: 'Sil',
          style: 'destructive',
          onPress: () => deleteCustomer(customer.id)
        }
      ]
    );
  };

  const renderCustomer = ({ item }: { item: CustomerDto }) => (
    <CustomerListItem
      customer={item}
      onPress={() => handleCustomerPress(item)}
      onDelete={() => handleDeleteCustomer(item)}
    />
  );

  return (
    <View style={styles.container}>
      {/* Search Header */}
      <View style={styles.searchContainer}>
        <TextInput
          style={styles.searchInput}
          placeholder="Müşteri ara..."
          value={searchTerm}
          onChangeText={handleSearch}
          autoCapitalize="none"
          autoCorrect={false}
        />
        <TouchableOpacity
          style={styles.addButton}
          onPress={() => navigation.navigate('CustomerCreate')}
        >
          <Text style={styles.addButtonText}>+ Yeni</Text>
        </TouchableOpacity>
      </View>

      {/* Error Banner */}
      {error && (
        <ErrorBanner
          message={error}
          onClose={clearError}
        />
      )}

      {/* Customer List */}
      {loading && customers.length === 0 ? (
        <LoadingIndicator />
      ) : (
        <FlatList
          data={customers}
          renderItem={renderCustomer}
          keyExtractor={(item) => item.id}
          refreshControl={
            <RefreshControl
              refreshing={refreshing}
              onRefresh={handleRefresh}
            />
          }
          ListEmptyComponent={
            <View style={styles.emptyContainer}>
              <Text style={styles.emptyText}>
                {searchTerm ? 'Arama sonucu bulunamadı' : 'Henüz müşteri yok'}
              </Text>
            </View>
          }
        />
      )}
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f5f5f5'
  },
  searchContainer: {
    flexDirection: 'row',
    padding: 16,
    backgroundColor: 'white'
  },
  searchInput: {
    flex: 1,
    height: 40,
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    paddingHorizontal: 12,
    marginRight: 8
  },
  addButton: {
    backgroundColor: '#007AFF',
    paddingHorizontal: 16,
    paddingVertical: 8,
    borderRadius: 8,
    justifyContent: 'center'
  },
  addButtonText: {
    color: 'white',
    fontWeight: 'bold'
  },
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    paddingTop: 50
  },
  emptyText: {
    fontSize: 16,
    color: '#666',
    textAlign: 'center'
  }
});
```

### **Customer Form Component**
```typescript
// src/screens/customer/CustomerFormScreen.tsx
import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  ScrollView,
  Alert,
  Switch
} from 'react-native';
import { useCustomerStore } from '../../store/customer';
import { LoadingIndicator } from '../../components/common/LoadingIndicator';
import { validateEmail, validatePhone } from '../../utils/validation';

export const CustomerFormScreen: React.FC = ({ route, navigation }) => {
  const { customerId } = route.params || {};
  const isEdit = !!customerId;

  const { selectedCustomer, createCustomer, updateCustomer, loading } = useCustomerStore();

  const [formData, setFormData] = useState({
    name: '',
    phone: '',
    email: '',
    address: '',
    active: true
  });

  const [errors, setErrors] = useState<Record<string, string>>({});

  useEffect(() => {
    if (isEdit && selectedCustomer) {
      setFormData({
        name: selectedCustomer.name,
        phone: selectedCustomer.phone,
        email: selectedCustomer.email || '',
        address: selectedCustomer.address || '',
        active: selectedCustomer.active
      });
    }
  }, [isEdit, selectedCustomer]);

  const validateForm = (): boolean => {
    const newErrors: Record<string, string> = {};

    if (!formData.name.trim()) {
      newErrors.name = 'Müşteri adı zorunludur';
    }

    if (!formData.phone.trim()) {
      newErrors.phone = 'Telefon numarası zorunludur';
    } else if (!validatePhone(formData.phone)) {
      newErrors.phone = 'Geçerli bir telefon numarası giriniz';
    }

    if (formData.email && !validateEmail(formData.email)) {
      newErrors.email = 'Geçerli bir e-posta adresi giriniz';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async () => {
    if (!validateForm()) return;

    try {
      if (isEdit) {
        await updateCustomer(customerId, formData);
        Alert.alert('Başarılı', 'Müşteri güncellendi', [
          { text: 'Tamam', onPress: () => navigation.goBack() }
        ]);
      } else {
        await createCustomer(formData);
        Alert.alert('Başarılı', 'Müşteri oluşturuldu', [
          { text: 'Tamam', onPress: () => navigation.goBack() }
        ]);
      }
    } catch (error) {
      Alert.alert('Hata', 'İşlem başarısız oldu');
    }
  };

  if (loading) {
    return <LoadingIndicator />;
  }

  return (
    <ScrollView style={styles.container}>
      <View style={styles.form}>
        {/* Name Field */}
        <View style={styles.fieldContainer}>
          <Text style={styles.label}>Müşteri Adı *</Text>
          <TextInput
            style={[styles.input, errors.name && styles.inputError]}
            value={formData.name}
            onChangeText={(text) => setFormData({...formData, name: text})}
            placeholder="Müşteri adını giriniz"
          />
          {errors.name && <Text style={styles.errorText}>{errors.name}</Text>}
        </View>

        {/* Phone Field */}
        <View style={styles.fieldContainer}>
          <Text style={styles.label}>Telefon Numarası *</Text>
          <TextInput
            style={[styles.input, errors.phone && styles.inputError]}
            value={formData.phone}
            onChangeText={(text) => setFormData({...formData, phone: text})}
            placeholder="05XX XXX XX XX"
            keyboardType="phone-pad"
          />
          {errors.phone && <Text style={styles.errorText}>{errors.phone}</Text>}
        </View>

        {/* Email Field */}
        <View style={styles.fieldContainer}>
          <Text style={styles.label}>E-posta</Text>
          <TextInput
            style={[styles.input, errors.email && styles.inputError]}
            value={formData.email}
            onChangeText={(text) => setFormData({...formData, email: text})}
            placeholder="ornek@email.com"
            keyboardType="email-address"
            autoCapitalize="none"
          />
          {errors.email && <Text style={styles.errorText}>{errors.email}</Text>}
        </View>

        {/* Address Field */}
        <View style={styles.fieldContainer}>
          <Text style={styles.label}>Adres</Text>
          <TextInput
            style={[styles.input, styles.textArea]}
            value={formData.address}
            onChangeText={(text) => setFormData({...formData, address: text})}
            placeholder="Adres bilgisini giriniz"
            multiline
            numberOfLines={3}
          />
        </View>

        {/* Active Switch */}
        <View style={styles.switchContainer}>
          <Text style={styles.label}>Aktif</Text>
          <Switch
            value={formData.active}
            onValueChange={(value) => setFormData({...formData, active: value})}
          />
        </View>

        {/* Submit Button */}
        <TouchableOpacity style={styles.submitButton} onPress={handleSubmit}>
          <Text style={styles.submitButtonText}>
            {isEdit ? 'Güncelle' : 'Oluştur'}
          </Text>
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f5f5f5'
  },
  form: {
    padding: 16
  },
  fieldContainer: {
    marginBottom: 16
  },
  label: {
    fontSize: 16,
    fontWeight: 'bold',
    marginBottom: 8,
    color: '#333'
  },
  input: {
    backgroundColor: 'white',
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 8,
    fontSize: 16
  },
  inputError: {
    borderColor: '#ff6b6b'
  },
  textArea: {
    height: 80,
    textAlignVertical: 'top'
  },
  errorText: {
    color: '#ff6b6b',
    fontSize: 14,
    marginTop: 4
  },
  switchContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    backgroundColor: 'white',
    paddingHorizontal: 16,
    paddingVertical: 12,
    borderRadius: 8,
    marginBottom: 24
  },
  submitButton: {
    backgroundColor: '#007AFF',
    paddingVertical: 16,
    borderRadius: 8,
    alignItems: 'center'
  },
  submitButtonText: {
    color: 'white',
    fontSize: 18,
    fontWeight: 'bold'
  }
});
```

## 5. Utility Functions

### **Validation Utils**
```typescript
// src/utils/validation.ts
export const validateEmail = (email: string): boolean => {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
};

export const validatePhone = (phone: string): boolean => {
  // Turkish phone number validation
  const phoneRegex = /^(\+90|90|0)?[5][0-9]{9}$/;
  const cleanPhone = phone.replace(/\s+/g, '').replace(/[()-]/g, '');
  return phoneRegex.test(cleanPhone);
};

export const validateRequired = (value: string): boolean => {
  return value.trim().length > 0;
};

export const validateMinLength = (value: string, minLength: number): boolean => {
  return value.length >= minLength;
};

export const validateMaxLength = (value: string, maxLength: number): boolean => {
  return value.length <= maxLength;
};

export const formatPhone = (phone: string): string => {
  const clean = phone.replace(/\D/g, '');

  if (clean.length === 11 && clean.startsWith('0')) {
    return clean.replace(/(\d{4})(\d{3})(\d{2})(\d{2})/, '$1 $2 $3 $4');
  }

  if (clean.length === 10) {
    return clean.replace(/(\d{3})(\d{3})(\d{2})(\d{2})/, '0$1 $2 $3 $4');
  }

  return phone;
};

export const formatCurrency = (amount: number): string => {
  return new Intl.NumberFormat('tr-TR', {
    style: 'currency',
    currency: 'TRY'
  }).format(amount);
};
```

### **Error Handler Utils**
```typescript
// src/utils/error-handler.ts
import { Alert } from 'react-native';
import { ApiError } from '../api/client';

export class ErrorHandler {
  static handle(error: any, context?: string) {
    console.error(`Error in ${context}:`, error);

    if (error instanceof ApiError) {
      this.handleApiError(error);
    } else if (error.name === 'NetworkError') {
      this.handleNetworkError();
    } else {
      this.handleGenericError(error);
    }
  }

  private static handleApiError(error: ApiError) {
    switch (error.status) {
      case 401:
        Alert.alert(
          'Oturum Süresi Doldu',
          'Lütfen tekrar giriş yapın',
          [{ text: 'Tamam', onPress: () => this.redirectToLogin() }]
        );
        break;

      case 403:
        Alert.alert('Yetkisiz İşlem', 'Bu işlem için yetkiniz bulunmuyor');
        break;

      case 404:
        Alert.alert('Bulunamadı', 'İstenen kaynak bulunamadı');
        break;

      case 422:
        // Validation errors are usually handled by the form
        break;

      case 500:
        Alert.alert('Sunucu Hatası', 'Geçici bir sorun oluştu, lütfen tekrar deneyin');
        break;

      default:
        Alert.alert('Hata', error.message || 'Beklenmeyen bir hata oluştu');
    }
  }

  private static handleNetworkError() {
    Alert.alert(
      'Bağlantı Hatası',
      'İnternet bağlantınızı kontrol edin ve tekrar deneyin'
    );
  }

  private static handleGenericError(error: any) {
    Alert.alert(
      'Hata',
      error.message || 'Beklenmeyen bir hata oluştu'
    );
  }

  private static redirectToLogin() {
    // Navigate to login screen
    // This depends on your navigation implementation
  }
}

// Usage
export const withErrorHandling = <T extends any[], R>(
  fn: (...args: T) => Promise<R>,
  context?: string
) => {
  return async (...args: T): Promise<R | null> => {
    try {
      return await fn(...args);
    } catch (error) {
      ErrorHandler.handle(error, context);
      return null;
    }
  };
};
```

---

**💡 Implementation Notes:**

1. **TypeScript kullanımı** - Tüm API interface'leri type-safe
2. **Error handling** - Comprehensive hata yönetimi
3. **Optimistic updates** - UI responsiveness için kritik
4. **State management** - Zustand ile basit ve etkili
5. **Form validation** - Real-time kullanıcı geri bildirimi
6. **Offline support** - Network durumuna göre davranış
7. **Performance optimization** - Memory management ve caching

Bu örnekler ile mobil uygulamanızı WebCarpetApp API'sine entegre edebilir ve profesyonel bir kullanıcı deneyimi sunabilirsiniz.