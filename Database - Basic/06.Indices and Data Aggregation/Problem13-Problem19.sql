
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

SELECT 
DepartmentID,
EmployeeID,
ManagerID,
Salary
INTO EmployeeWithSalaryOver30000
FROM Employees
WHERE Salary > 30000

DELETE FROM EmployeeWithSalaryOver30000
WHERE ManagerID = 42

UPDATE EmployeeWithSalaryOver30000
SET Salary = Salary + 5000
WHERE DepartmentID = 1

SELECT 
E.DepartmentID,
AVG(E.Salary)
FROM EmployeeWithSalaryOver30000 AS E
GROUP BY E.DepartmentID



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
SELECT 
T.DepartmentID,
T.Salary AS ThirdHighestSalary
FROM
(SELECT 
DepartmentID,
Salary,
DENSE_RANK() OVER (PARTITION BY DepartmentID ORDER BY Salary DESC) as Ranked
FROM Employees
GROUP BY DepartmentID,Salary) AS T
WHERE T.Ranked = 3



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

