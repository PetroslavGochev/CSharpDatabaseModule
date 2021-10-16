--Section 3

--05. CigarsByPrice
USE CigarShop
SELECT 
CigarName,
PriceForSingleCigar,
ImageURL
FROM Cigars
ORDER BY PriceForSingleCigar ASC, CigarName DESC

--06. CigarsByTaste

SELECT 
C.Id,
CigarName,
C.PriceForSingleCigar,
T.TasteType,
T.TasteStrength
FROM Cigars AS C
JOIN Tastes AS T ON T.Id = C.TastId
WHERE T.TasteType = 'Earthy' OR T.TasteType = 'Woody'
ORDER BY C.PriceForSingleCigar DESC

--07. Clients Without Cigars

SELECT 
C.Id,
CONCAT(C.FirstName, ' ', C.LastName) 
AS ClientName,
C.Email
FROM Clients AS C
FULL JOIN ClientsCigars AS CC ON C.Id = CC.ClientId
WHERE ClientId IS NULL
ORDER BY ClientName


--08. First 5 Cigars

SELECT TOP(5)
    C.CigarName,
    C.PriceForSingleCigar,
    C.ImageURL
    FROM Cigars AS C
    JOIN Sizes AS S ON S.Id = C.SizeId
    WHERE Length >= 12 AND (C.CigarName LIKE '%ci%' OR C.PriceForSingleCigar > 50) AND S.RingRange > 2.55
    ORDER BY C.CigarName ASC , C.PriceForSingleCigar DESC



--09. Clients With ZIP Codes

SELECT 
CONCAT(C.FirstName, ' ', C.LastName) AS FullName,
A.Country,
A.ZIP,
CONCAT('$', MAX(CI.PriceForSingleCigar)) AS CigarPrice
    FROM Clients AS C 
    JOIN Addresses AS A ON  A.Id = C.AddressId 
    JOIN ClientsCigars CC ON CC.ClientId = C.Id 
    JOIN Cigars CI ON CI.Id = CC.CigarId 
    WHERE A.ZIP NOT LIKE '%[^0-9]%'
    GROUP BY C.FirstName, C.LastName, A.Country, A.ZIP
  ORDER BY FullName

--10. Cigars By Size

SELECT 
C.LastName,
AVG(S.Length) AS CiagrLength, 
CEILING(AVG(S.RingRange)) AS CiagrRingRange
FROM Clients AS C
JOIN ClientsCigars AS CC ON CC.ClientId = C.Id
JOIN Cigars AS CI ON CI.Id = CC.CigarId
JOIN Sizes AS S ON CI.SizeId = S.Id
GROUP BY C.LastName
ORDER BY CiagrLength DESC
