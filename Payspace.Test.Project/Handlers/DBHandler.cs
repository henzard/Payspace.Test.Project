using System.Data;
using LiteDB;
using Microsoft.Data.SqlClient;
using Payspace.Test.Project.Extensions;
using Payspace.Test.Project.Models;
using static System.GC;

namespace Payspace.Test.Project.Handlers;

public interface IDbHandler : IDisposable
{
    bool SaveRequest(CalculateTransactions transaction);
    List<CalculateTransactions> GetRecordsRequest(string user);
}

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

    public void Dispose()
    {
        _logger.LogDebug("Disposed");
        SuppressFinalize(this);
    }
}

public class SqlDbHandler : IDbHandler
{
    private readonly ILogger<SqlDbHandler> _logger;
    private readonly SqlConnection _connection;

    public SqlDbHandler(ILogger<SqlDbHandler> logger)
    {
        _logger = logger;
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = "localhost\\SQLEXPRESS",
            UserID = "sa",
            Password = "Alicia07",
            InitialCatalog = "<your_database>"
        };
        _connection = new SqlConnection(builder.ConnectionString);
    }

    public bool SaveRequest(CalculateTransactions transaction)
    {
        try
        {
            var cmd = new SqlCommand("sp_Transactions_Add", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", transaction.Id);
            cmd.Parameters.AddWithValue("@PostalCode", transaction.PostalCode);
            cmd.Parameters.AddWithValue("@Amount", transaction.Amount);
            cmd.Parameters.AddWithValue("@Result", transaction.Result);
            cmd.Parameters.AddWithValue("@UserName", transaction.UserName);
            cmd.Parameters.AddWithValue("@TransactionDate", transaction.TransactionDate);
            _connection.Open();
            cmd.ExecuteNonQuery();
            _connection.Close();
            _logger.LogDebug("Sql data saved");
            return true;
        }
        catch (Exception ex)
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }

            _logger.LogError("{Message}", ex.Message);
            return false;
        }
    }

    public List<CalculateTransactions> GetRecordsRequest(string user)
    {
        _connection.Open();
        var data = new List<CalculateTransactions>();
        var sql = $"SELECT * FROM Transactions where user = {user}";

        using var command = new SqlCommand(sql, _connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            data.Add(reader.ConvertToObject<CalculateTransactions>());
        }

        return data;
    }

    public void Dispose()
    {
        _connection.Dispose();
        SuppressFinalize(this);
    }
}