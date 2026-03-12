use onlinestore
drop table Users
CREATE TABLE Users
(
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(10) NOT NULL CHECK (Role IN ('User', 'Admin')),
    Phone NVARCHAR(15) NULL,
    Address NVARCHAR(255) NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);
INSERT INTO Users (Username, Email, PasswordHash, Role, Phone, Address) VALUES
('admin', 'admin@store.com', 'admin123', 'Admin', '03001234567', 'Admin Address'),
('user1', 'user1@email.com', 'user123', 'User', '03009876543', 'User Address');
select * from Users
drop table Users
--categories
CREATE TABLE Categories
(
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL UNIQUE
);
select * from Categories
CREATE TABLE Products
(
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryID INT NOT NULL,
    ProductName NVARCHAR(150) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL DEFAULT 0,
    IsActive BIT DEFAULT 1,
    FOREIGN KEY (CategoryID)
        REFERENCES Categories(CategoryID)
);
INSERT INTO Products (CategoryID, ProductName, Price, Stock) VALUES
CREATE TABLE Orders
(
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(10,2) NOT NULL,
    OrderStatus NVARCHAR(20)
        CHECK (OrderStatus IN ('Pending', 'Confirmed', 'Completed', 'Cancelled'))
        DEFAULT 'Pending',
    CustomerName NVARCHAR(100) NULL,
    Phone NVARCHAR(15) NULL,
    Address NVARCHAR(255) NULL,
    Email NVARCHAR(100) NULL,
    PaymentMethod NVARCHAR(30) NULL,
    FOREIGN KEY (UserID)
        REFERENCES Users(UserID)
);
-- Order Items table (Individual products in each order)
CREATE TABLE OrderItems
(
    OrderItemID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    ProductName NVARCHAR(150) NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Subtotal AS (Quantity * Price),
    FOREIGN KEY (OrderID) 
        REFERENCES Orders(OrderID) 
        ON DELETE CASCADE,
    FOREIGN KEY (ProductID) 
        REFERENCES Products(ProductID)
);
select * from Users
select * from Orders
INSERT INTO Products (CategoryID, ProductName, Price, Stock, IsActive) VALUES
(1, 'Apple (1kg)', 200.00, 50, 1),
(1, 'Banana (1 dozen)', 150.00, 30, 1),
(2, 'Potato (1kg)', 80.00, 100, 1),
(3, 'Milk (1 liter)', 180.00, 40, 1),
(4, 'Mineral Water (1.5L)', 100.00, 60, 1),
(5, 'Bread', 120.00, 25, 1);
select * from Products
USE onlinestore;

-- Feedback table banayein
CREATE TABLE Feedback
(
    FeedbackID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    Username NVARCHAR(100),
    ProductName NVARCHAR(150) NULL,
    Rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    Comment NVARCHAR(MAX) NULL,
    FeedbackDate DATETIME DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1,
    
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE,
    )
