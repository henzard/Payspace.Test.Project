USE [PaySpace]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_Transactions]') AND type in (N'U'))
    DROP TABLE [dbo].[tbl_Transactions]
GO
IF Not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_Transactions]') AND type in (N'U'))
create table tbl_Transactions
(
    [Id] [uniqueidentifier] NULL,
	[PostalCode] [nvarchar](250) NULL,
	[Amount] [float] NULL,
	[Result] [float] NULL,
	[UserName] [nvarchar](100) NULL,
	[TransactionDate] [datetime] NULL
)
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Transactions_Add]'))
DROP PROCEDURE [dbo].[sp_Transactions_Add]
GO
IF Not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Transactions_Add]'))
BEGIN
EXEC ('CREATE PROCEDURE [dbo].[sp_Transactions_Add] ( @Id nvarchar(100),
                                     @PostalCode nvarchar(100),
                                     @Amount nvarchar(100),
                                     @Result numeric,
                                     @UserName nvarchar(50),
                                     @TransactionDate DATETIME)
AS

	set nocount on
    Insert into tbl_Transactions(Id, PostalCode, Amount, Result, UserName, TransactionDate)
    values (@Id, @PostalCode, @Amount, @Result, @UserName, @TransactionDate)')
END
GO