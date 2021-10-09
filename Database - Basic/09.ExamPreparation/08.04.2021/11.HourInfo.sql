CREATE FUNCTION udf_HoursToComplete(@StartDate DATETIME, @EndDate DATETIME)
RETURNS INT
AS BEGIN
	DECLARE @result INT;
	IF (@StartDate IS NOT NULL AND @EndDate IS NOT NULL)
	SET @result = datediff(HOUR, @StartDate, @EndDate)
	ELSE 
	SET @result = 0;
	RETURN @result
END


SELECT dbo.udf_HoursToComplete(OpenDate, CloseDate) AS TotalHours
   FROM Reports

   SELECT OpenDate, CloseDate FROM Reports