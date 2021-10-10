SELECT 
M.[FirstName] + ' ' + M.[LastName] AS Mechanic,
J.Status,
J.IssueDate
FROM Mechanics AS M
JOIN Jobs AS J ON J.MechanicId = M.MechanicId 
ORDER BY M.MechanicId, J.IssueDate,J.JobId