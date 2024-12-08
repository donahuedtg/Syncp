-- Create the database
CREATE DATABASE SyncpWalletDB;
GO

-- Use the newly created database
USE SyncpWalletDB;
GO

-- Create the Wallets table
CREATE TABLE [dbo].[Wallets] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(MAX),
    Currency NVARCHAR(MAX),
    Amount DECIMAL(18, 2)
);
GO

CREATE TABLE [dbo].[Operations](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[WalletId] [INT] NOT NULL,
	[Amount] [DECIMAL](18, 2) NOT NULL,
	[TimeStamp] [DATETIME2](7) NOT NULL DEFAULT (GETDATE())
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


ALTER TABLE [dbo].[Operations]  WITH CHECK ADD  CONSTRAINT [FK_Operations_Wallets] FOREIGN KEY([WalletId])
REFERENCES [dbo].[Wallets] ([Id])
GO




SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_WalletGet]
AS
BEGIN
SELECT [Id]
      ,[Name]
      ,[Currency]
      ,[Amount]
  FROM [dbo].[Wallets]
END



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_WalletInsert]
	@name NVARCHAR(MAX),
	@amount DECIMAL(18,2),
	@currency NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Id int;

    INSERT INTO dbo.Wallets(Name, Amount, Currency) VALUES(@name, @amount, @currency)
    
    SET @Id = SCOPE_IDENTITY();
    SELECT [Id]
		  ,[Name]
		  ,[Currency]
		  ,[Amount]
	  FROM [dbo].[Wallets]
	  WHERE [Id] = @Id
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_WalletDelete]
	@Id int
AS
BEGIN
	BEGIN TRY
	SET NOCOUNT ON;
		DECLARE @CurrentId int;
		SET @CurrentId = (SELECT Id FROM [dbo].Wallets WHERE Id = @Id)
		IF (@CurrentId IS NULL)
		BEGIN
			RAISERROR('Wallet not found', 16 , 1);
		END

		DELETE FROM [dbo].Wallets
			WHERE Id = @Id

		SELECT @Id
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_WalletExpenseOperation]
	@WalletId INT,
	@ExpenseAmount DECIMAL(18, 2)
AS
BEGIN
	BEGIN TRY
	SET NOCOUNT ON;

	IF (@ExpenseAmount > 0)
		BEGIN
			RAISERROR('Amount must be negative number', 16 , 1);
		END

	DECLARE @OperationId INT
	DECLARE @WalletCurrentAmount DECIMAL(18, 2);

	SET @WalletCurrentAmount = (SELECT Amount FROM [dbo].[Wallets] WHERE Id = @WalletId)
	IF (@WalletCurrentAmount IS NULL)
		BEGIN
			RAISERROR('Wallet not found', 16 , 1);
		END
	IF (@ExpenseAmount + @WalletCurrentAmount < 0)
		BEGIN
			RAISERROR('Wallet amount is not enough', 16 , 1);
		END
	
	BEGIN TRAN;
		INSERT INTO [dbo].[Operations] (WalletId, Amount) VALUES(@WalletId, @ExpenseAmount);
		SET @OperationId = SCOPE_IDENTITY();

		IF (@OperationId <= 0)
			BEGIN
				RAISERROR('Operation insert error', 16 , 1);
			END;
		
		UPDATE [dbo].[Wallets]
		SET Amount = (@ExpenseAmount + @WalletCurrentAmount)
		WHERE Id = @WalletId;

		IF (@@ROWCOUNT NOT IN (1))
			BEGIN
				RAISERROR('@Wallet update error', 16 , 1);
			END

		SELECT w.[Id]
				,w.[Name]
				,w.[Currency]
				,w.[Amount] AS TotalAmount
				,o.[Amount] AS ExpenseAmmount
				,o.[TimeStamp]
			FROM [dbo].[Operations] o
			LEFT JOIN [dbo].[Wallets] w
				ON w.Id = o.WalletId
			WHERE o.Id = @OperationId;

		COMMIT;	
	END TRY
	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK;
			END;

		THROW;
	END CATCH
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_WalletIncomeOperation]
	@WalletId INT,
	@IncomeAmount DECIMAL(18, 2)
AS
BEGIN
	BEGIN TRY
	SET NOCOUNT ON;

	IF (@IncomeAmount < 0)
		BEGIN
			RAISERROR('Amount must be positive number', 16 , 1);
		END

	DECLARE @OperationId INT
	DECLARE @WalletCurrentAmount DECIMAL(18, 2);

	SET @WalletCurrentAmount = (SELECT Amount FROM [dbo].[Wallets] WHERE Id = @WalletId)
	IF (@WalletCurrentAmount IS NULL)
		BEGIN
			RAISERROR('Wallet not found', 16 , 1);
		END

	BEGIN TRAN;
		INSERT INTO [dbo].[Operations] (WalletId, Amount) VALUES(@WalletId, @IncomeAmount);
		SET @OperationId = SCOPE_IDENTITY();

		IF (@OperationId <= 0)		
			BEGIN
				RAISERROR('Operation insert error', 16 , 1);
			END

		UPDATE [dbo].[Wallets]
		SET Amount = (@IncomeAmount + @WalletCurrentAmount)
		WHERE Id = @WalletId;

		IF (@@ROWCOUNT NOT IN (1))
			BEGIN
				RAISERROR('Wallet update error', 16 , 1);
			END

		SELECT w.[Id]
				,w.[Name]
				,w.[Currency]
				,w.[Amount] AS TotalAmount
				,o.[Amount] AS IncomeAmount
				,o.[TimeStamp]
			FROM [dbo].[Operations] o
			LEFT JOIN [dbo].[Wallets] w
				ON w.Id = o.WalletId
			WHERE o.Id = @OperationId;

		COMMIT;
	END TRY
	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK;
			END;

		THROW;
	END CATCH
END
