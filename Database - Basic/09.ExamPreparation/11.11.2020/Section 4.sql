-- Section 5

--11. Create View
CREATE VIEW v_UserWithCountries 
AS
SELECT 
FirstName + ' ' + LastName AS CustomerName,
Age,
Gender,
Name
FROM Customers
JOIN Countries ON Countries.Id = Customers.CountryId

SELECT TOP 5 *
  FROM v_UserWithCountries
 ORDER BY Age

 --12. Trigger
 GO
 CREATE TRIGGER DeleteProducts
 ON Products
 INSTEAD OF DELETE
 AS BEGIN
 DELETE
	FROM Feedbacks
	WHERE ProductId IN 
	(SELECT P.Id FROM Products AS P
	JOIN deleted AS D
		ON P.Id = D.Id)

	DELETE FROM ProductsIngredients
	WHERE ProductId IN 
	(SELECT P.Id FROM Products AS P
	JOIN deleted AS D
		ON P.Id = D.Id)

	DELETE FROM Products
	WHERE Products.Id  IN 
	(SELECT P.Id FROM Products AS P
	JOIN deleted AS D
		ON P.Id = D.Id)
  END
