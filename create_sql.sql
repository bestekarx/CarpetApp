USE carpetapp;

CREATE TABLE firm (
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	firm_code NVARCHAR(256),
	phone NVARCHAR(128),
	mail NVARCHAR(128) NOT NULL,
	owner NVARCHAR(128) NOT NULL,
	note NVARCHAR(512),
	active INT NOT NULL,
	membership_start_date DATETIME NOT NULL,
	membership_end_date DATETIME NOT NULL,
);

CREATE TABLE authority (
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	firm_id INT NOT NULL FOREIGN KEY REFERENCES firm(id),
	auth_direct_sale INT NOT NULL,
	auth_order INT NOT NULL,
	auth_customer_change INT NOT NULL,
	auth_expense_add INT NOT NULL,
	auth_report INT NOT NULL,
	auth_product INT NOT NULL,
	auth_vehicle INT NOT NULL,
	auth_area INT NOT NULL,
	auth_settings INT NOT NULL,
	auth_company INT NOT NULL,
	auth_collective_message INT NOT NULL,
	auth_message_user INT NOT NULL,
	auth_all_vehicle INT NOT NULL,
	auth_offline_mode INT
);

CREATE TABLE vehicle (
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	firm_id INT NOT NULL FOREIGN KEY REFERENCES firm(id),
	vehicle_name NVARCHAR(256),
	plate_number NVARCHAR(128),
	active INT,
	created_date DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "user" (
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	firm_id INT NOT NULL FOREIGN KEY REFERENCES firm(id),
	auth_id INT NOT NULL FOREIGN KEY REFERENCES authority(id),
	vehicle_id INT FOREIGN KEY REFERENCES vehicle(id),
	username NVARCHAR(128) NOT NULL,
	"password" NVARCHAR(1024) NOT NULL,
	full_name NVARCHAR(128) NOT NULL,
	active INT NOT NULL,
	"root" INT NOT NULL,
	print_normal INT,
	print_tag INT,
	onesignal_id NVARCHAR(256),
	is_notification INT,
	created_date DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE message_user (
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	user_id INT FOREIGN KEY REFERENCES "user", 
	firm_id INT NOT NULL FOREIGN KEY REFERENCES firm(id),
	username NVARCHAR(128),
	"password" NVARCHAR(512),
	title NVARCHAR(128),
	active INT,
	created_date DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE message_settings (
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	message_user_id INT FOREIGN KEY REFERENCES message_user(id),
	firm_id INT NOT NULL FOREIGN KEY REFERENCES firm(id),
	upon_receipt_message INT,
	new_order_message INT,
	when_delivered_message INT,
	send_upon_receipt_message INT,
	send_new_order_message INT,
	send_when_delivered_message INT
);

CREATE TABLE company (
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	firm_id INT NOT NULL FOREIGN KEY REFERENCES firm(id),
	user_id INT NOT NULL FOREIGN KEY REFERENCES "user"(id),
	message_settings_id INT NOT NULL FOREIGN KEY REFERENCES message_settings(id),
	"name" NVARCHAR(128) NOT NULL,
	"description" NVARCHAR(128) NOT NULL,
	color NVARCHAR(128) NOT NULL, 
	active INT NOT NULL,
	created_date DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE product (
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	firm_id INT NOT NULL FOREIGN KEY REFERENCES firm(id),
	user_id INT NOT NULL FOREIGN KEY REFERENCES "user"(id),
	price MONEY NOT NULL,
	"name" NVARCHAR(256) NOT NULL,
	"type" INT NOT NULL,
	active INT NOT NULL,
	created_date DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE printer (
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	firm_id INT NOT NULL FOREIGN KEY REFERENCES firm(id),
	"name" NVARCHAR(256),
	mac_address NVARCHAR(256)
);

CREATE TABLE area (
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	firm_id INT NOT NULL FOREIGN KEY REFERENCES firm(id),
	"name" NVARCHAR(128) NOT NULL,
	active INT NOT NULL,
	created_date DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE customer (
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	firm_id INT NOT NULL FOREIGN KEY REFERENCES firm(id),
	area_id INT NOT NULL FOREIGN KEY REFERENCES area(id),
	user_id INT FOREIGN KEY REFERENCES "user"(id),
	full_name NVARCHAR(128) NOT NULL,
	phone VARCHAR(128),
	country_code NVARCHAR(128),
	gsm NVARCHAR(128),
	"address" NVARCHAR(256) NOT NULL,
	coordinate NVARCHAR(128),
	balance MONEY,
	active INT NOT NULL,
	company_permission INT NOT NULL,
	created_date DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE received (
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	firm_id INT NOT NULL FOREIGN KEY REFERENCES firm(id),
	vehicle_id INT FOREIGN KEY REFERENCES vehicle(id),
	user_id INT NOT NULL FOREIGN KEY REFERENCES "user"(id),
	customer_id INT NOT NULL FOREIGN KEY REFERENCES customer(id),
	company_id INT FOREIGN KEY REFERENCES company(id),
	"status" INT NOT NULL,
	note NVARCHAR(256),
	"row_number" INT,
	active INT,
	purchase_date DATETIME NOT NULL,
	received_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	updated_user_id INT FOREIGN KEY REFERENCES "user"(id),
);

CREATE TABLE message_log (
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	firm_id INT NOT NULL FOREIGN KEY REFERENCES firm(id),
	user_id INT FOREIGN KEY REFERENCES "user"(id),
	received_id INT FOREIGN KEY REFERENCES received(id),
 	customer_id INT FOREIGN KEY REFERENCES customer(id),
	company_id INT FOREIGN KEY REFERENCES company(id),
	message_content NVARCHAR(256),
	message_successfully_send INT,
	country_code NVARCHAR(256),
	messaged_phone NVARCHAR(64),
	customer_name NVARCHAR(128),
	created_date DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE message_template (
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	firm_id INT NOT NULL FOREIGN KEY REFERENCES firm(id),
	user_id INT NOT NULL FOREIGN KEY REFERENCES "user"(id),
	message_title NVARCHAR(128) NOT NULL,
	message_content NVARCHAR(1024) NOT NULL,
	message_type INT NOT NULL,
	active INT NOT NULL,
	created_date DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE settings (
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	firm_id INT NOT NULL FOREIGN KEY REFERENCES firm(id),
	business_name NVARCHAR(128) NOT NULL,
	money_unit NVARCHAR(64),
	how_many_day_process INT,
	app_version NVARCHAR(64),
	max_firm INT
);

CREATE TABLE "order" (
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	firm_id INT NOT NULL FOREIGN KEY REFERENCES firm(id),
	user_id INT NOT NULL FOREIGN KEY REFERENCES "user"(id),
	received_id INT FOREIGN KEY REFERENCES received(id),
	order_discount INT NOT NULL,
	order_amount MONEY NOT NULL,
	order_total_price MONEY NOT NULL,
	order_type INT NOT NULL,
	order_status INT NOT NULL,
	order_row_number INT,
	active INT NOT NULL,
	calculated_used INT,
	created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	updated_date DATETIME,
	updated_user_id INT FOREIGN KEY REFERENCES "user"(id),
);

CREATE TABLE invoice (
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	order_id INT FOREIGN KEY REFERENCES "order"(id),
	user_id INT FOREIGN KEY REFERENCES "user"(id),
	firm_id INT FOREIGN KEY REFERENCES firm(id),
	customer_id INT FOREIGN KEY REFERENCES customer(id),
	total_price MONEY NOT NULL,
	paid_price MONEY,
	invoice_type INT,
	payment_type INT,
	invoice_note NVARCHAR(1024),
	active INT,
	updated_date DATETIME,
	created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	updated_user_id INT FOREIGN KEY REFERENCES "user"(id)
);

CREATE TABLE ordered_product (
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	firm_id INT NOT NULL FOREIGN KEY REFERENCES firm(id),
	order_id INT NOT NULL FOREIGN KEY REFERENCES "order"(id),
	product_name NVARCHAR(128) NOT NULL,
	product_price MONEY NOT NULL,
	number INT NOT NULL,
	square_meter INT NOT NULL
);

CREATE TABLE order_image (
    id INT PRIMARY KEY IDENTITY,
    firm_id INT NOT NULL,
    order_id INT NOT NULL,
    user_id INT NOT NULL,
    image_path NVARCHAR(MAX) NOT NULL,
    created_date DATETIME NOT NULL,
	FOREIGN KEY (firm_id) REFERENCES firm(id),
	FOREIGN KEY (order_id) REFERENCES "order"(id),
	FOREIGN KEY (user_id) REFERENCES "user"(id),
);
