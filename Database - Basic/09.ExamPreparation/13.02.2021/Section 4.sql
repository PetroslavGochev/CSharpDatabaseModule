--11. All User Commits
GO
CREATE FUNCTION udf_AllUserCommits(@username NVARCHAR(30))
RETURNS INT
AS BEGIN 
	RETURN (SELECT
			COUNT(c.Id)
			FROM Users AS u
			JOIN Commits AS c ON u.Id = c.ContributorId
			WHERE u.Username = @username)
END

GO
SELECT dbo.udf_AllUserCommits('UnderSinduxrein')

--12. Search For Files
GO
CREATE PROCEDURE usp_SearchForFiles(@fileExtension NVARCHAR(20))
AS 
	SELECT 
	Id,
	Name,
	CAST(Size AS NVARCHAR(50)) + 'KB' AS Size
	FROM Files 
	WHERE Name LIKE CONCAT('%',@fileExtension)

	EXEC usp_SearchForFiles 'txt'