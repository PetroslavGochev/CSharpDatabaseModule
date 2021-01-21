CREATE DATABASE Hotel

--•	Employees (Id, FirstName, LastName, Title, Notes)
CREATE TABLE Employees
(
	[Id] INT PRIMARY KEY IDENTITY,
	[FirstName] NVARCHAR (30) NOT NULL,
	[LastName] NVARCHAR (30) NOT NULL,
	[Title] NVARCHAR (30) NOT NULL,
	[Notes] NVARCHAR (200)
)
--•	Customers (AccountNumber, FirstName, LastName, PhoneNumber, EmergencyName, EmergencyNumber, Notes)
CREATE TABLE Customers
(
	[AccountNumber] INT PRIMARY KEY IDENTITY,
	[FirstName] NVARCHAR (30) NOT NULL,
	[LastName] NVARCHAR (30) NOT NULL,
	[PhoneNumber] NVARCHAR (10) NOT NULL,
	[EmergencyName] NVARCHAR (30) NOT NULL,
	[EmergencyNumber] NVARCHAR (10) NOT NULL,
	[Notes] NVARCHAR (200)
)
INSERT INTO [Customers] ([FirstName], [LastName], 
[PhoneNumber], [EmergencyName], [EmergencyNumber],
[Notes])
	VALUES
		('Petroslav','Gochev','8851264','1','123', NULL),
		('Petroslav','Gochev','8851264','1','123', NULL),
		('Petroslav','Gochev','8851264','1','123', NULL)

--•	RoomStatus (RoomStatus, Notes)
CREATE TABLE RoomStatus
(
	[RoomStatus] NVARCHAR(20) PRIMARY KEY,
	[Notes] NVARCHAR(200)
)
INSERT INTO [RoomStatus] ([RoomStatus], [Notes])
	VALUES
		('FREE AND CLEAN', NULL),
		('FREE, NOT CLEAN', NULL),
		('OCCUPIED', NULL)

--•	RoomTypes (RoomType, Notes)
CREATE TABLE RoomTypes
(
	[RoomType] NVARCHAR(20) PRIMARY KEY,
	[Notes] NVARCHAR(200)
)
INSERT INTO [RoomTypes] ([RoomType], [Notes])
	VALUES
		('FANCY',NULL),
		('FAMILLY', NULL),
		('SINGLE', NULL)
--•	BedTypes (BedType, Notes)
CREATE TABLE BedTypes
(
	[BedType] NVARCHAR(20) PRIMARY KEY,
	[Notes] NVARCHAR(200)
)
INSERT INTO [BedTypes] ([BedType], [Notes])
	VALUES
		('ONEPERSON', NULL),
		('TWOPERSONS', NULL),
		('KINGBED', NULL)


--•	Rooms (RoomNumber, RoomType, BedType, Rate, RoomStatus, Notes)
CREATE TABLE Rooms
(
	[RoomNumber] INT PRIMARY KEY IDENTITY,
	[RoomType] NVARCHAR (20) FOREIGN KEY REFERENCES RoomTypes(RoomType),
	[BedType] NVARCHAR (20) FOREIGN KEY REFERENCES BedTypes(BedType),
	[Rate] DECIMAL (3,1) NOT NULL,
	[RoomStatus] NVARCHAR (20) NOT NULL,
	[Notes] NVARCHAR(200)
)
INSERT INTO [Rooms] (RoomType, BedType,[Rate], [RoomStatus], [Notes])
	VALUES
		('FANCY','KINGBED', 85,'OCCUPIED',NULL),
		('FAMILLY','TWOPERSONS', 65,'FREE, NOT CLEAN',NULL),
		('SINGLE','ONEPERSON', 35,'FREE AND CLEAN',NULL)

--•	Payments (Id, EmployeeId, PaymentDate, AccountNumber, FirstDateOccupied, 
--LastDateOccupied, TotalDays, AmountCharged, 
--TaxRate, TaxAmount, PaymentTotal, Notes)

CREATE TABLE Payments
(
	[Id] INT PRIMARY KEY IDENTITY,
	[EmployeeId] INT FOREIGN KEY REFERENCES Employees(Id),
	[PaymentDate] DATETIME2 NOT NULL,
	[AccountNumber] INT FOREIGN KEY REFERENCES Customers(AccountNumber),
	[FirstDateOccupied] DATETIME2 NOT NULL,
	[LastDateOccupied] DATETIME2 NOT NULL,
	[TotalDays] AS DATEDIFF(DAY,FirstDateOccupied,LastDateOccupied),
	[AmountCharged] DECIMAL (7,2) NOT NULL,
	[TaxRate] DECIMAL (5,2) NOT NULL,
	[TaxAmount] AS AmountCharged*TaxRate,
	[PaymentTotal] DECIMAL (6,2) NOT NULL,
	[NOTES] NVARCHAR(200) NOT NULL,
)


INSERT INTO Payments ( PaymentDate, AccountNumber,
FirstDateOccupied, LastDateOccupied,
AmountCharged, TaxRate,
PaymentTotal, Notes)
	VALUES
		('2020-05-20','1','2020-05-20','2020-05-25',220,20,
		260,'ppp'),
		('2020-05-20','2','2020-05-20','2020-05-25',220,20,
		260,'ppp'),
		('2020-05-20','3','2020-05-20','2020-05-25',220,20,
		260,'ppp') 

--•	Occupancies (Id, EmployeeId, DateOccupied, AccountNumber, RoomNumber, RateApplied, PhoneCharge, Notes)
CREATE TABLE Occupancies
(
	[Id] INT PRIMARY KEY IDENTITY,
	[EmployeeId] INT FOREIGN KEY REFERENCES Employees(Id),
	[DateOccupied] DATETIME2 NOT NULL,
	[AccountNumber] INT FOREIGN KEY REFERENCES Customers(AccountNumber),
	[RoomNumber] INT FOREIGN KEY REFERENCES Rooms(RoomNumber),
	[RateApplied] DECIMAL(4,2), 
	[PhoneCharge] DECIMAL(5,2),
	[Notes] NVARCHAR(200)

)

INSERT INTO Occupancies ( DateOccupied,
AccountNumber, RoomNumber, RateApplied, PhoneCharge,
Notes)
	VALUES
		('2020-05-20',1,1,25.5, 30, NULL),
		('2020-05-20',2,2,25.5, 30, NULL),
		('2020-05-20',3,3,25.5, 30, NULL)

INSERT INTO [Employees] ([FirstName], [LastName], [Title], [Notes])
	VALUES
		('IVAN', 'KOSEV', 'RECEPTIONIST',NULL),
		('VANYA', 'STAMATOVA', 'MANAGER',NULL),
		('SANDRA','MAKSIMOVA', 'MAID', NULL)




