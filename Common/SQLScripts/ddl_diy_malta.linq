<Query Kind="SQL">
  <Connection>
    <ID>a4f2ae37-5444-49d5-882b-7b263a8c63d4</ID>
    <Persist>true</Persist>
    <Server>As-KNATZ-US</Server>
    <Database>DIYMalta</Database>
  </Connection>
</Query>

print '*********START!*********'

-- DROP EXISTING FOREIGN KEYS

print 'DROP  EXISTING FOREIGN KEYS'
if exists (select * from sysobjects where name = 'Contacts')
    ALTER TABLE Contacts drop CONSTRAINT FK_Contacts_ContactTypes
if exists (select * from sysobjects where name = 'Addresses') 
    ALTER TABLE Addresses drop CONSTRAINT FK_Addresses_AddressTypes 
if exists (select * from sysobjects where name = 'Addresses') 
    ALTER TABLE Addresses drop CONSTRAINT FK_Addresses_Towns 
if exists (select * from sysobjects where name = 'Users') 
    ALTER TABLE Users drop constraint  FK_Users_Genders
if exists (select * from sysobjects where name = 'Users') 
    ALTER TABLE Users drop constraint  FK_Users_Contacts
if exists (select * from sysobjects where name = 'Users') 
    ALTER TABLE Users drop constraint  FK_Users_Addresses
if exists (select * from sysobjects where name = 'Users') 
    ALTER TABLE Users drop constraint FK_Users_UserTypes
if exists (select * from sysobjects where name = 'UserRoles') 
    ALTER TABLE UserRoles drop CONSTRAINT FK_UserRoles_Roles
if exists (select * from sysobjects where name = 'UserRoles') 
    ALTER TABLE UserRoles drop CONSTRAINT FK_UserRoles_Users
if exists (select * from sysobjects where name = 'Menus') 
    ALTER TABLE Menus drop CONSTRAINT FK_Menus_Menus
if exists (select * from sysobjects where name = 'MenuRoles') 
    ALTER TABLE MenuRoles drop CONSTRAINT FK_MenuRoles_Roles
if exists (select * from sysobjects where name = 'MenuRoles') 
    ALTER TABLE MenuRoles drop CONSTRAINT FK_MenuRoles_Menus
if exists (select * from sysobjects where name = 'Categorys') 
    ALTER TABLE Categorys drop CONSTRAINT FK_Categorys_Categorys
if exists (select * from sysobjects where name = 'Products') 
    ALTER TABLE Products drop CONSTRAINT FK_Products_Categorys
if exists (select * from sysobjects where name = 'OrderDetails') 
    ALTER TABLE OrderDetails drop CONSTRAINT FK_OrderDetails_Orders	
if exists (select * from sysobjects where name = 'OrderDetails') 
    ALTER TABLE OrderDetails drop CONSTRAINT FK_OrderDetails_Products	
if exists (select * from sysobjects where name = 'OrderDetails') 
    ALTER TABLE OrderDetails drop CONSTRAINT FK_OrderDetails_Users	
if exists (select * from sysobjects where name = 'ShoppingCarts') 
    ALTER TABLE ShoppingCarts drop CONSTRAINT FK_ShoppingCarts_Users
if exists (select * from sysobjects where name = 'ShoppingCarts') 
    ALTER TABLE ShoppingCarts drop CONSTRAINT FK_ShoppingCarts_Products	
if exists (select * from sysobjects where name = 'PriceTypes') 
    ALTER TABLE PriceTypes drop CONSTRAINT FK_PriceTypes_Products
if exists (select * from sysobjects where name = 'PriceTypes') 
    ALTER TABLE PriceTypes drop CONSTRAINT FK_PriceTypes_UserTypes	
if exists (select * from sysobjects where name = 'Orders') 
    ALTER TABLE Orders drop CONSTRAINT FK_Orders_OrderStates
if exists (select * from sysobjects where name = 'Products') 
    ALTER TABLE Products drop CONSTRAINT FK_Products_SpecialSales
go
--========================================================================================
--========================================================================================
--========================================================================================
--========================================================================================
-- DROP EXISTING TABLES

print 'DROP EXISTING TABLES'
--ACCOUNT TABLES
if exists (select * from sysobjects where name = 'Users') drop table Users
if exists (select * from sysobjects where name = 'UserTypes') drop table UserTypes
if exists (select * from sysobjects where name = 'Contacts') drop table Contacts
if exists (select * from sysobjects where name = 'ContactTypes') drop table ContactTypes
if exists (select * from sysobjects where name = 'Genders') drop table Genders
if exists (select * from sysobjects where name = 'Towns') drop table Towns
if exists (select * from sysobjects where name = 'Addresses') drop table Addresses
if exists (select * from sysobjects where name = 'AddressTypes') drop table AddressTypes
--SYSTEM TABLES
if exists (select * from sysobjects where name = 'Roles') drop table Roles
if exists (select * from sysobjects where name = 'UserRoles') drop table UserRoles
if exists (select * from sysobjects where name = 'Menus') drop table Menus
if exists (select * from sysobjects where name = 'MenuRoles') drop table MenuRoles
if exists (select * from sysobjects where name = 'Categorys') drop table Categorys
--COMMERCE TABLES
if exists (select * from sysobjects where name = 'Products') drop table Products 
if exists (select * from sysobjects where name = 'ProductCategorys') drop table ProductCategorys
if exists (select * from sysobjects where name = 'Orders') drop table Orders
if exists (select * from sysobjects where name = 'OrderStates') drop table OrderStates
if exists (select * from sysobjects where name = 'OrderDetails') drop table OrderDetails
if exists (select * from sysobjects where name = 'ShoppingCarts') drop table ShoppingCarts
if exists (select * from sysobjects where name = 'PriceTypes') drop table PriceTypes
if exists (select * from sysobjects where name = 'SpecialSales') drop table SpecialSales
--MISC
if exists (select * from sysobjects where name = 'Messages') drop table Messages
if exists (select * from sysobjects where name = 'Comments') drop table Comments

--========================================================================================
--========================================================================================
--========================================================================================
--========================================================================================
-- CREATE NEW TABLES
print 'CREATE TABLES'
CREATE TABLE UserTypes (
    ID int IDENTITY(1,1) NOT NULL primary key,
    Name nvarchar(30)  NOT NULL
)
  
CREATE TABLE   ContactTypes  (
    ID  int IDENTITY(1,1) NOT NULL primary key,
    Name  nvarchar(50)  NOT NULL
)
 
CREATE TABLE   Contacts  (
    ID  int IDENTITY(1,1) NOT NULL primary key,
    Email  nvarchar(150)  NOT NULL,
    Phone  int  NOT NULL,
    Mobile  int  NOT NULL,
    TypeID  int  NULL 
    CONSTRAINT FK_Contacts_ContactTypes FOREIGN KEY references ContactTypes (ID)
)

CREATE TABLE  Genders (
    ID int IDENTITY(1,1) NOT NULL primary key,
    Name nvarchar(6)  NOT NULL
) 

CREATE TABLE  Towns (
    ID int IDENTITY(1,1) NOT NULL primary key,
    Name nvarchar(50)  NOT NULL
) 

CREATE TABLE   AddressTypes  (
    ID  int IDENTITY(1,1) NOT NULL primary key,
    Name  nvarchar(50)  NOT NULL
)

CREATE TABLE Addresses (
    ID int IDENTITY(1,1) NOT NULL primary key,
    Residence nvarchar(50)  NOT NULL,
    Street  nvarchar(50)  NOT NULL,
    PostCode  nvarchar(50)  NULL,
    TownID  int  NULL CONSTRAINT FK_Addresses_Towns FOREIGN KEY references Towns (ID), 
    TypeID  int  NULL CONSTRAINT FK_Addresses_AddressTypes FOREIGN KEY references AddressTypes (ID)
)

CREATE TABLE Users (
    Username nvarchar(50)  NOT NULL primary key,
    Password nvarchar(MAX)  NOT NULL,
    FirstName nvarchar(30)  NOT NULL,
    MiddleInitial nvarchar(10)  NULL,
    LastName nvarchar(30)  NOT NULL,
    DateOfBirth datetime  NOT NULL,
    GenderID int  NULL CONSTRAINT FK_Users_Genders FOREIGN KEY references Genders (ID),
    ContactID int  NULL  CONSTRAINT FK_Users_Contacts FOREIGN KEY references Contacts (ID),
    AddressID int  NULL  CONSTRAINT FK_Users_Addresses FOREIGN KEY references Addresses (ID),
    TypeID  int   NULL  CONSTRAINT FK_Users_UserTypes FOREIGN KEY references UserTypes (ID),
    Blocked bit   NULL,
    NoOfAttempts int  NULL
)

CREATE TABLE Roles(
    ID int IDENTITY(1,1) NOT NULL primary key,
    Name nvarchar(20)  NOT NULL
)

CREATE TABLE UserRoles (
    RoleID int  NOT NULL CONSTRAINT FK_UserRoles_Roles FOREIGN KEY references Roles (ID),
    Username nvarchar(50) NOT NULL CONSTRAINT FK_UserRoles_Users FOREIGN KEY references Users (Username), 
     CONSTRAINT PK_User_Roles PRIMARY KEY (RoleID,Username)
)

CREATE TABLE  Menus (
    ID int  NOT NULL primary key,
    Name nvarchar(50)  NOT NULL,
    Url nvarchar(200)  NULL,
    ParentID int  NULL CONSTRAINT FK_Menus_Menus FOREIGN KEY references Menus (ID),    
    Description nvarchar(100)  NULL
)

CREATE TABLE MenuRoles (
    MenuID int NOT NULL CONSTRAINT FK_MenuRoles_Menus FOREIGN KEY references Menus (ID),
    RoleID int NOT NULL CONSTRAINT FK_MenuRoles_Roles FOREIGN KEY references Roles (ID),
    CONSTRAINT PK_Menu_Roles PRIMARY KEY (MenuID, RoleID)
)

CREATE TABLE Categorys (
    ID int NOT NULL primary key,
    Name nvarchar(50)  NOT NULL,
    Description nvarchar(500)  NOT NULL,
    ParentID int  NULL CONSTRAINT FK_Categorys_Categorys FOREIGN KEY references Categorys (ID)
)


CREATE TABLE SpecialSales  (
    ID  uniqueidentifier NOT NULL primary key,
	Name nvarchar(100),  
    DateStarted  datetime  NOT NULL,
    DateExpired datetime not null,
    Discount decimal(18,2)  NOT NULL
)

create table Products
(
	ID uniqueidentifier not null primary key,
    Name nvarchar(100)  NOT NULL,
	Description nvarchar(MAX) not null, 
    CategoryID int  NOT NULL CONSTRAINT FK_Products_Categorys FOREIGN KEY references Categorys (ID),
	SaleID uniqueidentifier  NULL CONSTRAINT FK_Products_SpecialSales FOREIGN KEY references SpecialSales (ID),
    VAT decimal(5,2)  NOT NULL,
    ImagePath nvarchar(300)  not NULL,
    Stock int  NOT NULL, 
    Active bit  NOT NULL
)

CREATE TABLE OrderStates (
    ID int IDENTITY(1,1) NOT NULL primary key,
    Name nvarchar(50)  NOT NULL
)
  
CREATE TABLE Orders (
    ID uniqueidentifier NOT NULL primary key,
    DatePlaced datetime  NOT NULL,
    Status int  NULL CONSTRAINT FK_Orders_OrderStates FOREIGN KEY references OrderStates (ID)
)

CREATE TABLE OrderDetails (   
    OrderID uniqueidentifier  NOT  NULL CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY references Orders (ID),
    ProductID uniqueidentifier  NOT  NULL CONSTRAINT FK_OrderDetails_Products FOREIGN KEY references Products (ID),
    Username nvarchar(50)  NOT  NULL  CONSTRAINT FK_OrderDetails_Users FOREIGN KEY references Users (Username),
    UnitPrice decimal(18,2)  NULL,
    Quantity int  NULL,
	  CONSTRAINT PK_OrderDetails PRIMARY KEY (OrderID, ProductID, Username)
)

CREATE TABLE ShoppingCarts (
    ID uniqueidentifier NOT NULL primary key,
    Username nvarchar(50) NOT   NULL  CONSTRAINT FK_ShoppingCarts_Users FOREIGN KEY references Users (Username),
    ProductID  uniqueidentifier  NOT  NULL CONSTRAINT FK_ShoppingCarts_Products FOREIGN KEY references Products (ID),
    Quantity int  NOT NULL   
)

CREATE TABLE PriceTypes (   
    ProductID uniqueidentifier  NOT  NULL CONSTRAINT FK_PriceTypes_Products FOREIGN KEY references Products (ID),
    UserTypeID  int  NOT   NULL  CONSTRAINT FK_PriceTypes_UserTypes FOREIGN KEY references UserTypes (ID),
    UnitPrice decimal(18,2)  NOT NULL,
     CONSTRAINT PK_Price_Types PRIMARY KEY (ProductID, UserTypeID)
)




--===================================================================
--WIP
--===================================================================
/* 
--  ProductID uniqueidentifier  NOT  NULL CONSTRAINT FK_SpecialSales_Products FOREIGN KEY references Products (ID),
CREATE TABLE   Comments  (
    ID  int IDENTITY(1,1) NOT NULL primary key,
    DatePosted  datetime  NOT NULL,
    Contents  nvarchar(max)  NOT NULL,
    Title  nvarchar(50)  NOT NULL
)

CREATE TABLE  Messages (
    Id int IDENTITY(1,1) NOT NULL primary key,
    Sender nvarchar(150)  NOT NULL,
    Email nvarchar(150)  NOT NULL,
    Subject nvarchar(150)  NOT NULL,
    Message nvarchar(2000)  NOT NULL
)
*/

 go 
--========================================================================================
--========================================================================================
--========================================================================================
--========================================================================================
-- INSERT VALUES
print 'INSERTING VALUES'
--GENDER
insert Genders values ('Female')
insert Genders values ('Male')
insert Genders values ('Other')
--TOWN
insert Towns values ('Paola')
insert Towns values ('Fgura')
insert Towns values ('Mosta')
insert Towns values ('Valletta')
insert Towns values ('Zejtun')
insert Towns values ('Birgu')
insert Towns values ('Qormi')
--ROLE
insert Roles values ('Administrator')
insert Roles values ('Customer')
insert Roles values ('Manager')
insert Roles values ('Guest')
--ADDRESS TYPE
insert AddressTypes values ('Residential')
insert AddressTypes values ('Commercial')
insert AddressTypes values ('Shipping')
insert AddressTypes values ('Billing')
--CONTACT TYPE
insert ContactTypes values ('Commercial')
insert ContactTypes values ('Business')
insert ContactTypes values ('Personal')
--USER TYPE
insert UserTypes values ('Customer')
insert UserTypes values ('Retailer')
insert UserTypes values ('Supplier')
insert UserTypes values ('Administrator')
--ORDER STATES
insert OrderStates values ('Delivered')
insert OrderStates values ('Picked up')
insert OrderStates values ('Awaiting Delivery')
insert OrderStates values ('Awaiting Pickup')
insert OrderStates values ('Cancelled')
insert OrderStates values ('Processing')
insert OrderStates values ('Returned')
--CATEGORY
insert into Categorys ( ID , Name, Description , ParentID ) 
values (1001,'Electrical','Electrical Supplies',null)
, (2001,'Power Tools','Electric Tools for DIY work',null)
, (3001,'Hand Tools','Scew drivers etc',null)
, (3003,'Screw Drivers','Sets and individual screwdrivers',3001)
, (3004,'Spanners','Sets and individual spanners',3001)
, (3005,'Files','Sets and individual files and sand paper',3001)
, (4001,'Scafolding','Scafolds, ladders etc',null)
, (5001,'Plumbing','Plumbing Supplies',null)
, (6001,'Consumables','Screws, zip ties, adhesives etc',null)
, (7001,'Construction','Building Materials',null)
, (8001,'Tiles','Floor, wall and roof tiling materials',null)
, (9001,'Security','Locks and other home security materials',null)
, (10001,'Timber','Wooden materials for furniture and more',null)
, (11001,'Paint','Paint, wallpaper and decorating',null)
--MENU
--general
insert Menus values (1000,'Home','/default.aspx',null,'Home page')
--administrator
insert Menus values (2001,'Dashboard','/members/administrators/index.aspx',null,'Control panel for administrators')
insert Menus values (2002,'Profile','/members/index.aspx',2001,'Profile page')
insert Menus values (2003,'Cart','/members/cart.aspx',2001,'Shopping cart page')
insert Menus values (2004,'Checkout','/members/checkout.aspx',2001,'Checkout page')
--member
insert Menus values (3001,'Settings','/members/index.aspx',null,'Control panel for regular members')
insert Menus values (3002,'My Cart','/members/cart.aspx',3001,'Shopping cart page')
insert Menus values (3003,'Checkout Now','/members/checkout.aspx',3001,'Checkout page')
--general
insert Menus values (4000,'About','/about.aspx',null,'About us')
insert Menus values (5000,'Contact','/contact.aspx',null,'Contact us')
--MENU ROLES
--admins
insert MenuRoles values (1000,1)
insert MenuRoles values (2001,1)
insert MenuRoles values (2002,1)
insert MenuRoles values (2003,1)
insert MenuRoles values (2004,1)
insert MenuRoles values (4000,1)
insert MenuRoles values (5000,1)
--customers
insert MenuRoles values (1000,2)
insert MenuRoles values (3001,2)
insert MenuRoles values (3002,2)
insert MenuRoles values (3003,2)
insert MenuRoles values (4000,2)
insert MenuRoles values (5000,2)
--guest
insert MenuRoles values (1000,4)
insert MenuRoles values (4000,4)
insert MenuRoles values (5000,4)
--USERS
insert Addresses values ('Flat 1 Block i Unit 3', 'New Government Flats', 'PLA1818',	1,	1)
insert Contacts values ('001knatz@gmail.com', 27661576,	79945622, 3)
insert Users values ('test0001','B1B3773A05C0ED0176787A4F1574FF0075F7521E','Nathan','Z','Khupe','1986-12-10 00:00:00.000',2	,1	,1	,1	,0	,0)
insert UserRoles values (1,'test0001')
insert UserRoles values (2,'test0001')

--========================================================================================
--========================================================================================
--========================================================================================
--========================================================================================
print '*********DONE!*********'