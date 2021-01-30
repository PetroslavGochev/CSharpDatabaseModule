--01.Create Table Logs
CREATE TABLE Logs 
(
	LogId INT PRIMARY KEY IDENTITY,
	AccountID INT FOREIGN KEY REFERENCES Accounts(Id),
	OldSum MONEY,
	NewSum MONEY
)
CREATE  TRIGGER trgAfterUpdate ON Accounts FOR UPDATE
AS
	INSERT INTO Logs (AccountID,OldSum,NewSum)
	SELECT 
	i.Id AS AccountId,
	d.Balance AS OldSum,
	i.Balance AS NewSum
	FROM inserted AS i
	JOIN deleted AS d ON d.Id = i.Id
	WHERE i.Balance != d.Balance
GO

--02.Create Table Emails
CREATE TABLE NotificationEmails
(
	Id INT PRIMARY KEY IDENTITY,
	Recipient INT FOREIGN KEY REFERENCES Accounts(Id),
	[Subject] NVARCHAR(MAX),
	[Body] NVARCHAR(MAX)
)

CREATE  TRIGGER trg_insert_logTable ON Logs FOR INSERT
AS
	DECLARE @recipient INT = (SELECT AccountID FROM inserted)
	DECLARE @subject NVARCHAR(MAX) = (SELECT 'Balance change for account: ' + CAST(AccountID AS nvarchar) FROM inserted)
	DECLARE @body NVARCHAR(MAX) = (SELECT 'On ' + CAST(GETDATE() AS nvarchar) + ' your balance was changed from ' + CAST(OldSum AS nvarchar) + ' to ' + CAST(NewSum AS nvarchar) FROM inserted)
	INSERT INTO NotificationEmails(Recipient,Subject,Body)
	VALUES (@recipient,@subject,@body)
GO

--03.Deposit Money
CREATE PROCEDURE usp_DepositMoney (@AccountId INT , @MoneyAmount DECIMAL(18,4))
AS 
 BEGIN TRANSACTION 
	DECLARE @accountExist INT = (SELECT COUNT(*) 
	FROM Accounts AS A
	WHERE A.Id = @AccountId)
	IF(@MoneyAmount < 0)
	BEGIN
		THROW 50001, 'Money should be positive number',1
	END
	IF(@accountExist = 0)
	BEGIN 
		THROW 50002, 'AccountId doesn`t exist',1
	END

UPDATE Accounts 
 SET Balance += @MoneyAmount
 WHERE Id = @AccountId
 COMMIT

EXEC usp_DepositMoney @AccountID = 1,@MoneyAmount = 10


--04.Withdraw Money
CREATE OR ALTER PROCEDURE usp_WithdrawMoney  (@AccountId INT , @MoneyAmount DECIMAL(18,4))
AS 
	BEGIN TRANSACTION 
	DECLARE @accountExist INT = (SELECT COUNT(*) 
	FROM Accounts AS A
	WHERE A.Id = @AccountId)
	IF(@MoneyAmount < 0)
	BEGIN
		THROW 50001, 'Money should be positive number',1
	END
	IF(@accountExist = 0)
	BEGIN 
		THROW 50002, 'AccountId doesn`t exist',1
	END

UPDATE Accounts 
 SET Balance -= @MoneyAmount
 WHERE Id = @AccountId
 COMMIT
GO
EXEC usp_WithdrawMoney @AccountID = 1,@MoneyAmount = 10

--05.Money Transfer

CREATE PROCEDURE usp_TransferMoney(@SenderId INT, @ReceiverId INT, @Amount MONEY)
AS
 BEGIN TRANSACTION
	DECLARE @senderExist INT = (SELECT COUNT(*) FROM Accounts WHERE @SenderId = Id)
	DECLARE @receiverExist INT= (SELECT COUNT(*) FROM Accounts WHERE @ReceiverId = Id)
	IF(@Amount < 0 )
		BEGIN
			THROW 50001, 'Money should be positive number',1
		END
	ELSE IF(@senderExist = 0)
		BEGIN
			THROW 50002, 'Sender account doesn`t exist',1
		END
	ELSE IF(@receiverExist = 0)
		BEGIN
			THROW 50003, 'Receiver account doesn`t exist',1
		END
	ELSE IF((SELECT Balance FROM Accounts WHERE @SenderId = Id) < @Amount)
		BEGIN
			THROW 50004, 'Sender account doesn`t have enough money',1
		END
	ELSE
		BEGIN
		EXEC usp_DepositMoney @ReceiverId,@Amount
		EXEC usp_WithdrawMoney @SenderId,@Amount
		END

COMMIT