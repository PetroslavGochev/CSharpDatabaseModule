--01.Employees with Salary Above 35000
CREATE sPROCEDURE usp_GetEmployeesSalaryAbove35000
	AS
		SELECT 
		FirstName,
		LastName
		FROM Employees
		WHERE Salary > 35000
	GO;
EXEC usp_GetEmployeesSalaryAbove35000;

--02.Employees with Salary Above Number
CREATE PROCEDURE usp_GetEmployeesSalaryAboveNumber @Number DECIMAL(18,4)
	AS
		SELECT 
		FirstName,
		LastName
		FROM Employees
		WHERE Salary >= @Number
	GO
EXEC usp_GetEmployeesSalaryAboveNumber @Number = 48100

--03.Town Names Starting With
CREATE PROCEDURE usp_GetTownsStartingWith  @Letter NVARCHAR(20)
	AS 
		Select 
		Towns.Name AS Town
		FROM Towns
		WHERE Towns.Name Like @Letter+'%'
	GO
EXEC usp_GetTownsStartingWith @Letter = 'B'

--04.Employees from Town
CREATE PROCEDURE usp_GetEmployeesFromTown @TownName NVARCHAR(50)
	AS
		SELECT 
		FirstName,
		LastName
		FROM Employees AS E
		JOIN Addresses AS A ON A.AddressID = E.AddressID
		JOIN Towns AS T ON T.TownID = A.TownID
		WHERE T.Name = @TownName
	GO
EXEC usp_GetEmployeesFromTown @TownName = 'Sofia'

--05.Salary Level Function

CREATE FUNCTION ufn_GetSalaryLevel(@salary DECIMAL(18,4))
RETURNS VARCHAR(250)
AS BEGIN
	DECLARE @result NVARCHAR(250) 
	 IF (@salary < 30000)
        SET @result = 'Low';
    ELSE IF (@salary>= 30000 AND @salary <= 50000)
            SET @result = 'Average';
      ELSE
            SET @result = 'High';
	return @result
END

SELECT 
dbo.ufn_GetSalaryLevel(Salary)
FROM Employees

--06.Employees by Salary Level
CREATE PROCEDURE usp_EmployeesBySalaryLevel @levelOfSalary NVARCHAR(250)
AS
	Select 
	SalaryLevel.FirstName,
	SalaryLevel.LastName
	From
	(
	SELECT 
	FirstName,
	LastName,
	dbo.ufn_GetSalaryLevel(Salary) AS [Level]
	FROM Employees) AS SalaryLevel
	WHERE SalaryLevel.Level = @levelOfSalary
Go

EXEC usp_EmployeesBySalaryLevel @levelOfSalary = 'High'

--07.Define Function
CREATE FUNCTION ufn_IsWordComprised(@setOfLetters varchar(max), @word varchar(max))
    RETURNS bit AS
BEGIN
    DECLARE @WordLength int=LEN(@word);
    DECLARE @Index int=1;

    WHILE (@Index <= @WordLength)
        BEGIN
            IF (CHARINDEX(SUBSTRING(@word, @Index, 1), @setOfLetters)=0)
                BEGIN
                    RETURN 0
                END

            SET @Index+=1
        END
    RETURN 1
END
GO

--08.Delete Employees and Departments
CREATE PROC usp_DeleteEmployeesFromDepartment(@departmentId INT)
AS
BEGIN

    DELETE
    FROM EmployeesProjects
    WHERE EmployeeID IN (SELECT EmployeeID
                         FROM Employees
                         WHERE DepartmentID = @departmentId);

    UPDATE Employees
    SET ManagerID=NULL
    WHERE ManagerID IN (SELECT EmployeeID
                        FROM Employees
                        WHERE DepartmentID = @departmentId);

    ALTER TABLE Departments
        ALTER COLUMN ManagerID int;

    UPDATE Departments
    SET ManagerID = NULL
    WHERE ManagerID IN (SELECT EmployeeID
                        FROM Employees
                        WHERE DepartmentID = @departmentId);

    --delete employees from departments

    DELETE FROM Employees
    WHERE DepartmentID=@departmentId

    DELETE FROM Departments
    where DepartmentID=@departmentId

    SELECT count(*)
    FROM Employees
    where DepartmentID=@departmentId

END