-- Section 4

--11.Get Colonists Count
GO

CREATE FUNCTION dbo.udf_GetColonistsCount(@PlanetName VARCHAR (30))
RETURNS INT
AS
BEGIN
	DECLARE @count INT = 
				(SELECT COUNT(*) FROM Planets AS P 
				JOIN Spaceports AS SP ON SP.PlanetId = P.Id
				JOIN Journeys AS J ON J.DestinationSpaceportId = SP.Id
				JOIN TravelCards AS TC ON TC.JourneyId = J.Id
				JOIN Colonists AS C ON C.Id = TC.ColonistId
				WHERE P.Name = @PlanetName)
	RETURN @count
END

SELECT dbo.udf_GetColonistsCount('Otroyphus')

--12 Change Journey Purpose

GO 
CREATE PROCEDURE usp_ChangeJourneyPurpose(@JourneyId INT, @NewPurpose VARCHAR(50))
AS 
BEGIN
	DECLARE @CurrentPurpose VARCHAR(20) = 
					(SELECT Purpose FROM Journeys 
					 WHERE Id = @JourneyId)
	DECLARE @JourneyExist INT = (SELECT J.Id FROM Journeys AS J
					 WHERE Id = @JourneyId)
				IF @JourneyExist IS NULL 
			 THROW 50001, 'The journey does not exist!', 1;
				IF @CurrentPurpose = @NewPurpose 
			 THROW 50002, 'You cannot change the purpose!', 1;
	UPDATE Journeys
	SET Purpose = @NewPurpose
	WHERE Id = @JourneyId
END

EXEC usp_ChangeJourneyPurpose 4, 'Technical'
EXEC usp_ChangeJourneyPurpose 2, 'Educational'
EXEC usp_ChangeJourneyPurpose 196, 'Technical'