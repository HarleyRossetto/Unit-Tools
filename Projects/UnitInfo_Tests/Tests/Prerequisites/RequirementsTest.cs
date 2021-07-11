using System;
using System.Collections.Generic;
using Macquarie.Handbook.Data.Transcript;
using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Transcript.Facts.Providers;
using Macquarie.Handbook.Data.Unit.Prerequisites.Facts;
using Macquarie.Handbook.Data.Unit.Transcript.Facts;
using Macquarie.Handbook.Data.Unit.Transcript.Facts.Providers;
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
                Facts = new()
                {
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

        [TestMethod]
        public void RequirementMet_UnitTest() {
            // Transcript Facts
            UnitFact comp1000 = new("COMP1000", 95);
            UnitFact comp1350 = new("COMP1350", 84);

            Dictionary<string, ITranscriptFact> transcript = new()
            {
                { comp1000.UnitCode, comp1000 },
                { comp1350.UnitCode, comp1350 }
            };

            ITranscriptFactProvider transcriptProvider = new TranscriptFactDictionaryProvider(transcript);


            // COMP2250 requirements
            var programmingOr = new OrListRequirementFact()
            {
                Facts = new()
                {
                    new UnitRequirementFact(new("COMP1000", 50)),
                    new UnitRequirementFact(new("COMP115", 50)),
                }
            };
            var databaseOr = new OrListRequirementFact()
            {
                Facts = new()
                {
                    new UnitRequirementFact(new("COMP1350", 50)),
                    new UnitRequirementFact(new("ISYS114", 50))
                }
            };
            var parentOr = new OrListRequirementFact()
            {
                Facts = new()
                {
                    programmingOr,
                    databaseOr
                }
            };

            Assert.IsTrue(parentOr.RequirementMet(transcriptProvider));
        }

        [TestMethod]
        public void RequirementMet_CreditPoints() {
            // COMP2050 requirements
            var comp1010Req = new OrListRequirementFact()
            {
                Facts = new()
                {
                    new UnitRequirementFact(new("COMP1010", 50)),
                    new UnitRequirementFact(new("COMP125", 50))
                }
            };
            var requirement = new CreditPointRequirementFact(new(60), new(EnumStudyLevel.Level1000))
            {
                IncludingFact = comp1010Req,
            };

            //Transcript facts
            var comp1010 = new UnitFact("COMP1010", 51, EnumStudyLevel.Level1000);
            var comp1000 = new UnitFact("COMP1000", 50, EnumStudyLevel.Level1000);
            var math1000 = new UnitFact("MATH1000", 50, EnumStudyLevel.Level1000);
            var comp1350 = new UnitFact("COMP1350", 50, EnumStudyLevel.Level1000);
            var stat1170 = new UnitFact("STAT1170", 50, EnumStudyLevel.Level1000);
            var anat1001 = new UnitFact("ANAT1001", 50, EnumStudyLevel.Level1000);

            var dictionary = new Dictionary<string, ITranscriptFact>()
            {
                {comp1010.UnitCode, comp1010},
                {comp1000.UnitCode, comp1000},
                {math1000.UnitCode, math1000},
                {comp1350.UnitCode, comp1350},
                {stat1170.UnitCode, stat1170},
                {anat1001.UnitCode, anat1001},
            };

            ITranscriptFactProvider transcriptProvider = new TranscriptFactDictionaryProvider(dictionary);

            // Check

            Assert.IsTrue(requirement.RequirementMet(transcriptProvider));
            Console.WriteLine(requirement.ToString());

            requirement = new CreditPointRequirementFact(new(40))
            {
                IncludingFact = comp1010Req
            };

            Assert.IsTrue(requirement.RequirementMet(transcriptProvider));
            Console.WriteLine(requirement.ToString());

            requirement = new CreditPointRequirementFact(new(60), new(EnumStudyLevel.Level1000));

            Assert.IsTrue(requirement.RequirementMet(transcriptProvider));
            Console.WriteLine(requirement.ToString());

            requirement = new CreditPointRequirementFact(new(40));

            Assert.IsTrue(requirement.RequirementMet(transcriptProvider));
            Console.WriteLine(requirement.ToString());

        }

        [TestMethod]
        public void CreditPointRequirementStringTest() {
            var transcriptFacts = new CreditPointRequirementFact(new(40), new StudyLevelDescriptor(Macquarie.Handbook.Data.Unit.Transcript.Facts.EnumStudyLevel.Level3000, true));
            string expected = "40cp at 3000 level or above";
            Assert.AreEqual(expected, transcriptFacts.ToString());

            var comp1010Req = new OrListRequirementFact()
            {
                Facts = new()
                {
                    new UnitRequirementFact(new("COMP1010", 50)),
                    new UnitRequirementFact(new("COMP125", 50))
                }
            };
            transcriptFacts = new CreditPointRequirementFact(new(60), new(EnumStudyLevel.Level1000, true))
            {
                IncludingFact = comp1010Req,
            };

            expected = "60cp at 1000 level or above including (COMP1010 (P) or COMP125 (P))";
            Assert.AreEqual(expected, transcriptFacts.ToString());
        }

        [TestMethod]
        public void CreditPointRequirementUnitGroupTest() {
            var requirementFact = new CreditPointRequirementFact(new(10), new StudyLevelDescriptor(EnumStudyLevel.Level2000, false), new UnitGroupRequirementFact("LING"));
            string expected = "10cp in LING units at 2000 level";

            var randomLingUnit = new UnitFact("LING2000", 60, EnumStudyLevel.Level2000);
            var randomLingUnitTwo = new UnitFact("LING2010", 76, EnumStudyLevel.Level2000);
            var transcript = new Dictionary<string, ITranscriptFact>() {
                {randomLingUnit.UnitCode, randomLingUnit},
                {randomLingUnitTwo.UnitCode, randomLingUnitTwo}
            };

            Assert.AreEqual(expected, requirementFact.ToString());

            Assert.IsTrue(requirementFact.RequirementMet(new TranscriptFactDictionaryProvider(transcript)));


            var unitGroupRequirement = new OrListRequirementFact()
            {
                Facts = new()
                {
                    new UnitGroupRequirementFact("LING"),
                    new UnitGroupRequirementFact("COMP")
                }
            };
            requirementFact = new CreditPointRequirementFact(new(20), new StudyLevelDescriptor(EnumStudyLevel.Level2000, false), unitGroupRequirement);
            expected = "20cp in (LING or COMP) units at 2000 level";

            Assert.AreEqual(expected, requirementFact.ToString());

            Assert.IsTrue(requirementFact.RequirementMet(new TranscriptFactDictionaryProvider(transcript)));

            Assert.IsFalse(requirementFact.RequirementMet(null));

            transcript = new Dictionary<string, ITranscriptFact>() {
                {randomLingUnit.UnitCode, randomLingUnit}
            };

            Assert.IsFalse(requirementFact.RequirementMet(new TranscriptFactDictionaryProvider(transcript)));

            transcript = new Dictionary<string, ITranscriptFact>() {
                {"COMP1010", null}
            };

            Assert.IsFalse(requirementFact.RequirementMet(new TranscriptFactDictionaryProvider(transcript)));
        }

        [TestMethod]
        public void CreditPointRequirement_ComplexTest() {
            string expected = "50cp at 2000 level or above including 10cp in LING units at 2000 level";

            var includingReq = new CreditPointRequirementFact(new(10), new StudyLevelDescriptor(EnumStudyLevel.Level2000, false), new UnitGroupRequirementFact("LING"));
            var requirement = new CreditPointRequirementFact(new(50), new StudyLevelDescriptor(EnumStudyLevel.Level2000, true))
            {
                IncludingFact = includingReq
            };

            Assert.AreEqual(expected, requirement.ToString());

            var transcript = new Dictionary<string, ITranscriptFact>() {
                {"LING2010", new UnitFact("LING2010", 51, EnumStudyLevel.Level2000)},
                {"LING2000", new UnitFact("LING2000", 50, EnumStudyLevel.Level2000)},
                {"MATH3000", new UnitFact("MATH3000", 53, EnumStudyLevel.Level3000)},
                {"COMP4050", new UnitFact("COMP4050", 50, EnumStudyLevel.Level4000)},
                {"STAT1170", new UnitFact("STAT1170", 50, EnumStudyLevel.Level1000)},
                {"ANAT2000", new UnitFact("ANAT2000", 50, EnumStudyLevel.Level2000)}
            };

            Assert.IsTrue(requirement.RequirementMet(new TranscriptFactDictionaryProvider(transcript)));
        }
    }
}