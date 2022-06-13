using Newtonsoft.Json;

namespace Payspace.Test.Project.Models;

public class CalculateRequest
{
    [JsonProperty("postalCode")] public string? PostalCode { get; set; }

    [JsonProperty("amount")] public double? Amount { get; set; }
}