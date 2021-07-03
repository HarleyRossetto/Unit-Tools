using System;
using Macquarie.Handbook.Data.Transcript;
using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Unit.Prerequisites.Facts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitInfo_Tests.Tests
{
    [TestClass]
    public class RequirementsTest
    {

        [TestMethod]
        public void UnitRequirementFactTests() {
            //Basic Fact Tests
            IRequirementFact singleUnit = new UnitRequirementFact(new UnitFact("COMP1000", 0));
            string expectedString = "COMP1000 (F)";
            string result = singleUnit.ToString();
            Assert.AreEqual(expectedString, result);

            singleUnit = new UnitRequirementFact(new UnitFact("COMP1000", 50));
            expectedString = "COMP1000 (P)";
            Assert.AreEqual(expectedString, singleUnit.ToString());

            singleUnit = new UnitRequirementFact(new UnitFact("COMP1000", 65));
            expectedString = "COMP1000 (Cr)";
            Assert.AreEqual(expectedString, singleUnit.ToString());

            singleUnit = new UnitRequirementFact(new("COMP1000", 75));
            expectedString = "COMP1000 (D)";
            Assert.AreEqual(expectedString, singleUnit.ToString());

            singleUnit = new UnitRequirementFact(new("COMP1000", EnumGrade.HighDistinction));
            expectedString = "COMP1000 (HD)";
            Assert.AreEqual(expectedString, singleUnit.ToString());

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new UnitRequirementFact(new UnitFact("COMP1000", (EnumGrade)2)));

            //Basic Unit Facts, marks out of range
            IRequirementFact factLowMark = new UnitRequirementFact(new UnitFact("COMP1010", -10));
            expectedString = "COMP1010 (F)";
            Assert.AreEqual(expectedString, factLowMark.ToString());

            IRequirementFact factHighMark = new UnitRequirementFact(new UnitFact("COMP1010", 201));
            expectedString = "COMP1010 (HD)";
            Assert.AreEqual(expectedString, factHighMark.ToString());
        }

        [TestMethod]
        public void CourseFactTests() {
            Assert.ThrowsException<NullReferenceException>(() => new CourseFact(null));
            Assert.ThrowsException<NullReferenceException>(() => new CourseFact(""));
            Assert.ThrowsException<NullReferenceException>(() => new CourseFact("\n\t\n      \r\n"));
        }

        [TestMethod]
        public void CourseRequirementFactTests() {
            IRequirementFact courseFact = new CourseRequirementFact(new("C000006"));
            Assert.AreEqual("C000006", courseFact.ToString());
        }

        [TestMethod]
        public void GradeToStringConverterTest() {
            Assert.AreEqual("F", GradeConverter.ConvertToShortString(EnumGrade.Fail));
            Assert.AreEqual("P", GradeConverter.ConvertToShortString(EnumGrade.Pass));
            Assert.AreEqual("Cr", GradeConverter.ConvertToShortString(EnumGrade.Credit));
            Assert.AreEqual("D", GradeConverter.ConvertToShortString(EnumGrade.Distinction));
            Assert.AreEqual("HD", GradeConverter.ConvertToShortString(EnumGrade.HighDistinction));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => GradeConverter.ConvertToShortString((EnumGrade)2));
        }

        [TestMethod]
        public void ComplexRequirementChainTest() {
            var statReqFacts = new OrListRequirementFact()
            {
                Facts = new() {
                    new UnitRequirementFact(new UnitFact("STAT171", EnumGrade.Pass)),
                    new UnitRequirementFact(new UnitFact("SAT1371", EnumGrade.Pass)),
                }
            };

            var mathReqFacts = new OrListRequirementFact()
            {
                Facts = new()
                {
                    new UnitRequirementFact(new("MATH1020", EnumGrade.Pass)),
                    new UnitRequirementFact(new("MATH1025", EnumGrade.Pass)),
                    new UnitRequirementFact(new("MATH133", EnumGrade.Pass)),
                    new UnitRequirementFact(new("MATH135", EnumGrade.Pass)),
                }
            };

            var topOrList = new OrListRequirementFact()
            {
                Facts = new()
                {
                    statReqFacts,
                    mathReqFacts
                }
            };

            Console.WriteLine(topOrList.ToString());
        }

    }
}