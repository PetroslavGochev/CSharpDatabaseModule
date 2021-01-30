--09.Find Full Name
CREATE PROCEDURE usp_GetHoldersFullName 
AS 
	SELECT  
	CONCAT_WS(' ',AH.FirstName,AH.LastName) AS FullName
	FROM AccountHolders AS AH
GO
EXEC usp_GetHoldersFullName

--10.People with Balance Higher Than
CREATE PROCEDURE usp_GetHoldersWithBalanceHigherThan(@Money decimal(18, 4))
AS
BEGIN
    SELECT FirstName AS 'First Name',
           LastName  AS 'Last Name'
    FROM Accounts AS a
             JOIN AccountHolders AH ON a.AccountHolderId = AH.Id
    GROUP BY FirstName, LastName
    HAVING SUM(Balance) > @Money
    ORDER BY FirstName, LastName
END

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
CREATE  PROC usp_CalculateFutureValueForAccount(@accountId INT, @interestRate FLOAT)
AS
BEGIN
    SELECT a.Id    ,
           AH.FirstName,
          AH.LastName ,
          a.Balance,
           dbo.ufn_CalculateFutureValue(a.Balance,@interestRate, 5)                                       
    FROM Accounts AS a
             JOIN AccountHolders AS AH ON AH.Id = a.AccountHolderId
             WHERE a.Id=@accountId;
END
GO

EXEC usp_CalculateFutureValueForAccount