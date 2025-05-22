-- OrderDbInit.sql
-- SQL script to create tables and insert test data for Order Management project

-- Drop database if exists (optional)
-- DROP DATABASE IF EXISTS OrderDb;
-- GO

-- Create database
CREATE DATABASE OrderDb;
GO

USE OrderDb;
GO

-- Create Customers table
CREATE TABLE Customers (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Segment INT NOT NULL -- 0 = Regular, 1 = VIP, 2 = NewCustomer
);
GO

-- Create Orders table
CREATE TABLE Orders (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    CustomerId UNIQUEIDENTIFIER NOT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL,
    Status INT NOT NULL, -- 0 = Placed, 1 = Processing, 2 = Shipped, 3 = Delivered
    CreatedAt DATETIME2 NOT NULL,
    DeliveredAt DATETIME2 NULL,

    CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerId)
        REFERENCES Customers(Id)
);
GO

-- Insert Customers
INSERT INTO Customers (Id, Name, Segment)
VALUES
('33F25585-7C46-49CD-BBF4-348BD2F37DE5', 'Lucky Okorodiden', 1), -- VIP
('2903DB11-994C-4D9A-BC52-B2160753848D', 'Jane Doe', 2),         -- NewCustomer
('7C231B09-4371-4EB2-85DA-50B04A4BA33D', 'Regular Joe', 0);      -- Regular
GO

-- Insert Orders
INSERT INTO Orders (Id, CustomerId, TotalAmount, Status, CreatedAt, DeliveredAt)
VALUES
('621E0FFC-81F3-458D-8C0F-0AAAAA845823', '33F25585-7C46-49CD-BBF4-348BD2F37DE5', 1000.00, 3, '2025-05-17T19:52:08', '2025-05-22T19:52:08'),
('5A397DEF-932F-4F06-9B26-D77187361F0B', '2903DB11-994C-4D9A-BC52-B2160753848D', 500.00, 3, '2025-05-20T19:52:08', '2025-05-22T19:52:08'),
('BC499D01-B84F-44EA-981A-0439F5512896', '7C231B09-4371-4EB2-85DA-50B04A4BA33D', 200.00, 3, '2025-05-19T19:52:08', '2025-05-22T19:52:08'),
('6C234BE5-85A5-4F57-923A-1FBBFD589575', '33F25585-7C46-49CD-BBF4-348BD2F37DE5', 999.00, 0, GETUTCDATE(), NULL); -- For status update testing
GO
