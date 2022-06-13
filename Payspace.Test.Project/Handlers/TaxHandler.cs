using Newtonsoft.Json;
using Payspace.Test.Project.Extensions;
using Payspace.Test.Project.Models;

namespace Payspace.Test.Project;

public interface ITaxHandler
{
    double CalculateTax(CalculateRequest request);
}

public class TaxHandler : ITaxHandler
{
    private readonly ILogger<TaxHandler> _logger;

    public TaxHandler(ILogger<TaxHandler> logger)
    {
        _logger = logger;
    }

    public double CalculateTax(CalculateRequest request)
    {
        _logger.LogDebug("{FunctionName} with {Data}", nameof(CalculateTax), JsonConvert.SerializeObject(request));
        if (string.IsNullOrWhiteSpace(request.PostalCode ?? ""))
            throw new NullReferenceException($"{nameof(request.PostalCode)} cannot be null or empty");
        if (request.Amount is null or <= 0)
            throw new NullReferenceException($"{nameof(request.Amount)} cannot be null or less than 0");
        return TaxExtensions.GetTaxAmount(request.PostalCode ?? "", request.Amount ?? 0);
    }
}