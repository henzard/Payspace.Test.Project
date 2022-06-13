using Payspace.Test.Project.Models;

namespace Payspace.Test.Project;

public interface ITaxHandler
{
    double CalculateTax(CalculateRequest request);
}