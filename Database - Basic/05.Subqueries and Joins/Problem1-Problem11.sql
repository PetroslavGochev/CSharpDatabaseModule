--01.Employees Address
SELECT TOP(5) [EmployeeID],
[JobTitle],
Employees.AddressID,
[AddressText]
FROM [Employees]
JOIN [Addresses] ON Employees.AddressID = Addresses.AddressID
ORDER BY [AddressID]

SELECT * FROM Addresses

--02.Addresses with Towns
SELECT TOP(50) [FirstName],
[LastName],
Towns.[Name] AS Town,
Addresses.AddressText
FROM Employees
JOIN [Addresses] ON Employees.AddressID = Addresses.AddressID
JOIN [Towns] ON Addresses.TownID = Towns.TownID
ORDER BY [FirstName],[LastName]

--03.Sales Employee
SELECT 
[EmployeeID],
[FirstName],
[LastName],
Departments.Name AS DepartmentName
FROM Employees
JOIN Departments ON Employees.DepartmentID = Departments.DepartmentID
WHERE Departments.Name = 'Sales'
ORDER BY [EmployeeID]


--04.Employee Departments
SELECT TOP (5)
[EmployeeID],
[FirstName],
[Salary],
Departments.Name AS [DepartmentName]
FROM [Employees]
JOIN Departments ON Employees.DepartmentID = Departments.DepartmentID
WHERE [Salary] > 15000
ORDER BY Departments.DepartmentID

SELECT * FROM EmployeesProjects
--05.Employees Without Project
SELECT TOP (3)
Employees.EmployeeID,
Employees.FirstName
FROM Employees
LEFT JOIN EmployeesProjects ON EmployeesProjects.EmployeeID = Employees.EmployeeID
WHERE EmployeesProjects.ProjectID IS NULL
ORDER BY EmployeeID

--06.Employees Hired After
SELECT 
E.FirstName,
E.LastName,
E.HireDate,
D.Name AS DeptName
FROM Employees AS E
JOIN Departments AS D ON D.DepartmentID = E.DepartmentID
WHERE HireDate > '1999-01-01' AND (D.Name = 'Sales' OR D.Name = 'Finance')
ORDER BY HireDate ASC

SELECT * FROM Projects
SELECT * FROM EmployeesProjects
--07.Employees with Project
SELECT TOP (5)
E.EmployeeID,
E.FirstName,
P.Name AS [ProjectName]
FROM Employees AS E
JOIN EmployeesProjects AS EP ON E.EmployeeID = EP.EmployeeID
JOIN Projects AS P ON  EP.ProjectID = P.ProjectID 
WHERE P.StartDate > '2002-08-13' AND P.EndDate IS NULL
ORDER BY E.EmployeeID

--08.Employee 24
SELECT
E.EmployeeID,
E.FirstName,
CASE
WHEN YEAR(P.StartDate) >= 2005 THEN NULL
ELSE P.Name
END AS [ProjectName]
FROM [Employees] AS E
JOIN EmployeesProjects AS EP ON EP.EmployeeID = E.EmployeeID
JOIN Projects AS P ON P.ProjectID = EP.ProjectID
WHERE E.EmployeeID = 24

--09.Employee Manager
SELECT 
E.EmployeeID,
E.FirstName,
E.ManagerID,
M.FirstName AS ManagerName
FROM Employees AS E
JOIN Employees AS M ON M.EmployeeID = E.ManagerID
WHERE E.ManagerID = 3 OR E.ManagerID = 7
ORDER BY E.EmployeeID

--10.Employee Summary
SELECT TOP (50)
E.EmployeeID,
CONCAT(E.FirstName,' ',E.LastName) AS EmployeeName,
CONCAT(M.FirstName,' ',M.LastName) AS ManagerName,
D.Name AS DepartmentName
FROM Employees AS E
JOIN Employees AS M ON E.ManagerID = M.EmployeeID
JOIN Departments AS D ON E.DepartmentID = D.DepartmentID
ORDER BY E.EmployeeID

--11.Min Average Salary

