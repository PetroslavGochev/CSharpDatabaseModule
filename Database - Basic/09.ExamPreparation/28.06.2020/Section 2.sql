--Section 2

-- 02. Insert

INSERT INTO Planets (Name) VALUES
('Mars'),
('Earth'),
('Jupiter'),
('Saturn')

INSERT INTO Spaceships (Name, Manufacturer, LightSpeedRate) VALUES
('Golf','VW', 3),
('WakaWaka','Wakanda', 4),
('Falcon9','SpaceX', 1),
('Bed','Vidolov', 6)

--03.Update

UPDATE Spaceships
SET LightSpeedRate += 1
WHERE Id >= 8 AND Id <= 12

--04. Delete
DELETE FROM TravelCards 
WHERE JourneyId BETWEEN 1 AND 3

DELETE FROM Journeys 
WHERE Id BETWEEN 1 AND 3