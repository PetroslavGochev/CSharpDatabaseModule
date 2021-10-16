--Section 4

USE CigarShop
--11.. Clients With Cigars
GO

CREATE FUNCTION udf_ClientWithCigars(@name NVARCHAR(150))
RETURNS INT
AS
BEGIN

	DECLARE @numberOfCigars INT = (SELECT COUNT(*) FROM Clients AS C 
	JOIN ClientsCigars AS CC ON CC.ClientId = C.Id
	WHERE C.FirstName LIKE @name
	)

RETURN @numberOfCigars
END


SELECT dbo.udf_ClientWithCigars('Betty')

--12. Search For Cigar With Specific Taste

GO 

CREATE PROCEDURE usp_SearchByTaste(@taste NVARCHAR(100))
AS 
BEGIN
	SELECT 
	C.CigarName,
	CONCAT('$',C.PriceForSingleCigar) AS Price,
	T.TasteType,
	B.BrandName,
	CONCAT(S.Length, ' cm') AS CigarLength,
	CONCAT(S.RingRange,' cm') AS CigarRingRange
	FROM Cigars AS C
	JOIN Tastes AS T ON T.Id = C.TastId
	JOIN Brands AS B ON B.Id = C.BrandId
	JOIN Sizes AS S ON S.Id = C.SizeId
	WHERE T.TasteType Like @taste
	ORDER BY CigarLength ASC , CigarRingRange DESC
END

EXEC usp_SearchByTaste 'Woody'