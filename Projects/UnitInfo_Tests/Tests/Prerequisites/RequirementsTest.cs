using Microsoft.VisualStudio.TestTools.UnitTesting;

using Macquarie.Handbook.Data.Unit.Prerequisites;
using System;

namespace UnitInfo_Tests.Tests
{
    [TestClass]
    public class RequirementsTest
    {

        [TestMethod]
        public void RequirementChainBuilder() {
            //Basic Fact Tests
            IRequirementFact singleUnit = new RequirementUnit(new TranscriptUnitFact("COMP1000", 0));
            string expectedString = "COMP1000 (F)";
            Assert.AreEqual(expectedString, singleUnit.ToString());

            singleUnit = new RequirementUnit(new TranscriptUnitFact("COMP1000", 50));
            expectedString = "COMP1000 (P)";
            Assert.AreEqual(expectedString, singleUnit.ToString());

            singleUnit = new RequirementUnit(new TranscriptUnitFact("COMP1000", 65));
            expectedString = "COMP1000 (Cr)";
            Assert.AreEqual(expectedString, singleUnit.ToString());

            singleUnit = new RequirementUnit("COMP1000", 75);
            expectedString = "COMP1000 (D)";
            Assert.AreEqual(expectedString, singleUnit.ToString());

            singleUnit = new RequirementUnit("COMP1000", Grade.HighDistinction);
            expectedString = "COMP1000 (HD)";
            Assert.AreEqual(expectedString, singleUnit.ToString());

            singleUnit = new RequirementUnit(new TranscriptUnitFact("COMP1000", (Grade)2));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => singleUnit.ToString());


            //Basic Unit Facts, marks out of range
            IRequirementFact factLowMark = new RequirementUnit(new TranscriptUnitFact("COMP1010", -10));
            expectedString = "COMP1010 (F)";
            Assert.AreEqual(expectedString, factLowMark.ToString());

            IRequirementFact factHighMark = new RequirementUnit(new TranscriptUnitFact("COMP1010", 201));
            expectedString = "COMP1010 (HD)";
            Assert.AreEqual(expectedString, factHighMark.ToString());
        }

        [TestMethod]
        public void GradeToStringConverterTest() {
            Assert.AreEqual("F",    GradeToStringCoverter.Convert(Grade.Fail));
            Assert.AreEqual("P",    GradeToStringCoverter.Convert(Grade.Pass));
            Assert.AreEqual("C",    GradeToStringCoverter.Convert(Grade.Credit));
            Assert.AreEqual("D",    GradeToStringCoverter.Convert(Grade.Distinction));
            Assert.AreEqual("HD",   GradeToStringCoverter.Convert(Grade.HighDistinction));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => GradeToStringCoverter.Convert((Grade)2));
        }

    }
}