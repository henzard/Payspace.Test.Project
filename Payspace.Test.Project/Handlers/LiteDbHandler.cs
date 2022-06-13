using LiteDB;
using Payspace.Test.Project.Models;
using static System.GC;

namespace Payspace.Test.Project.Handlers;

public class LiteDbHandler : IDbHandler
{
    private readonly ILogger<LiteDbHandler> _logger;
    private readonly ILiteCollection<CalculateTransactions> _collection;

    public LiteDbHandler(ILogger<LiteDbHandler> logger)
    {
        _logger = logger;
        var db = new LiteDatabase(@"app.db");
        _collection = db.GetCollection<CalculateTransactions>("Transactions");
    }

    public bool SaveRequest(CalculateTransactions transaction)
    {
        try
        {
            _collection.Insert(transaction);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("{Message}", e.Message);
        }

        return false;
    }

    public List<CalculateTransactions> GetRecordsRequest(string user)
    {
        return _collection.Find(transactions => transactions.UserName == user).ToList();
    }

    public bool DeleteRecord(string id)
    {
        return _collection.Delete(id);
    }

    public void Dispose()
    {
        _logger.LogDebug("Disposed");
        SuppressFinalize(this);
    }
}