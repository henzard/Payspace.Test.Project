using LiteDB;
using Newtonsoft.Json;

namespace Payspace.Test.Project.Models;

public class CalculateTransactions
{
    public CalculateTransactions(CalculateRequest request, double result, string user)
    {
        Amount = request.Amount;
        PostalCode = request.PostalCode;
        UserName = user;
        Result = result;
    }

    public CalculateTransactions()
    {
    }

    [BsonId] public Guid Id { get; set; } = Guid.NewGuid();

    [JsonProperty("postalCode")] public string? PostalCode { get; set; }

    [JsonProperty("amount")] public double? Amount { get; set; }

    [JsonProperty("result")] public double? Result { get; set; }

    [JsonProperty("user")] public string? UserName { get; set; }

    [JsonProperty("date")] public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
}