using Payspace.Test.Project.Models;

namespace Payspace.Test.Project.Extensions;

public static class TaxExtensions
{
    public static double GetTaxAmount(string postalCode, double amount)
    {
        var taxType = PostalCodeToTaxCalculation(postalCode);
        return taxType switch
        {
            TaxTypes.NotSetup => throw new NotImplementedException("Postal Code not Setup"),
            TaxTypes.Progressive => ProgressiveCalculateAmount(amount),
            TaxTypes.FlatValue => FlatValueCalculateAmount(amount),
            TaxTypes.FlatRate => FlatRateCalculateAmount(amount),
            _ => throw new ArgumentOutOfRangeException(nameof(postalCode))
        };
    }
    public static TaxTypes PostalCodeToTaxCalculation(string postalCode)
    {
        return postalCode switch
        {
            "7441" => TaxTypes.Progressive,
            "A100" => TaxTypes.FlatValue,
            "7000" => TaxTypes.FlatRate,
            "1000" => TaxTypes.Progressive,
            _ => TaxTypes.NotSetup
        };
    }

    public static double FlatRateCalculateAmount(double amount)
    {
        return amount * .175;
    }
    public static double ProgressiveCalculateAmount(double amount)
    {
        var brackets = new Dictionary<double, double>
        {
            { 8350, .1 },
            { 33950, .15 },
            { 82250, .25 },
            { 171550, .28 },
            { 372950, .33 },
            { int.MaxValue, .35 }
        };
        return CalcProgressive(amount, brackets);
    }
    public static double FlatValueCalculateAmount(double amount)
    {
        if (amount >= 200_000)
        {
            return 10_000;
        }

        return amount * .15;
    }
    public static double CalcProgressive(double amount, Dictionary<double, double> brackets)
    {
        var taxAmount = 0D;
        var prevBracket = 0D;
        foreach (var (bracket, percentage) in brackets)
        {
            if (amount <= bracket)
                return taxAmount + (amount - prevBracket) * percentage;
            taxAmount += bracket * percentage;
            prevBracket = bracket;
        }

        return taxAmount;
    }
}