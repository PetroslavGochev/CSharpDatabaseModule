SELECT 
E.[FirstName] + ' ' + E.[LastName] AS FullName,
COUNT(U.Name) AS UsersCount
FROM Employees AS E 
LEFT JOIN Reports AS R ON R.EmployeeId = E.Id
LEFT JOIN Users AS U ON U.Id = R.UserId
GROUP BY E.FirstName,E.LastName
ORDER BY UsersCount DESC, FullName ASC