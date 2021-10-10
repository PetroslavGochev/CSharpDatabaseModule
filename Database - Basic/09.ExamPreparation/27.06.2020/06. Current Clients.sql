SELECT 
C.[FirstName] + ' ' + C.[LastName] AS Client,
DATEDIFF(DAY, J.IssueDate,'2017-04-24') AS 'Days going',
J.Status
FROM Jobs AS J
JOIN Clients AS C ON C.ClientId = J.ClientId
WHERE STATUS != 'Finished'
ORDER BY [Days going] DESC , C.ClientId ASC