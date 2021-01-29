--09.Find Full Name
CREATE PROCEDURE usp_GetHoldersFullName 
AS 
	SELECT  
	CONCAT_WS(' ',AH.FirstName,AH.LastName) AS FullName
	FROM AccountHolders AS AH
GO
EXEC usp_GetHoldersFullName

--10.People with Balance Higher Than
CREATE PROCEDURE usp_GetHoldersWithBalanceHigherThan (@number DECIMAL (18,4))
AS 
		SELECT 
		AH.FirstName,
		AH.LastName
		FROM AccountHolders AS AH
		JOIN Accounts AS A ON AH.Id = A.Id
		WHERE A.Balance > @number
		ORDER BY AH.FirstName,AH.LastName
GO

--11.Future Value Function
CREATE OR ALTER FUNCTION ufn_CalculateFutureValue (@initialSum DECIMAL (18,4),@yearlyInterestRate FLOAT,@numberOFYears INT)
RETURNS DECIMAL (18,4)
BEGIN 
	DECLARE @result DECIMAL (18,4)
	SELECT 
	@result = @initialSum * (POWER((1 + @yearlyInterestRate),@numberOFYears))
	RETURN @result
END
GO

SELECT dbo.ufn_CalculateFutureValue(1000,0.1,5)

--12.Calculating Interest
CREATE PROCEDURE usp_CalculateFutureValueForAccount 
AS
	SELECT 
	AH.Id AS [Account Id],
	AH.FirstName AS [First Name],
	AH.LastName AS [Last Name],
	A.Balance AS [Current Ballance],
	dbo.ufn_CalculateFutureValue(A.Balance,A.Id * 0.1,5) AS [Balance in 5 year]
	FROM AccountHolders AS AH
	JOIN Accounts AS A ON A.Id = AH.Id
GO

EXEC usp_CalculateFutureValueForAccount