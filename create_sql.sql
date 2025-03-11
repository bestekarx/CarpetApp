USE carpetapp;

-- Firm Tablosu (ABP'nin AbpTenants tablosu ile eşleşecek, bu nedenle kaldırıldı)
-- ABP'nin AbpTenants tablosu kullanılacak.

-- Vehicle Tablosu
CREATE TABLE vehicle (
                         id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                         tenant_id UNIQUEIDENTIFIER NOT NULL, -- ABP'nin tenant yapısı ile uyumlu
                         vehicle_name NVARCHAR(256),
                         plate_number NVARCHAR(128),
                         active BIT,
                         created_date DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Authority Tablosu (ABP'nin AbpRoles ve AbpPermissions yapısı ile eşleşecek, bu nedenle kaldırıldı)
-- ABP'nin AbpRoles ve AbpPermissions tabloları kullanılacak.

-- User Tablosu (ABP'nin AbpUsers tablosu ile eşleşecek, bu nedenle kaldırıldı)
-- ABP'nin AbpUsers tablosu kullanılacak.

-- MessageUser Tablosu
CREATE TABLE message_user (
                              id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                              tenant_id UNIQUEIDENTIFIER NOT NULL, -- ABP'nin tenant yapısı ile uyumlu
                              user_id UNIQUEIDENTIFIER NOT NULL, -- ABP'nin AbpUsers tablosu ile ilişkili
                              username NVARCHAR(128),
                              password NVARCHAR(512),
                              title NVARCHAR(128),
                              active BIT,
                              created_date DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- MessageSettings Tablosu
CREATE TABLE message_settings (
                                  id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                                  tenant_id UNIQUEIDENTIFIER NOT NULL, -- ABP'nin tenant yapısı ile uyumlu
                                  message_user_id INT FOREIGN KEY REFERENCES message_user(id),
                                  upon_receipt_message BIT,
                                  new_order_message BIT,
                                  when_delivered_message BIT,
                                  send_upon_receipt_message BIT,
                                  send_new_order_message BIT,
                                  send_when_delivered_message BIT
);

-- Company Tablosu
CREATE TABLE company (
                         id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                         tenant_id UNIQUEIDENTIFIER NOT NULL, -- ABP'nin tenant yapısı ile uyumlu
                         user_id UNIQUEIDENTIFIER NOT NULL, -- ABP'nin AbpUsers tablosu ile ilişkili
                         message_settings_id INT NOT NULL FOREIGN KEY REFERENCES message_settings(id),
                         name NVARCHAR(128) NOT NULL,
                         description NVARCHAR(128) NOT NULL,
                         color NVARCHAR(128) NOT NULL,
                         active BIT NOT NULL,
                         created_date DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Product Tablosu
CREATE TABLE product (
                         id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                         tenant_id UNIQUEIDENTIFIER NOT NULL, -- ABP'nin tenant yapısı ile uyumlu
                         user_id UNIQUEIDENTIFIER NOT NULL, -- ABP'nin AbpUsers tablosu ile ilişkili
                         price MONEY NOT NULL,
                         name NVARCHAR(256) NOT NULL,
                         type INT NOT NULL,
                         active BIT NOT NULL,
                         created_date DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Printer Tablosu
CREATE TABLE printer (
                         id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                         tenant_id UNIQUEIDENTIFIER NOT NULL, -- ABP'nin tenant yapısı ile uyumlu
                         name NVARCHAR(256),
                         mac_address NVARCHAR(256)
);

-- Area Tablosu
CREATE TABLE area (
                      id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                      tenant_id UNIQUEIDENTIFIER NOT NULL, -- ABP'nin tenant yapısı ile uyumlu
                      name NVARCHAR(128) NOT NULL,
                      active BIT NOT NULL,
                      created_date DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Customer Tablosu
CREATE TABLE customer (
                          id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                          tenant_id UNIQUEIDENTIFIER NOT NULL, -- ABP'nin tenant yapısı ile uyumlu
                          area_id INT NOT NULL FOREIGN KEY REFERENCES area(id),
                          company_id INT NOT NULL FOREIGN KEY REFERENCES company(id),
                          user_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES AbpUsers(Id), -- ABP'nin AbpUsers tablosu ile ilişkili
                          full_name NVARCHAR(128) NOT NULL,
                          phone VARCHAR(128),
                          country_code NVARCHAR(128),
                          gsm NVARCHAR(128),
                          address NVARCHAR(256) NOT NULL,
                          coordinate NVARCHAR(128),
                          balance MONEY,
                          active BIT NOT NULL,
                          company_permission BIT NOT NULL,
                          created_date DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Received Tablosu
CREATE TABLE received (
                          id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                          tenant_id UNIQUEIDENTIFIER NOT NULL, -- ABP'nin tenant yapısı ile uyumlu
                          vehicle_id INT FOREIGN KEY REFERENCES vehicle(id),
                          user_id UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES AbpUsers(Id), -- ABP'nin AbpUsers tablosu ile ilişkili
                          customer_id INT NOT NULL FOREIGN KEY REFERENCES customer(id),
                          status INT NOT NULL,
                          note NVARCHAR(256),
                          row_number INT,
                          active BIT,
                          purchase_date DATETIME NOT NULL,
                          received_date DATETIME NOT NULL,
                          updated_date DATETIME NOT NULL,
                          created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
                          updated_user_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES AbpUsers(Id) -- ABP'nin AbpUsers tablosu ile ilişkili
);

-- MessageLog Tablosu
CREATE TABLE message_log (
                             id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                             tenant_id UNIQUEIDENTIFIER NOT NULL, -- ABP'nin tenant yapısı ile uyumlu
                             user_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES AbpUsers(Id), -- ABP'nin AbpUsers tablosu ile ilişkili
                             received_id INT FOREIGN KEY REFERENCES received(id),
                             customer_id INT FOREIGN KEY REFERENCES customer(id),
                             company_id INT FOREIGN KEY REFERENCES company(id),
                             message_content NVARCHAR(256),
                             message_successfully_send BIT,
                             country_code NVARCHAR(256),
                             messaged_phone NVARCHAR(64),
                             customer_name NVARCHAR(128),
                             created_date DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- MessageTemplate Tablosu
CREATE TABLE message_template (
                                  id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                                  tenant_id UNIQUEIDENTIFIER NOT NULL, -- ABP'nin tenant yapısı ile uyumlu
                                  user_id UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES AbpUsers(Id), -- ABP'nin AbpUsers tablosu ile ilişkili
                                  message_title NVARCHAR(128) NOT NULL,
                                  message_content NVARCHAR(1024) NOT NULL,
                                  message_type INT NOT NULL,
                                  active BIT NOT NULL,
                                  created_date DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Settings Tablosu (ABP'nin AbpSettings tablosu ile eşleşecek, bu nedenle kaldırıldı)
-- ABP'nin AbpSettings tablosu kullanılacak.

-- Order Tablosu
CREATE TABLE "order" (
                         id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                         tenant_id UNIQUEIDENTIFIER NOT NULL, -- ABP'nin tenant yapısı ile uyumlu
                         user_id UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES AbpUsers(Id), -- ABP'nin AbpUsers tablosu ile ilişkili
                         received_id INT FOREIGN KEY REFERENCES received(id),
                         order_discount INT NOT NULL,
                         order_amount MONEY NOT NULL,
                         order_total_price MONEY NOT NULL,
                         order_status INT NOT NULL,
                         order_row_number INT,
                         active BIT NOT NULL,
                         calculated_used BIT,
                         created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
                         updated_date DATETIME,
                         updated_user_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES AbpUsers(Id) -- ABP'nin AbpUsers tablosu ile ilişkili
);

-- Invoice Tablosu
CREATE TABLE invoice (
                         id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                         tenant_id UNIQUEIDENTIFIER NOT NULL, -- ABP'nin tenant yapısı ile uyumlu
                         order_id INT FOREIGN KEY REFERENCES "order"(id),
                         user_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES AbpUsers(Id), -- ABP'nin AbpUsers tablosu ile ilişkili
                         customer_id INT FOREIGN KEY REFERENCES customer(id),
                         total_price MONEY NOT NULL,
                         paid_price MONEY,
                         payment_type INT,
                         invoice_note NVARCHAR(1024),
                         active BIT,
                         updated_date DATETIME,
                         created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
                         updated_user_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES AbpUsers(Id) -- ABP'nin AbpUsers tablosu ile ilişkili
);

-- OrderedProduct Tablosu
CREATE TABLE ordered_product (
                                 id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                                 tenant_id UNIQUEIDENTIFIER NOT NULL, -- ABP'nin tenant yapısı ile uyumlu
                                 order_id INT NOT NULL FOREIGN KEY REFERENCES "order"(id),
                                 product_name NVARCHAR(128) NOT NULL,
                                 product_price MONEY NOT NULL,
                                 number INT NOT NULL,
                                 square_meter INT NOT NULL
);

-- OrderImage Tablosu
CREATE TABLE order_image (
                             id INT PRIMARY KEY IDENTITY,
                             tenant_id UNIQUEIDENTIFIER NOT NULL, -- ABP'nin tenant yapısı ile uyumlu
                             order_id INT NOT NULL FOREIGN KEY REFERENCES "order"(id),
                             user_id UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES AbpUsers(Id), -- ABP'nin AbpUsers tablosu ile ilişkili
                             image_path NVARCHAR(MAX) NOT NULL,
                             created_date DATETIME NOT NULL
);