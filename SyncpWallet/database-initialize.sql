-- Create the database
CREATE DATABASE SyncpWalletDB;
GO

-- Use the newly created database
USE SyncpWalletDB;
GO

-- Create the Wallets table
CREATE TABLE Wallets (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(MAX),
    Currency NVARCHAR(MAX),
    Amount DECIMAL(18, 2)
);
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_WalletInsert
	@name NVARCHAR(MAX),
	@amount DECIMAl,
	@currency NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO dbo.Wallets(Name, Amount, Currency) VALUES(@name, @amount, @currency)
	SELECT CAST(SCOPE_IDENTITY() AS int) AS WalletId
END
GO
