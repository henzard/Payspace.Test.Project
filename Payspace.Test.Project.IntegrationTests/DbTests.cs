using Microsoft.Extensions.Logging.Abstractions;
using Payspace.Test.Project.Handlers;
using Payspace.Test.Project.Models;

namespace Payspace.Test.Project.IntegrationTests;

public class DbTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestingLiteDb()
    {
        var logger = new NullLogger<LiteDbHandler>();
        var liteDbHandler = new LiteDbHandler(logger);
        liteDbHandler.SaveRequest(
            new CalculateTransactions(new CalculateRequest { Amount = 7000, PostalCode = "7000" }, 7000, "Test User"));
        var rec = liteDbHandler.GetRecordsRequest("Test User");
        Assert.That(rec, Has.Count.EqualTo(1));
        var res = liteDbHandler.DeleteRecord(rec.First().Id.ToString());
        Assert.That(res, Is.True);
    }

    [Test]
    public void TestingSqlDb()
    {
        var logger = new NullLogger<SqlDbHandler>();
        var sqlDbHandler = new SqlDbHandler(logger);
        sqlDbHandler.SaveRequest(
            new CalculateTransactions(new CalculateRequest { Amount = 7000, PostalCode = "7000" }, 7000, "Test User"));
        var rec = sqlDbHandler.GetRecordsRequest("Test User");
        Assert.That(rec, Has.Count.EqualTo(1));
        var res = sqlDbHandler.DeleteRecord(rec.First().Id.ToString());
        Assert.That(res, Is.True);
    }
}