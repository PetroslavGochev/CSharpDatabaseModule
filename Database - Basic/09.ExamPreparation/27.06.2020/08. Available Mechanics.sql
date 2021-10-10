SELECT 
M.FirstName + ' ' + M.LastName AS Available
FROM Mechanics AS M 
LEFT JOIN Jobs AS J ON J.MechanicId = M.MechanicId
WHERE J.Status = 'Finished' OR J.JobId IS NULL
GROUP BY M.FirstName,M.LastName, M.MechanicId
ORDER BY M.MechanicId

SELECT m.FirstName + ' ' + m.LastName
    FROM Mechanics AS m
             LEFT JOIN Jobs J
                       ON m.MechanicId = J.MechanicId
    WHERE J.Status = 'Finished'
       OR J.JobId IS NULL
    ORDER BY m.MechanicId

SELECT * FROM Mechanics