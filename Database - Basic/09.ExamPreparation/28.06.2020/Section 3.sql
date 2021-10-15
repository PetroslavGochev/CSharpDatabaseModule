--Section 3
USE ColonialJourney
--05. Select all military journeys

SELECT 
Id,
FORMAT(JourneyStart,'dd/MM/yyyy'),
FORMAT(JourneyEnd,'dd/MM/yyyy')
FROM Journeys AS J
WHERE J.Purpose = 'Military'
ORDER BY JourneyStart

--06. Select all pilots

SELECT
C.Id as id,
C.FirstName + ' ' + c.LastName AS full_name
FROM Colonists AS C
JOIN TravelCards AS TC ON TC.ColonistId = C.Id
WHERE TC.JobDuringJourney = 'Pilot'
ORDER BY Id

--07. Count colonists

SELECT COUNT(*) FROM Colonists AS C
JOIN TravelCards AS TC ON TC.ColonistId = C.Id
JOIN Journeys AS J ON J.Id = TC.JourneyId
WHERE J.Purpose = 'Technical'

--08.Select spaceships with pilots younger than 30 years

SELECT
S.Name,
S.Manufacturer
FROM Spaceships AS S
JOIN Journeys AS J ON J.SpaceshipId = S.Id
JOIN TravelCards AS TC ON TC.JourneyId = J.Id
JOIN Colonists AS C ON C.Id = TC .ColonistId
WHERE DATEDIFF(YEAR, C.BirthDate,'01/01/2019') <= 30 AND TC.JobDuringJourney = 'Pilot'
ORDER BY S.Name ASC

--09. Select all planets and their journey count

SELECT 
P.Name AS PlanetName,
Count(*) AS JournetsCount
FROM Planets AS P
JOIN Spaceports AS S ON S.PlanetId = P.Id
JOIN Journeys AS J ON J.DestinationSpaceportId = S.Id
GROUP BY P.Id, P.Name
Order by JournetsCount DESC, PlanetName  ASC

--10. Select Second Oldest Important Colonist
SELECT 
TEMP.Job,
TEMP.FullName,
TEMP.Rank
FROM
	(SELECT 
	C.FirstName + ' ' + C.LastName AS FullName,
	TC.JobDuringJourney AS Job,
	DENSE_RANK() OVER (PARTITION BY TC.JobDuringJourney ORDER BY C.Birthdate) AS [Rank]
	FROM Colonists AS C
	JOIN TravelCards AS TC ON TC.ColonistId = C.Id
	JOIN Journeys AS J ON J.Id = TC.JourneyId) AS TEMP
WHERE TEMP.Rank = 2