--12. Highest Peaks in Bulgaria
SELECT 
MC.CountryCode,
M.MountainRange,
P.PeakName,
P.Elevation
FROM Mountains AS M
JOIN Peaks AS P ON M.Id = P.MountainId
JOIN MountainsCountries AS MC ON MC.MountainId = M.Id
WHERE MC.CountryCode = 'BG' AND P.Elevation > 2835
ORDER BY P.Elevation DESC

--13.Count Mountain Ranges
SELECT 
MC.CountryCode,
COUNT(*)
FROM Mountains AS M
JOIN MountainsCountries AS MC ON MC.MountainId = M.Id
WHERE MC.CountryCode = 'RU' OR MC.CountryCode = 'BG' OR MC.CountryCode = 'US'
GROUP BY MC.CountryCode


--14.Countries with Rivers
SELECT TOP(5)
C.CountryName,
R.RiverName
FROM Rivers AS R
RIGHT JOIN CountriesRivers AS CR ON CR.RiverId = R.Id
RIGHT JOIN Countries AS C ON CR.CountryCode = C.CountryCode
WHERE C.ContinentCode = 'AF'
ORDER BY C.CountryName ASC

----15.Continents and Currencies
SELECT 
t.ContinentCode,
t.CurrencyCode,
t.CurrencyUsage
FROM
(
SELECT
ContinentCode,
CurrencyCode,
COUNT(*) AS CurrencyUsage,
DENSE_RANK() OVER (PARTITION BY ContinentCode Order by COUNT(*) DESC ) AS Ranked
FROM Countries
GROUP BY ContinentCode,CurrencyCode) AS t
WHERE t.Ranked = 1 AND T.CurrencyUsage != 1
ORDER BY t.ContinentCode,T.CurrencyCode

 


--16.Countries Without Any Mountains
SELECT 
COUNT(*) AS Count
FROM 
(
SELECT 
CountryName
FROM Countries AS C
LEFT JOIN MountainsCountries AS MC ON MC.CountryCode = C.CountryCode
WHERE MC.CountryCode IS NULL ) AS CountryWithoutMountains




--17.Highest Peak and Longest River By Country
SELECT TOP (5)
C.CountryName,
MAX(P.Elevation) AS HighestPeakElevation,
MAX(R.Length) AS LongestRiverLength
FROM Countries AS C
LEFT JOIN CountriesRivers AS CR ON CR.CountryCode = C.CountryCode
LEFT JOIN Rivers AS R ON R.Id = CR.RiverId
LEFT JOIN MountainsCountries AS MC ON MC.CountryCode = C.CountryCode
LEFT JOIN Mountains AS M ON M.Id = MC.MountainId
LEFT JOIN Peaks AS P ON P.MountainId = MC.MountainId
GROUP BY C.CountryName
ORDER BY HighestPeakElevation DESC,LongestRiverLength DESC,C.CountryName ASC


--18.Highest Peak Name and Elevation by Country
SELECT TOP(5)
CountryMountainsPeaks.Country,
ISNULL(CountryMountainsPeaks.[Highest Peak Name],'(no highest peak)'),
ISNULL(CountryMountainsPeaks.[Highest Peak Elevation],0),
ISNULL(CountryMountainsPeaks.Mountain,'(no mountain)')
FROM
	(SELECT 
		C.CountryName AS Country,
		P.PeakName AS [Highest Peak Name],
		P.Elevation AS [Highest Peak Elevation],
		M.MountainRange AS Mountain,
		DENSE_RANK() OVER (PARTITION BY C.CountryName ORDER BY P.Elevation DESC) AS Ranked
	FROM Countries AS C
		LEFT JOIN MountainsCountries AS  MC ON C.CountryCode = MC.CountryCode
		LEFT JOIN Mountains AS M ON M.Id = MC.MountainId
		LEFT JOIN Peaks AS P ON P.MountainId = MC.MountainId) AS CountryMountainsPeaks
WHERE CountryMountainsPeaks.Ranked = 1
ORDER BY CountryMountainsPeaks.Country,CountryMountainsPeaks.[Highest Peak Name]






