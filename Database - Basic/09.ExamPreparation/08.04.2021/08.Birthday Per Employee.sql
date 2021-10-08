SELECT 
Username,
C.Name AS CategoryName
FROM Reports AS R
JOIN Users AS U ON U.Id = R.UserId
JOIN Categories AS C ON C.Id = R.CategoryId
WHERE DAY(OpenDate) = DAY(Birthdate) AND MONTH(OpenDate) = MONTH(Birthdate)
ORDER BY Username ASC, CategoryName ASC