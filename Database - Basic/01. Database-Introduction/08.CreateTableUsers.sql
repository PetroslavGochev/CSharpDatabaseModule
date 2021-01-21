--Using SQL query create table Users with columns:
--•	Id – unique number for every user. There will be no more than 263-1 users. (Auto incremented)
--•	Username – unique identifier of the user will be no more than 30 characters (non Unicode). (Required)
--•	Password – password will be no longer than 26 characters (non Unicode). (Required)
--•	ProfilePicture – image with size up to 900 KB. 
--•	LastLoginTime
--•	IsDeleted – shows if the user deleted his/her profile. Possible states are true or false.
--Make Id primary key. Populate the table with exactly 5 records. Submit your CREATE and INSERT statements as Run queries & check DB.
-- 08.Create Table Users
CREATE TABLE [Users]
(
	[Id] INT PRIMARY KEY IDENTITY,
	[Username] VARCHAR (30) UNIQUE NOT NULL,
	[Password] VARCHAR (26) NOT NULL,
	[ProfilePicture] VARBINARY,
	[LastLoginTime] DATETIME,
	[IsDeleted] BIT NOT NULL,
)
INSERT INTO [Users] ([Username],[Password],[ProfilePicture],[LastLoginTime],[IsDeleted])
VALUES 
('VASKO','mit',NULL,2020/03/03,0),
('Dimitar','petroslav',NULL,2020/03/03,0),
('Stanislav','petroslav',NULL,2020/03/03,0),
('Georgi','petroslav',NULL,2020/03/03,0),
('Vasilen','petroslav',NULL,2020/03/03,0)
 
 --09.Change Primary Key
 ALTER TABLE [Users]
 DROP CONSTRAINT[PK_Users]

 ALTER TABLE [Users]
 ADD CONSTRAINT PK_Users_CompositeIdUsername
 PRIMARY KEY([Id],[Username])

 --10.Add Check Constraint
 ALTER TABLE [Users]
 ADD CONSTRAINT CK_Users_PasswordLenght
 CHECK (LEN([Password]) >= 5)

 --11.Set Default Value of a Field
 ALTER TABLE [Users]
 ADD CONSTRAINT DF_Users_LastLoginDateNow
 DEFAULT GETDATE() FOR LastLoginTime

 --12.Set Unique Field
 ALTER TABLE [Users]
 DROP CONSTRAINT [PK_Users_CompositeIdUsername]

 ALTER TABLE [Users]
 ADD CONSTRAINT PK_Users_IdPrimaryKey
 PRIMARY KEY ([Id])

 ALTER TABLE [Users]
 ADD CONSTRAINT CT_Username_Lenght
 CHECK (LEN([Username]) >= 3)
 



