--13.Scalar Function: Cash in User Games Odd Rows
CREATE FUNCTION	ufn_CashInUsersGames (@gameName NVARCHAR(250))
RETURNS TABLE
AS
	
RETURN SELECT 
		SUM(T.Cash) AS SumCash
		FROM(
		SELECT 
		ROW_NUMBER() OVER (ORDER BY UG.Cash DESC) AS Id,
		UG.Cash AS Cash
		FROM UsersGames AS UG
		JOIN Games AS G ON G.Id = UG.GameId
		WHERE G.Name = @gameName) AS T
		WHERE T.Id % 2 != 0
GO
SELECT *
FROM dbo.ufn_CashInUsersGames('Love in a mist')