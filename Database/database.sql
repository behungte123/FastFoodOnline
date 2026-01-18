-- =============================================
-- Fast Food Online Database Schema
-- SQL Server Database Creation Script
-- =============================================

USE master;
GO

-- Drop database if exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'FastFoodOnlineDb')
BEGIN
    ALTER DATABASE FastFoodOnlineDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE FastFoodOnlineDb;
END
GO

-- Create database
CREATE DATABASE FastFoodOnlineDb;
GO

USE FastFoodOnlineDb;
GO

-- =============================================
-- TABLE: Roles
-- =============================================
CREATE TABLE Roles (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255) NULL
);
GO

-- =============================================
-- TABLE: Accounts
-- =============================================
CREATE TABLE Accounts (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(MAX) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(MAX) NULL,
    Address NVARCHAR(255) NULL,
    DateOfBirth DATE NULL,
    RoleId INT NOT NULL,
    CONSTRAINT FK_Accounts_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id) ON DELETE CASCADE
);
GO

-- =============================================
-- TABLE: FoodCategories
-- =============================================
CREATE TABLE FoodCategories (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255) NULL
);
GO

-- =============================================
-- TABLE: Foods
-- =============================================
CREATE TABLE Foods (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(150) NOT NULL,
    Description NVARCHAR(500) NULL,
    Price DECIMAL(18,2) NOT NULL,
    ImageUrl NVARCHAR(255) NULL,
    CategoryId INT NOT NULL,
    Tag NVARCHAR(200) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Foods_FoodCategories FOREIGN KEY (CategoryId) REFERENCES FoodCategories(Id) ON DELETE CASCADE
);
GO

-- =============================================
-- TABLE: Combos
-- =============================================
CREATE TABLE Combos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(150) NOT NULL,
    Description NVARCHAR(500) NULL,
    Price DECIMAL(18,2) NOT NULL,
    ImageUrl NVARCHAR(255) NULL,
    IsActive BIT NOT NULL DEFAULT 1
);
GO

-- =============================================
-- TABLE: ComboItems
-- =============================================
CREATE TABLE ComboItems (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ComboId INT NOT NULL,
    FoodId INT NOT NULL,
    Quantity INT NOT NULL,
    CONSTRAINT FK_ComboItems_Combos FOREIGN KEY (ComboId) REFERENCES Combos(Id) ON DELETE CASCADE,
    CONSTRAINT FK_ComboItems_Foods FOREIGN KEY (FoodId) REFERENCES Foods(Id) ON DELETE CASCADE
);
GO

-- =============================================
-- TABLE: Orders
-- =============================================
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    AccountId INT NOT NULL,
    OrderDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    DeliveryAddress NVARCHAR(255) NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT N'Chưa giao',
    TotalAmount DECIMAL(18,2) NOT NULL,
    Notes NVARCHAR(500) NULL,
    CONSTRAINT FK_Orders_Accounts FOREIGN KEY (AccountId) REFERENCES Accounts(Id) ON DELETE CASCADE
);
GO

-- =============================================
-- TABLE: OrderDetails
-- =============================================
CREATE TABLE OrderDetails (
    Id INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT NOT NULL,
    FoodId INT NULL,
    ComboId INT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
    CONSTRAINT FK_OrderDetails_Foods FOREIGN KEY (FoodId) REFERENCES Foods(Id),
    CONSTRAINT FK_OrderDetails_Combos FOREIGN KEY (ComboId) REFERENCES Combos(Id)
);
GO

-- =============================================
-- SEED DATA: Roles
-- =============================================
INSERT INTO Roles (Name, Description) VALUES
(N'Admin', N'Quản trị viên hệ thống'),
(N'Customer', N'Khách hàng');
GO

-- =============================================
-- SEED DATA: Accounts
-- Password: 123456 (plain text for demo - in production should be hashed)
-- =============================================
INSERT INTO Accounts (FullName, Email, Password, PhoneNumber, Address, DateOfBirth, RoleId) VALUES
(N'Quản trị viên', 'admin@fastfood.local', '123456', '0123456789', N'Hà Nội', '1990-01-01', 1),
(N'Khách hàng mẫu', 'customer@fastfood.local', '123456', '0987654321', N'TP Hồ Chí Minh', '1995-05-15', 2);
GO

-- =============================================
-- SEED DATA: FoodCategories
-- =============================================
INSERT INTO FoodCategories (Name, Description) VALUES
(N'Gà rán', N'Các món gà rán giòn ngon'),
(N'Burger', N'Bánh mì kẹp thịt đa dạng'),
(N'Đồ uống', N'Nước giải khát các loại'),
(N'Món phụ', N'Khoai tây chiên, salad...');
GO

-- =============================================
-- SEED DATA: Foods
-- =============================================
INSERT INTO Foods (Name, Description, Price, ImageUrl, CategoryId, Tag, IsActive) VALUES
-- Gà rán
(N'Gà rán giòn cay', N'Gà rán với lớp vỏ giòn rụm, cay nồng đặc trưng', 45000, 'https://images.unsplash.com/photo-1626082927389-6cd097cdc6ec?w=500', 1, N'spicy,chicken', 1),
(N'Gà rán truyền thống', N'Gà rán công thức truyền thống, thơm ngon đậm đà', 42000, 'https://images.unsplash.com/photo-1562967914-608f82629710?w=500', 1, N'chicken,classic', 1),
(N'Cánh gà BBQ', N'Cánh gà nướng sốt BBQ thơm lừng', 55000, 'https://images.unsplash.com/photo-1608039829572-78524f79c4c7?w=500', 1, N'chicken,bbq,wings', 1),
(N'Đùi gà giòn rụm', N'Đùi gà chiên giòn kích cỡ lớn', 48000, 'https://images.unsplash.com/photo-1594221708779-94832f4320d1?w=500', 1, N'chicken', 1),

-- Burger
(N'Burger gà giòn', N'Bánh mì kẹp gà giòn với rau củ tươi ngon', 39000, 'https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=500', 2, N'burger,chicken', 1),
(N'Burger bò phô mai', N'Bánh mì kẹp thịt bò xay với phô mai cheddar tan chảy', 52000, 'https://images.unsplash.com/photo-1550547660-d9450f859349?w=500', 2, N'burger,beef,cheese', 1),
(N'Burger cá', N'Bánh mì kẹp phi-lê cá chiên giòn', 45000, 'https://images.unsplash.com/photo-1520072959219-c595dc870360?w=500', 2, N'burger,fish', 1),

-- Đồ uống
(N'Pepsi', N'Nước ngọt Pepsi lon 330ml', 15000, 'https://images.unsplash.com/photo-1554866585-cd94860890b7?w=500', 3, N'drink,soda', 1),
(N'7Up', N'Nước ngọt 7Up lon 330ml', 15000, 'https://images.unsplash.com/photo-1625772452859-1c03d5bf1137?w=500', 3, N'drink,soda', 1),
(N'Trà đào', N'Trà đào cam sả mát lạnh', 25000, 'https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=500', 3, N'drink,tea', 1),
(N'Nước suối', N'Nước khoáng Aquafina 500ml', 10000, 'https://images.unsplash.com/photo-1559827260-dc66d52bef19?w=500', 3, N'drink,water', 1),

-- Món phụ
(N'Khoai tây chiên', N'Khoai tây lắc phô mai', 25000, 'https://images.unsplash.com/photo-1573080496219-bb080dd4f877?w=500', 4, N'fries,side', 1),
(N'Salad rau củ', N'Rau củ tươi ngon với sốt', 30000, 'https://images.unsplash.com/photo-1512621776951-a57141f2eefd?w=500', 4, N'salad,healthy,side', 1),
(N'Bánh ngọt', N'Bánh ngọt chiên giòn với đường', 20000, 'https://images.unsplash.com/photo-1558326567-98ae2405596b?w=500', 4, N'dessert,side', 1);
GO

-- =============================================
-- SEED DATA: Combos
-- =============================================
INSERT INTO Combos (Name, Description, Price, ImageUrl, IsActive) VALUES
(N'Combo Gà Rán Gia Đình', N'2 miếng gà + 1 khoai tây chiên lớn + 2 Pepsi', 120000, 'https://images.unsplash.com/photo-1513639776629-7b61b0ac49cb?w=500', 1),
(N'Combo Burger Đơn', N'1 Burger gà + 1 khoai tây + 1 Pepsi', 65000, 'https://images.unsplash.com/photo-1607013251379-e6eecfffe234?w=500', 1),
(N'Combo Tiết Kiệm', N'1 miếng gà + 1 khoai tây nhỏ + 1 nước suối', 55000, 'https://images.unsplash.com/photo-1606755962773-d324e0a13086?w=500', 1),
(N'Combo Đại Tiệc', N'4 miếng gà + 2 burger + 2 khoai tây lớn + 4 Pepsi', 250000, 'https://images.unsplash.com/photo-1555939594-58d7cb561ad1?w=500', 1);
GO

-- =============================================
-- SEED DATA: ComboItems
-- =============================================
-- Combo Gà Rán Gia Đình (Id=1)
INSERT INTO ComboItems (ComboId, FoodId, Quantity) VALUES
(1, 2, 2),  -- 2 Gà rán truyền thống
(1, 12, 1), -- 1 Khoai tây chiên
(1, 8, 2);  -- 2 Pepsi

-- Combo Burger Đơn (Id=2)
INSERT INTO ComboItems (ComboId, FoodId, Quantity) VALUES
(2, 5, 1),  -- 1 Burger gà giòn
(2, 12, 1), -- 1 Khoai tây chiên
(2, 8, 1);  -- 1 Pepsi

-- Combo Tiết Kiệm (Id=3)
INSERT INTO ComboItems (ComboId, FoodId, Quantity) VALUES
(3, 2, 1),  -- 1 Gà rán truyền thống
(3, 12, 1), -- 1 Khoai tây chiên
(3, 11, 1); -- 1 Nước suối

-- Combo Đại Tiệc (Id=4)
INSERT INTO ComboItems (ComboId, FoodId, Quantity) VALUES
(4, 2, 4),  -- 4 Gà rán truyền thống
(4, 5, 2),  -- 2 Burger gà giòn
(4, 12, 2), -- 2 Khoai tây chiên
(4, 8, 4);  -- 4 Pepsi
GO

-- =============================================
-- INDEXES for Performance
-- =============================================
CREATE INDEX IX_Accounts_Email ON Accounts(Email);
CREATE INDEX IX_Accounts_RoleId ON Accounts(RoleId);
CREATE INDEX IX_Foods_CategoryId ON Foods(CategoryId);
CREATE INDEX IX_Foods_IsActive ON Foods(IsActive);
CREATE INDEX IX_Combos_IsActive ON Combos(IsActive);
CREATE INDEX IX_Orders_AccountId ON Orders(AccountId);
CREATE INDEX IX_Orders_OrderDate ON Orders(OrderDate);
CREATE INDEX IX_OrderDetails_OrderId ON OrderDetails(OrderId);
CREATE INDEX IX_ComboItems_ComboId ON ComboItems(ComboId);
CREATE INDEX IX_ComboItems_FoodId ON ComboItems(FoodId);
GO

-- =============================================
-- VIEWS for Reporting
-- =============================================

-- View: Active Foods with Category
CREATE VIEW vw_ActiveFoods AS
SELECT
    f.Id,
    f.Name,
    f.Description,
    f.Price,
    f.ImageUrl,
    f.Tag,
    c.Name AS CategoryName
FROM Foods f
INNER JOIN FoodCategories c ON f.CategoryId = c.Id
WHERE f.IsActive = 1;
GO

-- View: Active Combos with Items
CREATE VIEW vw_ActiveCombos AS
SELECT
    c.Id,
    c.Name,
    c.Description,
    c.Price,
    c.ImageUrl,
    COUNT(ci.Id) AS ItemCount
FROM Combos c
LEFT JOIN ComboItems ci ON c.Id = ci.ComboId
WHERE c.IsActive = 1
GROUP BY c.Id, c.Name, c.Description, c.Price, c.ImageUrl;
GO

-- View: Order Summary
CREATE VIEW vw_OrderSummary AS
SELECT
    o.Id AS OrderId,
    a.FullName AS CustomerName,
    a.Email AS CustomerEmail,
    o.OrderDate,
    o.Status,
    o.TotalAmount,
    COUNT(od.Id) AS TotalItems
FROM Orders o
INNER JOIN Accounts a ON o.AccountId = a.Id
LEFT JOIN OrderDetails od ON o.Id = od.OrderId
GROUP BY o.Id, a.FullName, a.Email, o.OrderDate, o.Status, o.TotalAmount;
GO

-- =============================================
-- STORED PROCEDURES
-- =============================================

-- Procedure: Get Popular Foods (Top 5 most ordered)
CREATE PROCEDURE sp_GetPopularFoods
AS
BEGIN
    SELECT TOP 5
        f.Id,
        f.Name,
        f.Description,
        f.Price,
        f.ImageUrl,
        COUNT(od.Id) AS OrderCount
    FROM Foods f
    INNER JOIN OrderDetails od ON f.Id = od.FoodId
    WHERE f.IsActive = 1
    GROUP BY f.Id, f.Name, f.Description, f.Price, f.ImageUrl
    ORDER BY OrderCount DESC;
END
GO

-- Procedure: Get Customer Order History
CREATE PROCEDURE sp_GetCustomerOrders
    @AccountId INT
AS
BEGIN
    SELECT
        o.Id,
        o.OrderDate,
        o.DeliveryAddress,
        o.Status,
        o.TotalAmount,
        o.Notes
    FROM Orders o
    WHERE o.AccountId = @AccountId
    ORDER BY o.OrderDate DESC;
END
GO

-- Procedure: Calculate Revenue by Date Range
CREATE PROCEDURE sp_GetRevenue
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT
        CAST(OrderDate AS DATE) AS OrderDay,
        COUNT(Id) AS TotalOrders,
        SUM(TotalAmount) AS Revenue
    FROM Orders
    WHERE CAST(OrderDate AS DATE) BETWEEN @StartDate AND @EndDate
    AND Status NOT IN ('Cancelled', N'Đã hủy')
    GROUP BY CAST(OrderDate AS DATE)
    ORDER BY OrderDay;
END
GO

-- =============================================
-- COMPLETION MESSAGE
-- =============================================
PRINT '========================================';
PRINT 'Database FastFoodOnlineDb created successfully!';
PRINT '========================================';
PRINT '';
PRINT 'Tables created:';
PRINT '  - Roles';
PRINT '  - Accounts';
PRINT '  - FoodCategories';
PRINT '  - Foods';
PRINT '  - Combos';
PRINT '  - ComboItems';
PRINT '  - Orders';
PRINT '  - OrderDetails';
PRINT '';
PRINT 'Sample Data:';
PRINT '  - 2 Roles (Admin, Customer)';
PRINT '  - 2 Accounts (admin@fastfood.local / 123456, customer@fastfood.local / 123456)';
PRINT '  - 4 Food Categories';
PRINT '  - 14 Foods';
PRINT '  - 4 Combos with items';
PRINT '';
PRINT 'Views created:';
PRINT '  - vw_ActiveFoods';
PRINT '  - vw_ActiveCombos';
PRINT '  - vw_OrderSummary';
PRINT '';
PRINT 'Stored Procedures created:';
PRINT '  - sp_GetPopularFoods';
PRINT '  - sp_GetCustomerOrders';
PRINT '  - sp_GetRevenue';
PRINT '';
PRINT 'Database ready for use!';
GO
