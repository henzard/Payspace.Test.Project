using System.Data;
using Microsoft.Data.SqlClient;
using Payspace.Test.Project.Extensions;
using Payspace.Test.Project.Models;

namespace Payspace.Test.Project.Handlers;

public class SqlDbHandler : IDbHandler
{
    private readonly SqlConnection _connection;
    private readonly ILogger<SqlDbHandler> _logger;

    public SqlDbHandler(ILogger<SqlDbHandler> logger)
    {
        _logger = logger;
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = "localhost\\SQLEXPRESS",
            UserID = "sa",
            Password = "Alicia07",
            InitialCatalog = "PaySpace"
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
            if (_connection.State == ConnectionState.Open) _connection.Close();

            _logger.LogError("{Message}", ex.Message);
            return false;
        }
    }

    public List<CalculateTransactions> GetRecordsRequest(string user)
    {
        if (_connection.State != ConnectionState.Open)
            _connection.Open();
        var data = new List<CalculateTransactions>();
        var sql = $"SELECT * FROM tbl_Transactions where UserName = '{user}'";

        using var command = new SqlCommand(sql, _connection);
        using var reader = command.ExecuteReader();
        while (reader.Read()) data.Add(reader.ConvertToObject<CalculateTransactions>());

        return data;
    }

    public bool DeleteRecord(string id)
    {
        try
        {
            var cmd = new SqlCommand($"Delete from tbl_Transactions where Id = '{id}'", _connection);

            if (_connection.State != ConnectionState.Open)
                _connection.Open();
            cmd.ExecuteNonQuery();
            _connection.Close();
            _logger.LogDebug("DeleteRecord");
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("{Message}", e.Message);
            return false;
        }
    }

    public void Dispose()
    {
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}