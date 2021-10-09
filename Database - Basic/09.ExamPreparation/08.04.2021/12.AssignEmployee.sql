CREATE PROCEDURE usp_AssignEmployeeToReport(@EmployeeId INT, @ReportId INT)
AS 
	DECLARE @employee INT = (
		SELECT 
		[DepartmentId]
		FROM Employees AS E 
		WHERE E.Id = @EmployeeId)
	DECLARE @reports INT = (
		SELECT DepartmentId FROM Reports AS R
		JOIN Categories AS C ON C.Id = R.CategoryId
		WHERE R.Id = @ReportId)
	IF (@employee != @reports)
	THROW 100000, 'Employee doesn''t belong to the appropriate department!',1
	
	UPDATE Reports
	SET EmployeeId = @EmployeeId
	WHERE Reports.Id = @ReportId
GO

EXEC usp_AssignEmployeeToReport 30, 1


