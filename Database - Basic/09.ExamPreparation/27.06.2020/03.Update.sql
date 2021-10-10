USE WMS

UPDATE Jobs
SET MechanicId = 3,
Status = 'In Progress'
WHERE Status = 'Pending'


