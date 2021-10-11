--05. Commits

SELECT 
Id,
Message,
RepositoryId,
ContributorId
FROM Commits
ORDER BY Id,Message,RepositoryId,ContributorId

--06. Front-End

SELECT 
Id,
Name,
Size
FROM Files
WHERE Size > 1000 AND Name LIKE '%html%'
ORDER BY Size DESC, Id DESC, Name

--07. Issue Assignment

SELECT
I.Id,
[Username] + ' : ' + I.[Title]
FROM Issues AS I
JOIN Users AS U ON U.Id = I.AssigneeId
ORDER BY I.Id DESC, I.AssigneeId ASC

--08. Single Files

SELECT
F.Id,
F.Name,
CAST(F.[Size] AS NVARCHAR) + 'KB' AS Size
FROM Files AS F
LEFT JOIN Files AS f1 ON F.Id = f1.ParentId
WHERE f1.ParentId IS NULL
ORDER BY Id,Name, Size DESC
	
-- 09. Commits in Repositories

SELECT TOP(5)
R.Id,
R.Name,
Count(*) AS Commits
FROM Repositories AS R
JOIN Commits AS C ON C.RepositoryId = R.Id
JOIN RepositoriesContributors AS RC ON RC.RepositoryId = R.Id
GROUP BY R.Name, R.Id
ORDER BY Commits DESC, R.Id, R.Name

--10. Average Size

SELECT 
U.Username,
AVG(Size) AS Size
FROM Users AS U
JOIN RepositoriesContributors AS RC ON RC.ContributorId = U.Id
JOIN Commits AS C ON C.ContributorId = RC.ContributorId
JOIN Files AS F ON F.CommitId = C.Id
GROUP BY U.Username
ORDER BY Size DESC, Username