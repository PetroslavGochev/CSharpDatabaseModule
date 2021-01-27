
--13.Departments Total Salaries
SELECT 
e.DepartmentID,
SUM(Salary) AS TotalSalary
FROM Employees AS e 
GROUP BY e.DepartmentID
ORDER BY E.DepartmentID

--14.Employees Minimum Salaries
SELECT
E.DepartmentID,
MIN(Salary) AS MinimumSalary
FROM Employees AS e
WHERE E.DepartmentID IN (2,5,7) AND HireDate > '2000-01-01'
GROUP BY E.DepartmentID

--15. Employees Average Salaries

--SELECT 
--E.EmployeeID
--FROM Employees AS E
--WHERE E.Salary > 30000 

--16.Employees Maximum Salaries
SELECT
E.DepartmentID,
MAX(Salary) AS MaxSalary
FROM Employees AS E
GROUP BY E.DepartmentID	
HAVING MAX(Salary) < 30000 OR MAX(Salary) > 70000

--17.Employees Count Salaries
SELECT 
COUNT(*) AS Count
FROM Employees
WHERE ManagerID IS NULL

--18.3rd Highest Salary



--19.Salary Challenge
SELECT TOP (10)
E.FirstName,
E.LastName,
E.DepartmentID
FROM Employees AS E
JOIN (SELECT 
E.DepartmentID,
AVG(Salary) AS AvarageDepartmentsSalary
FROM Employees AS E
GROUP BY E.DepartmentID) AS AverageSalary ON AverageSalary.DepartmentID = E.DepartmentID
WHERE E.Salary > AverageSalary.AvarageDepartmentsSalary
ORDER BY E.DepartmentID

