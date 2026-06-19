IF OBJECT_ID(N'dbo.Product_information', N'U') IS NOT NULL DROP TABLE dbo.Product_information;
IF OBJECT_ID(N'dbo.Supplier', N'U') IS NOT NULL DROP TABLE dbo.Supplier;
IF OBJECT_ID(N'dbo.Type_of_subgenre', N'U') IS NOT NULL DROP TABLE dbo.Type_of_subgenre;
IF OBJECT_ID(N'dbo.Media_type', N'U') IS NOT NULL DROP TABLE dbo.Media_type;
IF OBJECT_ID(N'dbo.Type_of_genre', N'U') IS NOT NULL DROP TABLE dbo.Type_of_genre;
IF OBJECT_ID(N'dbo.Shops', N'U') IS NOT NULL DROP TABLE dbo.Shops;
GO

CREATE TABLE dbo.Media_type
(
    ID_C BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Media_type PRIMARY KEY,
    Carrier NVARCHAR(30) NOT NULL
);
GO

CREATE TABLE dbo.Type_of_genre
(
    ID_G BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Type_of_genre PRIMARY KEY,
    Genre NVARCHAR(30) NOT NULL
);
GO

CREATE TABLE dbo.Type_of_subgenre
(
    ID_Subgenre BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Type_of_subgenre PRIMARY KEY,
    ID_G BIGINT NOT NULL,
    Subgenre NVARCHAR(30) NOT NULL,
    CONSTRAINT FK_Subgenre_Genre FOREIGN KEY (ID_G) REFERENCES dbo.Type_of_genre(ID_G)
);
GO

CREATE TABLE dbo.Shops
(
    ID_M BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Shops PRIMARY KEY,
    Street NVARCHAR(50) NOT NULL,
    Number_of_people_per_day BIGINT NOT NULL
);
GO

CREATE TABLE dbo.Supplier
(
    ID_Supplier BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Supplier PRIMARY KEY,
    ID_M BIGINT NOT NULL,
    Company NVARCHAR(50) NOT NULL,
    Wholesale_price MONEY NOT NULL,
    CONSTRAINT FK_Supplier_Shops FOREIGN KEY (ID_M) REFERENCES dbo.Shops(ID_M),
    CONSTRAINT CK_Supplier_Wholesale CHECK (Wholesale_price >= 0)
);
GO

CREATE TABLE dbo.Product_information
(
    ID_I BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Product_information PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Publisher NVARCHAR(50) NOT NULL,
    ID_G BIGINT NOT NULL,
    ID_Subgenre BIGINT NOT NULL,
    ReleaseYear INT NOT NULL,
    ID_C BIGINT NOT NULL,
    ID_Supplier BIGINT NOT NULL,
    Price MONEY NOT NULL,
    CONSTRAINT FK_Product_Media FOREIGN KEY (ID_C) REFERENCES dbo.Media_type(ID_C),
    CONSTRAINT FK_Product_Genre FOREIGN KEY (ID_G) REFERENCES dbo.Type_of_genre(ID_G),
    CONSTRAINT FK_Product_Subgenre FOREIGN KEY (ID_Subgenre) REFERENCES dbo.Type_of_subgenre(ID_Subgenre),
    CONSTRAINT FK_Product_Supplier FOREIGN KEY (ID_Supplier) REFERENCES dbo.Supplier(ID_Supplier),
    CONSTRAINT CK_Product_ReleaseYear CHECK (ReleaseYear BETWEEN 1900 AND 2100),
    CONSTRAINT CK_Product_Price CHECK (Price >= 0)
);
GO

CREATE INDEX IX_Product_Name ON dbo.Product_information(Name);
CREATE INDEX IX_Product_Genre ON dbo.Product_information(ID_G, ID_Subgenre);
GO

INSERT INTO dbo.Media_type (Carrier) VALUES
(N'CD-диск'), (N'DVD-диск'), (N'Цифровой ключ'), (N'Винил');
GO

INSERT INTO dbo.Type_of_genre (Genre) VALUES
(N'Хип-хоп'), (N'Платформер'), (N'Рок'), (N'Ролевая'), (N'Кино');
GO

INSERT INTO dbo.Type_of_subgenre (ID_G, Subgenre) VALUES
(1, N'Поп-рэп'),
(2, N'Метроидвания'),
(3, N'Хард-рок'),
(3, N'Поп-рок'),
(4, N'Ролевой экшн');
GO

INSERT INTO dbo.Shops (Street, Number_of_people_per_day) VALUES
(N'ул. Лизюкова', 700),
(N'пл. Ленина', 1000),
(N'ул. Мира', 800);
GO

INSERT INTO dbo.Supplier (ID_M, Company, Wholesale_price) VALUES
(3, N'Оптимер', 1000),
(1, N'Оптовый рай', 250),
(2, N'ОптимаЛайн', 350),
(3, N'ГроссХаус', 600),
(1, N'Товарный Опт', 300);
GO

INSERT INTO dbo.Product_information (Name, Publisher, ID_G, ID_Subgenre, ReleaseYear, ID_C, ID_Supplier, Price) VALUES
(N'Stronger', N'Kanye West', 1, 1, 2007, 1, 5, 550),
(N'Hollow knight', N'Team cherry', 2, 2, 2017, 3, 2, 750),
(N'Smells Like Teen Spirit', N'Nirvana', 3, 3, 1991, 2, 3, 400),
(N'Bohemian Rhapsody', N'Queen', 3, 3, 1975, 1, 4, 750),
(N'Red Dead Redemption 2', N'Rockstar Games', 4, 5, 2018, 3, 1, 1500);
GO
