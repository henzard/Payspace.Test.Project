using Payspace.Test.Project.Models;

namespace Payspace.Test.Project.Handlers;

public interface IDbHandler : IDisposable
{
    bool SaveRequest(CalculateTransactions transaction);
    List<CalculateTransactions> GetRecordsRequest(string user);
}