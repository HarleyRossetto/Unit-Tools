using Macquarie.Handbook.Data.Transcript.Facts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitInfo_Tests.Tests.Data.Unit.Prerequisites.Facts
{
    [TestClass]
    public class TestUnitRequirementFact
    {
        UnitFact unit = new("COMP1010", 78);

        [TestInitialize]
        public void Initialise() {
        }

        [TestMethod]
        public void Test() {

        }
    }
}