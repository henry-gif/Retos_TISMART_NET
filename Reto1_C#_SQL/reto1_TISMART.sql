-----------------------------------------------------------------------creacion de base de datos----------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------
create database BDSALES;
------------------------------------------------------------------------uso de la base de datos-------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------
use BDSALES;

-------------------------------------------------------------------------creacion de las tablas-----------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Crear tabla Customer
CREATE TABLE Customer (
    CustomerID INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL
);

-- Crear tabla Category
CREATE TABLE Category (
    CategoryID INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    CategoryDescription VARCHAR(100) NOT NULL
);

-- Crear tabla Product
CREATE TABLE Product (
    ProductID INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    ProductName VARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    CategoryIDFK INT NOT NULL,
    FOREIGN KEY (CategoryIDFK) REFERENCES Category(CategoryID)
);

-- Crear tabla Orders
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    CustomerFK INT NOT NULL,
    OrderD DATE NOT NULL,
    FOREIGN KEY (CustomerFK) REFERENCES Customer(CustomerID)
);

-- Crear tabla OrderDetail
CREATE TABLE OrderDetail (
    OrderIDFK INT NOT NULL,
    ProductIDFK INT NOT NULL,
    Quantity INT NOT NULL,
    PRIMARY KEY (OrderIDFK, ProductIDFK),
    FOREIGN KEY (OrderIDFK) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductIDFK) REFERENCES Product(ProductID)
);
-------------------------------------------------------------------------creacion de la vista general -----------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------
CREATE VIEW VistaGeneral AS
SELECT 
    c.CustomerId,
    c.FirstName,
    c.LastName,
    c.Email,
    o.OrderID,
    o.OrderD,
    p.ProductID,
    p.ProductName,
    p.Price,
    cat.CategoryId,
    cat.CategoryDescription,
    od.Quantity
FROM 
    Customer c
JOIN 
    Orders o ON c.CustomerId = o.CustomerFK
JOIN 
    OrderDetail od ON o.OrderID = od.OrderIdfk
JOIN 
    Product p ON od.ProductIDFK = p.ProductID
JOIN 
    Category cat ON p.CategoryIDFK = cat.CategoryId;
	go
-----------------------------------------------------------Creacion de procedure-------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE RegistrarDatos
    @FirstName VARCHAR(100),
    @LastName VARCHAR(100),
    @Email VARCHAR(255),
    @ProductName VARCHAR(100),
    @Price DECIMAL(10, 2),
    @Quantity INT,
    @CategoryDescription VARCHAR(100)
AS
BEGIN
    DECLARE @CustomerId INT;
    DECLARE @ProductId INT;
    DECLARE @OrderId INT;
	DECLARE  @CategoryId INT;
    BEGIN TRY
	BEGIN TRANSACTION;
        -- Insertar en la tabla Customer
        INSERT INTO Customer (FirstName, LastName, Email)
        VALUES (@FirstName, @LastName, @Email);
        SET @CustomerId = SCOPE_IDENTITY();

        -- Insertar en la tabla Category 
        INSERT INTO Category (CategoryDescription)
        VALUES (@CategoryDescription);
        SET @CategoryId = SCOPE_IDENTITY(); 

        -- Insertar en la tabla Product
        INSERT INTO Product (ProductName, Price, CategoryIDFK)
        VALUES (@ProductName, @Price, @CategoryId); 
        SET @ProductId = SCOPE_IDENTITY();

        -- Insertar en la tabla Orders
        INSERT INTO Orders (CustomerFK, OrderD)
        VALUES (@CustomerId, GETDATE());
        SET @OrderId = SCOPE_IDENTITY();

        -- Insertar en la tabla OrderDetail
        INSERT INTO OrderDetail (OrderIDFK, ProductIDFK, Quantity)
        VALUES (@OrderId, @ProductId, @Quantity);

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK; -- Deshacer la transacción en caso de error
        PRINT 'Error al registrar datos: ' + ERROR_MESSAGE();
    END CATCH;
END
GO

-----------------------------------------------------------REALIZAR REGISTROS-------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------
EXEC RegistrarDatos
    @FirstName = 'Joseoh',
    @LastName = 'Jostar',
    @Email = 'jojo@example.com',
    @ProductName = 'Laptop',
    @Price = 99.99,
    @Quantity = 5,
	@CategoryDescription='Electronica';
	
	EXEC RegistrarDatos
    @FirstName = 'Henry',
    @LastName = 'Lozada',
    @Email = 'hl@gmail.com',
    @ProductName = 'Tostador',
    @Price =49.99,
    @Quantity = 100,
	@CategoryDescription='Electronica';
