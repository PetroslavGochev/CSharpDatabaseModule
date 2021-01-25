-- 01.Find Names of All Employees by First Name
SELECT [FirstName],[LastName] FROM [Employees]
WHERE [FirstName] LIKE 'SA%'


--02.Find Names of All employees by Last Name 
SELECT [FirstName],[LastName] FROM [Employees]
WHERE [LastName] LIKE '%ei%'


--03.Find First Names of All Employees
SELECT [FirstName] FROM [Employees]
WHERE [DepartmentID] = 3 OR [DepartmentID] = 10 AND YEAR([HireDate]) <= 2005 AND YEAR([HireDate]) >= 1995  

--04.Find All Employees Except Engineers
SELECT [FirstName],[LastName] FROM [Employees]
WHERE [JobTitle] NOT LIKE '%engineer%'

--05.Find Towns with Name Length
SELECT [Name] FROM Towns
WHERE LEN([Name]) = 6 OR LEN([Name]) = 5
ORDER BY [Name] ASC


--06.Find Towns Starting With
SELECT [TownID],[Name] FROM Towns
WHERE [Name] LIKE 'M%'  OR [Name] LIKE 'K%' OR [Name] LIKE 'B%' OR  [Name] LIKE 'E%'  
ORDER BY [Name] ASC

--07.Find Towns Not Starting With
SELECT [TownID],[Name] FROM Towns
WHERE [Name] NOT LIKE 'R%'  AND [Name] NOT LIKE 'D%' AND[Name] NOT LIKE 'B%'  
ORDER BY [Name] ASC

--08.Create View Employees Hired After 2000 Year
CREATE VIEW V_EmployeesHiredAfter2000 AS
SELECT [FirstName],[LastName] FROM [Employees]
WHERE YEAR([HireDate]) > 2000 

--09.Length of Last Name
SELECT [FirstName],[LastName] FROM [Employees]
WHERE LEN([LastName]) =	5

--10.Rank Employees by Salary
SELECT [EmployeeID],
		[FirstName],
		[LastName],
		[Salary], 
		DENSE_RANK() OVER (
	PARTITION BY [Salary]
	ORDER BY [EmployeeID]) AS [Rank]
FROM [Employees]
WHERE [Salary] >= 10000  AND [Salary] <= 50000 
ORDER BY [Salary] DESC
 
--11.Find All Employees with Rank 2 *
SELECT * FROM (
SELECT [EmployeeID],
		[FirstName],
		[LastName],
		[Salary], 
		DENSE_RANK() OVER (
	PARTITION BY [Salary]
	ORDER BY [EmployeeID]) AS [Rank]
FROM [Employees]
WHERE [Salary] >= 10000  AND [Salary] <= 50000 
 ) t
WHERE [Rank] = 2 
ORDER BY [Salary] DESC