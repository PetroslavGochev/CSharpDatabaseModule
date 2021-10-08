SELECT 
ISNULL(E.FirstName + ' ' + E.LastName, 'None') AS Employee,
ISNULL(D.Name,'None') AS Department,
ISNULL(C.Name, 'None') AS Category,
ISNULL(R.Description, 'None') AS Description,
ISNULL(FORMAT(R.OpenDate, 'dd.MM.yyyy'), 'None') AS OpenDate,
ISNULL(S.Label, 'None') AS Status,
ISNULL(U.Name, 'None') AS 'User'
FROM Reports AS r
             LEFT JOIN Employees E
                       ON E.Id = r.EmployeeId
             LEFT JOIN Departments D
                       ON E.DepartmentId = D.Id
             LEFT JOIN Categories C
                       ON C.Id = r.CategoryId
             LEFT JOIN Status S
                       ON S.Id = r.StatusId
             LEFT JOIN Users U
                       ON U.Id = r.UserId
ORDER BY E.FirstName DESC,
             E.LastName DESC,
             Department,
             Category,
             r.Description,
             r.OpenDate,
             S.Label,
             [USER]
