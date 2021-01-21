CREATE DATABASE CarRental

--•	Categories (Id, CategoryName, DailyRate, WeeklyRate, MonthlyRate, WeekendRate)
CREATE TABLE Categories
(
	[Id] INT PRIMARY KEY IDENTITY,
	[CategoryName] NVARCHAR(30) NOT NULL,
	[DailyRate] DECIMAL (2,1) NOT NULL,
	[WeeklyRate] DECIMAL (2,1) NOT NULL,
	[MonthlyRate] DECIMAL (2,1) NOT NULL,
	[WeekendRate] DECIMAL (2,1) NOT NULL,
)

--(Id, PlateNumber, Manufacturer, Model, CarYear, CategoryId, Doors, Picture, Condition, Available)
CREATE TABLE Cars
(
	[Id] INT PRIMARY KEY IDENTITY,
	[PlateNumber] NVARCHAR(8) NOT NULL,
	[Manufacturer] NVARCHAR (20) NOT NULL,
	[Model] NVARCHAR (20) NOT NULL,
	[CarYear] DATETIME NOT NULL,
	[CategoryId] INT FOREIGN KEY REFERENCES Categories(Id),
	[Doors] INT NOT NULL,
	[Picture] VARBINARY (MAX),
	[Condition] NVARCHAR (20),
	[Avilable] BIT NOT NULL,
)


--•	Employees (Id, FirstName, LastName, Title, Notes)
CREATE TABLE [Employees]
(
	[Id] INT PRIMARY KEY IDENTITY,
	[FirstName] NVARCHAR (30) NOT NULL,
	[LastName] NVARCHAR (30) NOT NULL,
	[Title] NVARCHAR (30) NOT NULL,
	[Notes] NVARCHAR(MAX)
)


--•	Customers (Id, DriverLicenceNumber, FullName, Address, City, ZIPCode, Notes)
CREATE TABLE [Customers]
(
	[Id] INT PRIMARY KEY IDENTITY,
	[DriveLicenceNumber] NVARCHAR (10) NOT NULL,
	[FullName] NVARCHAR (40) NOT NULL,
	[Address] NVARCHAR (30),
	[City] NVARCHAR (30) NOT NULL,
	[ZIPCode] NVARCHAR (30) NOT NULL,
	[Notes] NVARCHAR(200)
)

--•	RentalOrders (Id, EmployeeId, CustomerId, CarId, TankLevel,
--KilometrageStart, KilometrageEnd, TotalKilometrage, StartDate, EndDate, TotalDays, RateApplied, 
--TaxRate, OrderStatus, Notes)
CREATE TABLE [RentalOrders]
(
	[Id] INT PRIMARY KEY IDENTITY,
	[EmployeeId] INT FOREIGN KEY REFERENCES Employees(Id),
	[CustomerId] INT FOREIGN KEY REFERENCES Customers(Id),
	[CarId] INT FOREIGN KEY REFERENCES Cars(Id),
	[TankLevel] INT NOT NULL,
	[KilometrageStart] INT NOT NULL,
	[KilometrageEnd] INT NOT NULL,
	[TotalKilometrage] AS [KilometrageStart] - [KilometrageEnd],
	[StartDate] DATETIME NOT NULL,
	[EndDate] DATETIME NOT NULL,
	[TotalDate] AS DATEDIFF(Day,[StartDate],[EndDate]),
	[RateApplied] DECIMAL (5,2) NOT NULL,
	[TaxRate] DECIMAL (7,2) NOT NULL,
	[OrderStatus] NVARCHAR (30) NOT NULL,
	[Notes] NVARCHAR(200)
)

	
INSERT INTO [Categories]([CategoryName],[DailyRate],[WeeklyRate],[MonthlyRate],[WeekendRate])
VALUES 
('Sport', 8.9 , 9 , 6.7, 8.9),
('Suv', 9.9 , 7.9 , 7.9, 9.5),
('Sedan', 8 , 8 , 9.9, 7.9)

INSERT INTO [Cars]([PlateNumber],[Manufacturer],[Model],[CarYear]
,CategoryId,[Doors],[Picture],[Condition],[Avilable])
VALUES 
('A7473NV', 'Peugeot' , '508' , '2011', 1 , 5, NULL, 'Yes',1),
('V8384NW', 'Mercedes-Benz', 'S-Class' , '2012', 2, 4, NULL, 'Yes',1),
('A7878', 'Opel' ,'Astra' , '2015', 1 , 4, NULL, NULL , 0)	

INSERT INTO [Employees]([FirstName],[LastName],[Title],[Notes])
VALUES 
('Petroslav', 'Gochev' , 'Front-Office' , NULL),
('Stanislav', 'Dimitrov', 'Managment' , NULL),
('Milko', 'Todorov' ,'Director' , NULL)	

INSERT INTO [Customers]([DriveLicenceNumber],[FullName],[Address],[City],[ZIPCode],[Notes])
VALUES 
('License1', 'Petroslav Gochev' , NULL , 'Bourgas','8000',NULL),
('License2', 'Ivan Dimitrov', NULL , 'Varna','8000',NULL),
('License3', 'Stefan Todorov' ,NULL , 'Sofia','8000',NULL)	


INSERT INTO [RentalOrders](EmployeeId,CustomerId,[CarId],[TankLevel]
,[KilometrageStart],[KilometrageEnd],[StartDate],[EndDate],[RateApplied],[TaxRate],
[OrderStatus],[Notes])
VALUES 
		(1,1,1,5,20,40,'2020-05-16','2020-05-20',
		65.00,65.00,'COMPLETE', NULL),
		(2,2,2,5,20,50,'2020-05-16','2020-05-20',
		65.00,65.00,'COMPLETE', NULL),
		(3,3,3,5,30,100,'2020-05-16','2020-05-20',
		65.00,65.00,'COMPLETE', NULL)