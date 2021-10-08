SELECT TOP 5 
[Name] AS CategoryName,
COUNT([NAME]) AS ReportsNumber
FROM Reports AS R
JOIN Categories as C ON C.Id = R.CategoryId
GROUP BY C.Name
ORDER BY ReportsNumber DESC, CategoryName ASC
