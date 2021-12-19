using Macquarie.Handbook.Data.Transcript.Facts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Macquarie.Handbook.Data.Unit.Prerequisites.Facts;

[TestClass]
public class TestUnitRequirementFact
{
    UnitFact unit = new() {
        UnitCode = "COMP1010",
        Results = new(57, null)
    };

    [TestInitialize]
    public void Initialise() {
    }

    // [TestMethod]
    // public void Test() {

    // }
}
