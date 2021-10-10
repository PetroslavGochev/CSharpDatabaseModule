SELECT
M.FirstName + ' ' + M.LastName AS Mechanic,
AVG(DATEDIFF(DAY, IssueDate, FinishDate))
FROM JOBS
JOIN Mechanics AS M ON M.MechanicId = Jobs.MechanicId
GROUP BY M.FirstName, M.LastName, M.MechanicId
ORDER BY M.MechanicId