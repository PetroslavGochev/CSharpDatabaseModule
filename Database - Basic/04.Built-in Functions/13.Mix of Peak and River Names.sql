--13.Mix of Peak and River Names
SELECT [PeakName],
[RiverName],
CONCAT
(Lower
(SUBSTRING(PeakName,1,Len(PeakName)-1)),Lower(RiverName)) AS Mix 
FROM [Peaks],[Rivers]
WHERE SUBSTRING(PeakName,Len(PeakName),1) = SUBSTRING(RiverName,1,1)
ORDER BY Mix
