using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

using Macquarie.Handbook.Helpers.Extensions;

namespace Test.Macquarie.Handbook.Helpers.Prerequisites;

[TestClass]
public class UnitTestEnrolmentRuleParentheseParser
{
    [TestMethod]
    public void TestGetRangeStartAndLength() {
        Range range = new(5, 25);
        var (start, length) = range.GetRangeStartAndLength();
        Assert.AreEqual(5, start);
        Assert.AreEqual(21, length);
    }
}
