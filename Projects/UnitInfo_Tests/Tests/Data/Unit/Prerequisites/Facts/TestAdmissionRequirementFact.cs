using System;
using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Unit.Prerequisites.Facts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitInfo_Tests.Tests.Data.Unit.Prerequisites.Facts
{
    [TestClass]
    public class TestAdmissionRequirementFact
    {
        private IRequirementFact validFact;
        private IRequirementFact invalidFact;

        [TestInitialize]
        public void Initialise() {
            validFact = new CourseRequirementFact(new CourseFact("MEng"));
            invalidFact = new UnitRequirementFact(new UnitFact(){ UnitCode = "COMP1000", Results = new(50, null)});
        }

        [TestMethod]
        public void TestFactPropertyAssignment() {

            AdmissionRequirementFact admissionFact = new()
            {
                Fact = validFact
            };

            Assert.ReferenceEquals(validFact, admissionFact.Fact);

            Assert.ThrowsException<ArgumentException>(() => {
                admissionFact = new()
                {
                    Fact = invalidFact
                };
            });
        }

        [TestMethod]
        public void TestFactToString() {
            string expected = "Admission to MEng";

            AdmissionRequirementFact admissionFact = new()
            {
                Fact = validFact
            };

            Assert.AreEqual(expected, admissionFact.ToString());
        }
    }
}