-- Section 3

-- 05. EEE-Mails

SELECT 
FirstName,
LastName,
FORMAT(BirthDate, 'MM-dd-yyyy'),
C.Name,
Email
FROM Accounts AS A
JOIN Cities AS C ON C.Id = A.CityId
WHERE Email LIKE 'e%'
ORDER BY C.Name

-- 06.City Statistics

SELECT
C.Name,
COUNT(*) AS Hotels
FROM Cities AS C 
JOIN Hotels AS H ON H.CityId = C.Id
GROUP BY C.Name
ORDER BY Hotels DESC, C.NAME

--07. Longest and Shortest Trips
--SELECT 
--tabl.Id,
--tabl.FullName,
--Max(TripsDaysCollection),
--MIN(TripsDaysCollection)
--FROM
--(SELECT
--A.Id AS Id,
--A.FirstName + ' ' + A.LastName AS FullName,
----DATEDIFF(Day,T.ArrivalDate,T.ReturnDate) AS TripDay,
----DENSE_RANK() OVER (PARTITION BY A.Id Order by DATEDIFF(Day,T.ArrivalDate,T.ReturnDate)) AS TripsDaysCollection
--MIN() KEEP (DENSE_RANK FIRST ORDER BY DATEDIFF(Day,T.ArrivalDate,T.ReturnDate)) OVER (PARTITION BY deptno) AS lowest,
--FROM Trips AS T
--JOIN Rooms AS R ON R.Id = T.RoomId
--JOIN Hotels AS H ON H.Id = R.HotelId
--JOIN Cities AS C ON C.Id = H.CityId
--JOIN Accounts AS A ON A.CityId = C.Id) AS tabl

----MIN(sal) KEEP (DENSE_RANK FIRST ORDER BY sal) OVER (PARTITION BY deptno) AS lowest,

--08. Metropolis

SELECT TOP (10)
C.Id,
C.Name AS City,
C.CountryCode AS Country,
COUNT(A.Id) AS Accounts
FROM Cities AS C
JOIN Accounts AS A ON A.CityId = C.Id
GROUP BY C.Name,C.Id, C.CountryCode
ORDER BY Accounts DESC

--09. Romantic Gateway

SELECT 
A.Id,
A.Email,
C.Name AS City,
COUNT(*) AS Trips
FROM Accounts AS A
JOIN AccountsTrips AS AT ON AT.AccountId = A.Id
JOIN Trips AS T ON AT.TripId = T.Id
JOIN Rooms AS R ON R.Id = T.RoomId
JOIN Cities AS C ON C.Id = A.CityId
JOIN Hotels AS H ON H.Id = R.HotelId
WHERE A.CityId = H.CityId
GROUP BY A.Email,A.Id, C.Name
ORDER BY Trips DESC, A.Id 

--10. GRPR Violation
SELECT 
Trips.Id,
CONCAT(a.FirstName, ' ', COALESCE(a.MiddleName+ ' ',''), a.LastName) AS 'Full Name',
C.Name AS 'From',
Trips.Name AS 'To',
Trips.Duration
FROM
		(SELECT 
		A.Id AS AccountData,
		T.Id,
		C.Name,
				IIF
					(T.CancelDate IS NULL,
					CONCAT(DateDiff(day,T.ArrivalDate, T.ReturnDate),' days'),
					'Canceled') AS Duration
		FROM Trips AS T
		 JOIN Rooms AS R ON R.Id = T.RoomId
		 JOIN Hotels AS H ON H.Id = R.HotelId
		 JOIN Cities AS C ON C.Id = H.CityId
		 JOIN AccountsTrips AS AT ON AT.TripId = T.Id
		 JOIN Accounts AS A ON A.Id = AT.AccountId) AS Trips
INNER JOIN Accounts AS A ON A.Id = Trips.AccountData
JOIN Cities AS C ON A.CityId = C.Id
WHERE A.FirstName IS NOT NULL
ORDER BY [Full Name],Id
