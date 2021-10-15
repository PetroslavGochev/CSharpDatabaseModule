--12. Create Procedure

GO
CREATE PROC usp_SwitchRoom (@TripId INT, @TargetRoomId INT)
AS
BEGIN
	DECLARE	@CurrentRoom INT = (SELECT TOP(1) RoomId FROM Trips WHERE Id = @TripId)

	IF (SELECT TOP(1) HotelId FROM Rooms WHERE Id = @CurrentRoom) !=
	   (SELECT TOP(1) HotelId FROM Rooms WHERE Id = @TargetRoomId)
			THROW 51000, 'Target room is in another hotel!', 1

	
	IF (SELECT Beds FROM Rooms WHERE Id = @TargetRoomId) <
	   (
	    SELECT 
			COUNT(*)
			FROM Trips AS t
			JOIN AccountsTrips AS at ON t.Id = at.TripId
			WHERE t.Id = @TripId
			GROUP BY t.Id
		   )
				THROW 51001, 'Not enough beds in target room!', 1

	   UPDATE Trips
	   SET RoomId = @TargetRoomId
	   WHERE Id = @TripId
END

GO

EXEC usp_SwitchRoom 10, 11
SELECT RoomId FROM Trips WHERE Id = 10
EXEC usp_SwitchRoom 10, 7
EXEC usp_SwitchRoom 10, 8