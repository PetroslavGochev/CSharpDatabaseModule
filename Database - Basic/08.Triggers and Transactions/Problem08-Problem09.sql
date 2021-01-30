--08.Employees with Three Projects
CREATE PROCEDURE usp_AssignProject(@emloyeeId INT, @projectID INT) 
AS
	BEGIN TRANSACTION
	DECLARE @numberOfProjects INT = (SELECT COUNT(*) FROM EmployeesProjects AS EP WHERE EP.EmployeeID = @emloyeeId)
	IF (@numberOfProjects >= 3)
		BEGIN 
			ROLLBACK
			RAISERROR('The employee has too many projects!',16,1)		
		END
	ELSE 
		BEGIN
			INSERT INTO EmployeesProjects (EmployeeID,ProjectID)
			VALUES (@emloyeeId,@projectID)
		END
	COMMIT

--09.Delete Employees
CREATE TABLE Deleted_Employees
(
	EmployeeId INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(200),
	LastName NVARCHAR(200),
	MiddleName NVARCHAR(200),
	JobTitle NVARCHAR(200),
	DepartmentId INT FOREIGN KEY REFERENCES Departments(DepartmentID),
	Salary DECIMAL(18,4)
)

CREATE TRIGGER trg_DeleteEmployee ON Employees 
AFTER DELETE
AS
	INSERT INTO Deleted_Employees
	SELECT 
	d.FirstName,
	d.LastName,
	d.MiddleName,
	d.JobTitle,
	d.DepartmentID,
	d.Salary
	FROM deleted AS d
GO
