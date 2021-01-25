SELECT [MountainRange],[PeakName],[Elevation]
FROM [Mountains] JOIN [Peaks]
ON [Peaks].MountainId = Mountains.Id
WHERE [MountainRange] = 'Rila'
ORDER BY [Elevation] DESC