--Using SQL queries create Movies database with the following entities:
--Set most appropriate data types for each column.
--Set primary key to each table. Populate each table with exactly 5 records. 
--Make sure the columns that are present in 2 tables would be of the same data type. 
--Consider which fields are always required and which are optional. 
--Submit your CREATE TABLE and INSERT statements as Run queries & check DB.


--13.Movies Database
CREATE DATABASE [Movies]
USE [Movies]
--•	Directors (Id, DirectorName, Notes)
CREATE TABLE [Directors]
(
	[Id] INT PRIMARY KEY NOT NULL IDENTITY,
	[DirectorName] NVARCHAR(250) NOT NULL,
	[Notes] NVARCHAR(MAX)
)
--•	Genres (Id, GenreName, Notes)
CREATE TABLE [Genres]
(
	[Id] INT PRIMARY KEY NOT NULL IDENTITY,
	[GenreName] NVARCHAR(250) NOT NULL,
	[Notes] NVARCHAR(MAX)
)

--•	Categories (Id, CategoryName, Notes)
CREATE TABLE [Categories]
(
	[Id] INT PRIMARY KEY NOT NULL IDENTITY,
	[CategoryName] NVARCHAR(250) NOT NULL,
	[Notes] NVARCHAR(MAX)
)

--•	Movies (Id, Title, DirectorId, CopyrightYear, Length, GenreId, CategoryId, Rating, Notes)
CREATE TABLE [Movies]
(
	[Id] INT PRIMARY KEY NOT NULL IDENTITY,
	[Title] NVARCHAR(250) NOT NULL,
	[DirectorId] INT FOREIGN KEY REFERENCES Directors(Id),
	[CopyRightYear] DATE NOT NULL,
	[Lenght] TIME NOT NULL,
	[GenreId] INT FOREIGN KEY REFERENCES Genres(Id),
	[CategoryId] INT FOREIGN KEY REFERENCES Categories(Id),
	[Rating] DECIMAL (3,1) NOT NULL,
	[Notes] NVARCHAR(MAX)
)
INSERT INTO [Directors] ([DirectorName],[Notes])
VALUES
('Ivan',NULL),
('Miroslav',NULL),
('Petroslav',NULL),
('Stefan',NULL),
('Rosen',NULL)

INSERT INTO [Genres] ([GenreName],[Notes])
VALUES
('Comedy',NULL),
('Comedy',NULL),
('Drama',NULL),
('Scary',NULL),
('Thriller',NULL)


INSERT INTO Categories(CategoryName,Notes)
	VALUES
		('Best Picture', NULL),
		('Best Actor in a Leading Role', NULL),
		('Best Music', NULL),
		('Best Director', NULL),
		('Best Film Editing', NULL)


INSERT INTO Movies(Title, DirectorId,CopyrightYear,
[Lenght],GenreId, CategoryId, Rating, Notes)
	VALUES
		('The Shawshank Redemption',1,'1994','02:11',
		5,4,9.2,NULL),
		('The Godfather',2,'1972','02:55',
		1,5,9.2, NULL),
		('Schindlers List',3,'1993','03:15',
		3,1,8.9, NULL),
		('The Lord of the Rings',
		4,'2003','03:21',
		4,2,8.9, NULL),
		('Pulp Fiction',5,'1994','02:34',
		2,3,8.9, NULL)
