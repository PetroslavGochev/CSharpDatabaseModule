CREATE TABLE Manufacturers
(
	[ManufacturerID] INT PRIMARY KEY IDENTITY(1,1),
	[Name] NVARCHAR (30) NOT NULL,
	[EstablishedOn] DATETIME2 NOT NULL,
)

CREATE TABLE Models
(
	[ModelID] INT PRIMARY KEY IDENTITY (101,1),
	[Name] NVARCHAR (30) NOT NULL,
	[ManufacturerID] INT FOREIGN KEY REFERENCES Manufacturers(ManufacturerID) NOT NULL
)

INSERT INTO Manufacturers ([Name],[EstablishedOn])
VALUES 
('BMW','1916/03/07'),
('Tesla','2003/01/01'),
('Lada','1966/05/01')

INSERT INTO Models ([Name],[ManufacturerID])
VALUES 
('X1',1),
('i6',1),
('Model S',2),
('Model X',2),
('Model 3',2),
('Nova',3)