using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payspace.Test.Project.Handlers;
using Payspace.Test.Project.Models;

namespace Payspace.Test.Project.Controllers;

[Authorize]
public class TaxSystem : Controller
{
    private readonly IDbHandler _dbHandler;
    private readonly ILogger<TaxSystem> _logger;
    private readonly ITaxHandler _taxHandler;

    public TaxSystem(ITaxHandler taxHandler, IDbHandler dbHandler, ILogger<TaxSystem> logger)
    {
        _taxHandler = taxHandler;
        _dbHandler = dbHandler;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Calculate(CalculateRequest request)
    {
        try
        {
            var response = _taxHandler.CalculateTax(request);
            var user = User.Identity?.Name ?? "Unknown";
            if (user == "Unknown")
                _logger.LogWarning("User not Found");
            _dbHandler.SaveRequest(new CalculateTransactions(request, response, user));
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError("{Message}", e.Message);
            return BadRequest(e.Message);
        }
    }
}