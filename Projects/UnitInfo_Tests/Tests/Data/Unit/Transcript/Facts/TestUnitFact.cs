using System;
using Macquarie.Handbook.Data.Transcript;
using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Unit.Transcript.Facts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitInfo_Tests.Tests.Data.Unit.Transcript.Facts
{
    [TestClass]
    public class TestUnitFact
    {
        readonly UnitFact a = new()
        {
            UnitCode = "COMP1000",
            Results = new(57, null)
        };

        readonly UnitFact b = new()
        {
            UnitCode = "COMP1000",
            Results = new(57, null)
        };

        readonly UnitFact c = new()
        {
            UnitCode = "COMP1000",
            Results = new(0, EnumGrade.Fail)
        };

        readonly UnitFact d = new()
        {
            UnitCode = "MATH1010",
            Results = new(89, null)
        };

        /// <summary>
        /// Establishes that UnitFact can correctly determine the SubjectCodeHeader
        /// and Study level based off regex parsing of input UnitCode.
        /// </summary>
        [TestMethod]
        public void TestCreationUnitCodeProperty() {
            // Valid input parameters
            UnitFact validUnitFact = new()
            {
                UnitCode = "Comp1010"
            };

            Assert.AreEqual("COMP1010", validUnitFact.UnitCode);
            Assert.AreEqual("COMP", validUnitFact.SubjectCodeHeader);
            Assert.AreEqual(EnumStudyLevel.Level1000, validUnitFact.StudyLevel);

            // Invalid Input Parameters
            // UnitCode is wrong format
            // Study level cannot be determined, so we default to NoLevel
            UnitFact invalidUnitFact = new()
            {
                UnitCode = "Comp"
            };

            Assert.AreEqual(string.Empty, invalidUnitFact.UnitCode);
            Assert.AreEqual(string.Empty, invalidUnitFact.SubjectCodeHeader);
            Assert.AreEqual(EnumStudyLevel.NoLevel, invalidUnitFact.StudyLevel);
        }

        [TestMethod]
        public void TestCreationStudyLevelProperty() {
            UnitFact fact;
            for (uint i = 1; i <= 8; i++) {
                uint levelCode = i * 1000u;
                fact = new()
                {
                    StudyLevel = StudyLevelConverter.FromInt(levelCode)
                };

                Assert.AreEqual((i == 5) ? EnumStudyLevel.NoLevel : StudyLevelConverter.FromInt(levelCode), fact.StudyLevel);
            }

            fact = new()
            {
                UnitCode = "COMP0000"
            };

            Assert.AreEqual(EnumStudyLevel.NoLevel, fact.StudyLevel);
        }

        [TestMethod]
        public void TestMarksProperty() {
            UnitFact fact;
            for (uint i = 0; i < 110; i += 10) {
                fact = new()
                {
                    Results = new(i, null)
                };

                switch (i) {
                    case > 100:
                        Assert.AreEqual(100, fact.Marks);
                        break;
                    default:
                        Assert.AreEqual(i, fact.Marks);
                        break;
                }
            }
        }

        [TestMethod]
        public void TestResultsProperty() {
            //Marks valid, grade null
            const uint MARKS = 57;

            UnitFact fact = new()
            {
                UnitCode = "COMP1010",
                Results = new(MARKS, null)
            };

            Assert.AreEqual(EnumGrade.Pass, fact.Grade);
            Assert.AreEqual(MARKS, fact.Marks);
            Assert.AreEqual(new(MARKS, EnumGrade.Pass), fact.Results);

            //null marks, Grade valid
            var grade = EnumGrade.Distinction;
            fact = new()
            {
                UnitCode = "COMP1010",
                Results = new(null, grade)
            };

            Assert.AreEqual(grade, fact.Grade);
            Assert.AreEqual(new(75, grade), fact.Results);

            //Double null case
            fact = new()
            {
                UnitCode = "COMP1010",
                Results = new(null, null)
            };

            Assert.AreEqual(0u, fact.Marks);
            Assert.AreEqual(EnumGrade.Fail, fact.Grade);
            Assert.AreEqual(new(0, EnumGrade.Fail), fact.Results);
        }

        [TestMethod]
        public void TestEquals() {
            //Null
            Assert.IsFalse(a.Equals(null));

            //Incorrect type
            Assert.IsFalse(a.Equals(new DateTime()));

            //Valid match
            Assert.IsTrue(a.Equals(b));

            //Invalid match
            Assert.IsFalse(a.Equals(c));

            //Invalid match
            Assert.IsFalse(a.Equals(d));

            // Equals this
            Assert.IsFalse(a.Equals(a));
        }

        [TestMethod]
        public void TestGetHashCode() {
            Assert.IsTrue(a.GetHashCode() == b.GetHashCode());

            Assert.IsFalse(a.GetHashCode() == d.GetHashCode());
        }
    }
}