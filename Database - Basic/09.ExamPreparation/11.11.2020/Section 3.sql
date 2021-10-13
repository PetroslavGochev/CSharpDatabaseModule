--Section 3

--05.Products By Price

SELECT 
Name,
Price,
Description
FROM Products
ORDER BY Price DESC,NAME ASC

--06.Negative feedback

SELECT 
F.ProductId,
F.Rate,
F.Description,
F.CustomerId,
C.Age,
C.Gender
FROM Feedbacks AS F
JOIN Customers AS C ON C.Id = F.CustomerId
WHERE F.Rate < 5.0
ORDER BY F.ProductId DESC, F.Rate ASC


--07. Customers without feedback
SELECT 
CONCAT(c.FirstName, ' ', c.LastName),
C.PhoneNumber,
C.Gender
FROM Customers AS C
LEFT JOIN Feedbacks AS F ON C.Id = F.CustomerId
WHERE F.Id IS NULL
ORDER BY C.Id ASC


--08.Customer by criteria

SELECT 
CU.FirstName,
CU.Age,
CU.PhoneNumber
FROM Customers AS CU
JOIN Countries AS C ON C.Id = CU.CountryId 
WHERE (CU.Age >= 21 AND CU.FirstName LIKE '%an%' OR CU.PhoneNumber LIKE '%38' AND C.Name != 'Greece')
ORDER BY CU.FirstName ASC, CU.Age DESC

-- 09. Middle Range Distributors
SELECT * FROM 
(SELECT 
D.Name AS DistributorName,
I.Name AS IngredientName,
P.Name AS ProductName,
AVG(RATE) AS AverageRate
FROM Distributors AS D
JOIN Ingredients AS I ON I.DistributorId = D.Id
JOIN ProductsIngredients AS IP ON IP.IngredientId = I.Id
JOIN Products AS P ON P.Id = IP.ProductId
JOIN Feedbacks AS F ON F.ProductId = P.Id
GROUP BY I.Name, D.Name, P.Name) AS Test
WHERE Test.AverageRate >= 5 AND TEST.AverageRate <= 8
ORDER BY DistributorName,IngredientName, ProductName


-- 10. Country Representative
SELECT
	CountryName,
	DistributorName
	FROM (SELECT 
		c.Name AS CountryName,
		d.Name AS DistributorName,
		COUNT(i.Id) AS Count, 
		DENSE_RANK() OVER (PARTITION BY c.Name ORDER BY COUNT(i.Id) DESC) AS [Rank]
		FROM Countries AS c
		JOIN Distributors AS d ON c.Id = d.CountryId 
		LEFT JOIN Ingredients AS i ON d.Id = i.DistributorId
		GROUP BY c.Name, d.Name) as temp
	WHERE Rank = 1
	ORDER BY CountryName, DistributorName
