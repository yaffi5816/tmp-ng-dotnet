
CREATE DATABASE MyShopEfrat2026

CREATE TABLE Categories
(
CategoryID  smallint identity(100,5) primary key,
 
CategoryName varchar(100)  unique not null
)



CREATE TABLE Products
(
ProductsID  smallint identity(1,1) primary key,
CategoryID  smallint references Categories not null,
ProductsName varchar(100)  unique not null ,
ProductsDescreption varchar(200) ,
Price float not null check (price>0),
ImgUrl varchar(200)  unique
)



CREATE TABLE Users
(
UserID  smallint identity(1,1) primary key,
[Password] varchar(100) not null,
[UserName] varchar(100)  unique not null ,
FirstName varchar(100),
LastName varchar(100),
)

CREATE TABLE Orders
(
OrderID  smallint identity(100,5) primary key,
OrderDate date default getdate(),
OrderSum smallint  not null ,
UserID  smallint references Users not null
)




CREATE TABLE Orders_Items
(
OrderItemID  smallint identity(1,1) primary key,
ProductsID smallint references Products not null ,
OrderID  smallint references Orders not null,
Quantity smallint not null
)
