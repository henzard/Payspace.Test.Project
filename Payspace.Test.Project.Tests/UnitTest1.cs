using System.Security.Cryptography.X509Certificates;
using Payspace.Test.Project.Extensions;
using Payspace.Test.Project.Models;

namespace Payspace.Test.Project.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }
    
    [TestCase("7441", ExpectedResult = "Progressive")]
    [TestCase("A100", ExpectedResult = "Flat Value")]
    [TestCase("7000", ExpectedResult = "Flat rate")]
    [TestCase("1000", ExpectedResult = "Progressive")]
    [TestCase("0000", ExpectedResult = "Postal Code Not Setup")]
    public string WhenPostalCodeThenGetTaxCalculation(string postalCode)
    {
        Assert.That(!string.IsNullOrWhiteSpace(postalCode), Is.True);
        return postalCode switch
        {
            "7441" => "Progressive",
            "A100" => "Flat Value",
            "7000" => "Flat rate",
            "1000" => "Progressive",
            _ => "Postal Code Not Setup"
        };
    }

    [TestCase("7441", ExpectedResult = TaxTypes.Progressive)]
    [TestCase("A100", ExpectedResult = TaxTypes.FlatValue)]
    [TestCase("7000", ExpectedResult = TaxTypes.FlatRate)]
    [TestCase("1000", ExpectedResult = TaxTypes.Progressive)]
    [TestCase("0000", ExpectedResult = TaxTypes.NotSetup)]
    public TaxTypes WhenPostalCodeThenGetTaxCalculation_New(string postalCode)
    {
        Assert.That(!string.IsNullOrWhiteSpace(postalCode), Is.True);
        return TaxExtensions.PostalCodeToTaxCalculation(postalCode);
    }

    [TestCase(2, ExpectedResult = 0.3)]
    [TestCase(10_001, ExpectedResult = 1500.1499999999999)]
    [TestCase(200_000, ExpectedResult = 10_000)]
    [TestCase(400_000, ExpectedResult = 10_000)]
    public double WhenFlatValueThenCalculateAmount(double amount)
    {
        //Amount must be positive and not 0
        Assert.That(amount, Is.GreaterThan(0));
        return TaxExtensions.FlatValueCalculateAmount(amount);
    }
    [TestCase(2, ExpectedResult = 0.3)]
    [TestCase(10_001, ExpectedResult = 1500.1499999999999)]
    [TestCase(200_000, ExpectedResult = 10_000)]
    [TestCase(400_000, ExpectedResult = 10_000)]
    public double WhenFlatValueThenCalculateAmount_New(double amount)
    {
        //Amount must be positive and not 0
        Assert.That(amount, Is.GreaterThan(0));
        if (amount >= 200_000)
        {
            return 10_000;
        }

        return amount * .15;
    }

    [TestCase(2, ExpectedResult = 0.34999999999999998)]
    [TestCase(10_001, ExpectedResult = 1_750.175)]
    [TestCase(200_000, ExpectedResult = 35_000.0)]
    [TestCase(400_000, ExpectedResult = 70_000.0)]
    public double WhenFlatRateThenCalculateAmount(double amount)
    {
        //Amount must be positive and not 0
        Assert.That(amount, Is.GreaterThan(0));
        return amount * .175;
    }
    
    [TestCase(2, ExpectedResult = 0.34999999999999998)]
    [TestCase(10_001, ExpectedResult = 1_750.175)]
    [TestCase(200_000, ExpectedResult = 35_000.0)]
    [TestCase(400_000, ExpectedResult = 70_000.0)]
    public double WhenFlatRateThenCalculateAmount_New(double amount)
    {
        //Amount must be positive and not 0
        Assert.That(amount, Is.GreaterThan(0));
        return TaxExtensions.FlatRateCalculateAmount(amount);
    }
    
    [TestCase(8_350, ExpectedResult = 835)]
    [TestCase(33_950, ExpectedResult = 4_675.0)]
    [TestCase(82_250, ExpectedResult = 18_002.5)]
    [TestCase(171_550, ExpectedResult = 51_494.0)]
    [TestCase(372_950, ExpectedResult = 140_986.0)]
    [TestCase(400_000, ExpectedResult = 207_065.0)]
    public double WhenProgressiveDynamicThenCalculateAmount(double amount)
    {
        //Amount must be positive and not 0
        Assert.That(amount, Is.GreaterThan(0));
        var brackets = new Dictionary<double, double>
        {
            { 8350, .1 },
            { 33950, .15 },
            { 82250, .25 },
            { 171550, .28 },
            { 372950, .33 },
            { int.MaxValue, .35 }
        };
        return TaxExtensions.CalcProgressive(amount, brackets);
    }
    [TestCase(8_350, ExpectedResult = 835)]
    [TestCase(33_950, ExpectedResult = 4_675.0)]
    [TestCase(82_250, ExpectedResult = 18_002.5)]
    [TestCase(171_550, ExpectedResult = 51_494.0)]
    [TestCase(372_950, ExpectedResult = 140_986.0)]
    [TestCase(400_000, ExpectedResult = 207_065.0)]
    public double WhenProgressiveDynamicThenCalculateAmount_New(double amount)
    {
        //Amount must be positive and not 0
        Assert.That(amount, Is.GreaterThan(0));
         return TaxExtensions.ProgressiveCalculateAmount(amount);
    }
    [TestCase("7441",8_350, ExpectedResult = 835)]
    [TestCase("7441",33_950, ExpectedResult = 4_675.0)]
    [TestCase("7441",82_250, ExpectedResult = 18_002.5)]
    [TestCase("7441",171_550, ExpectedResult = 51_494.0)]
    [TestCase("7441",372_950, ExpectedResult = 140_986.0)]
    [TestCase("7441",400_000, ExpectedResult = 207_065.0)]
    [TestCase("1000",8_350, ExpectedResult = 835)]
    [TestCase("1000",33_950, ExpectedResult = 4_675.0)]
    [TestCase("1000",82_250, ExpectedResult = 18_002.5)]
    [TestCase("1000",171_550, ExpectedResult = 51_494.0)]
    [TestCase("1000",372_950, ExpectedResult = 140_986.0)]
    [TestCase("1000",400_000, ExpectedResult = 207_065.0)]
    [TestCase("A100",2, ExpectedResult = 0.3)]
    [TestCase("A100",10_001, ExpectedResult = 1500.1499999999999)]
    [TestCase("A100",200_000, ExpectedResult = 10_000)]
    [TestCase("A100",400_000, ExpectedResult = 10_000)]
    [TestCase("7000",2, ExpectedResult = 0.34999999999999998)]
    [TestCase("7000",10_001, ExpectedResult = 1_750.175)]
    [TestCase("7000",200_000, ExpectedResult = 35_000.0)]
    [TestCase("7000",400_000, ExpectedResult = 70_000.0)]
    public double TaxEndToEndTesting(string postalCode, double amount)
    {
        Assert.That(amount, Is.GreaterThan(0));
        return TaxExtensions.GetTaxAmount(postalCode, amount);
    }
}