create table tbl_Transactions
(
    Id         nvarchar(250),
    PostalCode nvarchar(250),
    Amount     nvarchar(100),
    Result     numeric,
    UserName   nvarchar(100),
    TransactionDate       DATETIME
)

create procedure sp_Transactions_Add @Id nvarchar(100),
                                     @PostalCode nvarchar(100),
                                     @Amount nvarchar(100),
                                     @Result numeric,
                                     @UserName nvarchar(50),
                                     @TransactionDate DATETIME
AS
BEGIN
    Insert into tbl_Transactions(Id, PostalCode, Amount, Result, UserName, TransactionDate)
    values (@Id, @PostalCode, @Amount, @Result, @UserName, @TransactionDate)
END 