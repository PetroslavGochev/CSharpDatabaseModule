--14.Games from 2011 and 2012 year
SELECT TOP(50) [Name],Format([Start],'yyyy-MM-dd') AS [Start] FROM [Games]
WHERE YEAR([Start]) >= 2011 AND YEAR([Start]) <= 2012
ORDER BY [Start],[Name]

--15.User Email Providers
SELECT [Username],SUBSTRING([Email], CHARINDEX('@',[Email]) + 1 ,LEN([Email]) - CHARINDEX('@',[Email]) + 1) AS [Email Provider]
FROM [Users]
ORDER BY [Email Provider],[Username]

--16.Get Users with IPAdress Like Pattern
SELECT [Username],[ipAddress] FROM [Users]
WHERE [ipAddress] LIKE '___.1_%._%.___'
ORDER BY [Username]

--17.Show All Games with Duration and Part of the Day
SELECT [Name],
	CASE 
	WHEN DATEPART(HOUR,[Start]) BETWEEN 0 AND 11 THEN 'Morning'
	WHEN DATEPART(HOUR,[Start]) BETWEEN 12 AND 17 THEN 'Afternoon'
	WHEN DATEPART(HOUR,[Start]) BETWEEN 18 AND 24 THEN 'Evening'
	END AS [Part of the Day],
	CASE
	WHEN [Duration] BETWEEN 0 AND 3 THEN 'Extra Short'
	WHEN [Duration] BETWEEN 4 AND 6 THEN 'Short'
	WHEN [Duration] >= 6 THEN 'Long'
	ELSE 'Extra Long'
	END AS [Duration]
FROM [Games]
ORDER BY [Name],[Duration],[Part of the Day]