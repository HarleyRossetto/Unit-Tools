using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

using Macquarie.Handbook.Data.Helpers;

namespace UnitTools_Tests
{
    [TestClass]
    public class UnitTestEnrolmentRuleParentheseParser
    {
        [TestMethod]
        public void TestGetRangeStartAndLength() {
            Range range = new Range(5, 25);
            var result = range.GetRangeStartAndLength();
            Assert.AreEqual(5, result.start);
            Assert.AreEqual(21, result.length);
        }
    }
}
