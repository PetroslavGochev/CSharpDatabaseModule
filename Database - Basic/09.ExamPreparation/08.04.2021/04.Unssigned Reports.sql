Use Service

SELECT 
[Description],
FORMAT([OpenDate],'dd-MM-yyy') AS [OpenDate]
FROM Reports AS R
WHERE [EmployeeId] IS NULL
ORDER BY R.[OpenDate] ASC, R.[Description] ASC