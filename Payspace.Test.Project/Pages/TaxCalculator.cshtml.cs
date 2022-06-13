using Microsoft.AspNetCore.Mvc.RazorPages;
using Payspace.Test.Project.Handlers;
using Payspace.Test.Project.Models;
using static Newtonsoft.Json.JsonConvert;

namespace Payspace.Test.Project.Pages;

public class TaxCalculator : PageModel
{
    private readonly IDbHandler _dbHandler;
    private readonly ILogger<TaxCalculator> _logger;
    private readonly ITaxHandler _taxHandler;
    private string? _user;

    public TaxCalculator(ITaxHandler taxHandler, IDbHandler dbHandler, ILogger<TaxCalculator> logger)
    {
        _taxHandler = taxHandler;
        _dbHandler = dbHandler;
        _logger = logger;
    }

    public new CalculateRequest? Request { get; set; }
    public List<CalculateTransactions>? TransactionsList { get; set; }

    public void OnPostDelete(string id)
    {
        _logger.LogWarning("Deleting {Id}", id);
        _user = User.Identity?.Name ?? "Unknown";
        ViewData["UserName"] = _user;
        if (!_dbHandler.DeleteRecord(id))
            ViewData["Error"] = "Record cannot Delete";
        TransactionsList = _dbHandler.GetRecordsRequest(_user);
    }

    public void OnPostEdit(CalculateRequest request)
    {
        _user = User.Identity?.Name ?? "Unknown";
        _logger.LogDebug("{Data}", SerializeObject(request));
        var response = _taxHandler.CalculateTax(request);
        if (_user == "Unknown")
            _logger.LogWarning("User not Found");
        _dbHandler.SaveRequest(new CalculateTransactions(request, response, _user));
        ViewData["LastResult"] = response;
        ViewData["UserName"] = _user;
        TransactionsList = _dbHandler.GetRecordsRequest(_user);
    }

    public void OnGet()
    {
        _user = User.Identity?.Name ?? "Unknown";
        ViewData["UserName"] = _user;
        TransactionsList = _dbHandler.GetRecordsRequest(_user);
    }
}