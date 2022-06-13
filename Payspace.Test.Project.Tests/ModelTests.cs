namespace Payspace.Test.Project.Tests;

public class ModelTests
{
    private const string PostalCode = "0000";
    private const string UserName = "Test User";
    private const double Amount = 100;
    private const double Result = 0;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void When_New_CalculateTransactions_Then_Id_And_Date_Is_Not_Default()
    {
        var output = new CalculateTransactions
        {
            Amount = Amount,
            PostalCode = PostalCode,
            UserName = UserName,
            Result = Result
        };
        Assert.Multiple(() =>
        {
            Assert.That(output.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(output.TransactionDate, Is.Not.EqualTo(default));
        });
        Assert.Pass();
    }

    [Test]
    public void When_New_CalculateTransactions_Then_Both_Constructors_Match()
    {
        var outputA = new CalculateTransactions
        {
            Amount = Amount,
            PostalCode = PostalCode,
            UserName = UserName,
            Result = Result
        };
        var outputB = new CalculateTransactions(new CalculateRequest
        {
            Amount = Amount,
            PostalCode = PostalCode
        }, Result, UserName);
        Assert.Multiple(() =>
        {
            Assert.That(outputA.Amount, Is.EqualTo(outputB.Amount));
            Assert.That(outputA.PostalCode, Is.EqualTo(outputB.PostalCode));
            Assert.That(outputA.UserName, Is.EqualTo(outputB.UserName));
            Assert.That(outputA.Result, Is.EqualTo(outputB.Result));
        });
        Assert.Pass();
    }
}