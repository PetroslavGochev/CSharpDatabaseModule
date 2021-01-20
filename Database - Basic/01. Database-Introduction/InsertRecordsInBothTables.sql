--Populate both tables with sample records given in the table below.
--Minions		Towns
--Id	Name	Age	TownId		Id	Name
--1	Kevin	22	1		1	Sofia
--2	Bob	15	3		2	Plovdiv
--3	Steward	NULL	2		3	Varna


CREATE TABLE Minions
(
  Id INT PRIMARY KEY,
  Name NVARCHAR(250),
  Age INT,
  TownId INT
)
CREATE TABLE Towns
(
  Id INT PRIMARY KEY,
  Name NVARCHAR(250),
)
ALTER TABLE Minions
ADD FOREIGN KEY (TownId) REFERENCES Towns

INSERT INTO Towns(Id,Name)
VALUES (1,'Sofia')
INSERT INTO Towns(Id,Name)
VALUES (2,'Plovdiv')
INSERT INTO Towns(Id,Name)
VALUES (3,'Varna')

INSERT INTO Minions(Id,Name,Age,TownId)
VALUES (1,'Kevin',22,1)
INSERT INTO Minions(Id,Name,Age,TownId)
VALUES (2,'Bob',15,3)
INSERT INTO Minions(Id,Name,Age,TownId)
VALUES (3,'Steward',NULL,2)

