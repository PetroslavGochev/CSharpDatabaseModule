CREATE TABLE [People]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(200) NOT NULL,
	[Picture] VARBINARY (MAX),
	[Height] DECIMAL (5,2),
	[Weight] DECIMAL (5,2),
	[Gender] CHAR(1),
	[Birthdate] DATE NOT NULL,
	[Biography] NVARCHAR (MAX)
)

INSERT INTO People ([Name],[Picture],[Height],[Weight],[Gender],[Birthdate],[Biography])
VALUES
('Petroslav',NULL,177,80,'m','1990/10/10',Null),
('Petroslav',NULL,177,80,'m','1990/10/10',Null),
('Petroslav',NULL,177,80,'m','1990/10/10',Null),
('Petroslav',NULL,177,80,'m','1990/10/10',Null),
('Petroslav',NULL,177,80,'m','1990/10/10',Null)
