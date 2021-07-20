using System;
using System.Collections.Generic;
using Macquarie.Handbook.Data.Transcript;
using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Transcript.Facts.Providers;
using Macquarie.Handbook.Data.Unit.Prerequisites.Facts;
using Macquarie.Handbook.Data.Unit.Transcript.Facts;
using Macquarie.Handbook.Data.Unit.Transcript.Facts.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    [TestClass]
    public class RequirementsTest
    {

        [TestMethod]
        public void UnitRequirementFactTests() {
            //Basic Fact Tests
            IRequirementFact singleUnit = new UnitRequirementFact(new UnitFact() { UnitCode = "COMP1000", Results = new(0, null) });
            string expectedString = "COMP1000 (F)";
            string result = singleUnit.ToString();
            Assert.AreEqual(expectedString, result);

            singleUnit = new UnitRequirementFact(new UnitFact() { UnitCode = "COMP1000", Results = new(50, null) });
            expectedString = "COMP1000 (P)";
            Assert.AreEqual(expectedString, singleUnit.ToString());

            singleUnit = new UnitRequirementFact(new UnitFact() { UnitCode = "COMP1000", Results = new(65, null) });
            expectedString = "COMP1000 (Cr)";
            Assert.AreEqual(expectedString, singleUnit.ToString());

            singleUnit = new UnitRequirementFact(new UnitFact() { UnitCode = "COMP1000", Results = new(75, null) });
            expectedString = "COMP1000 (D)";
            Assert.AreEqual(expectedString, singleUnit.ToString());

            singleUnit = new UnitRequirementFact(new UnitFact() { UnitCode = "COMP1000", Results = new(null, EnumGrade.HighDistinction) });
            expectedString = "COMP1000 (HD)";
            Assert.AreEqual(expectedString, singleUnit.ToString());

            //Basic Unit Facts, marks out of range
            IRequirementFact factLowMark = new UnitRequirementFact(new UnitFact() { UnitCode = "COMP1010", Results = new(10, null) });
            expectedString = "COMP1010 (F)";
            Assert.AreEqual(expectedString, factLowMark.ToString());

            IRequirementFact factHighMark = new UnitRequirementFact(new UnitFact() { UnitCode = "COMP1010", Results = new(201, null) });
            expectedString = "COMP1010 (HD)";
            Assert.AreEqual(expectedString, factHighMark.ToString());
        }
       

        [TestMethod]
        public void ComplexRequirementChainTest() {
            var statReqFacts = new OrListRequirementFact<IRequirementFact>()
            {
                Facts = new()
                {
                    new UnitRequirementFact(new UnitFact() { UnitCode = "STAT171", Results = new(0, EnumGrade.Pass) }),
                    new UnitRequirementFact(new UnitFact() { UnitCode = "STAT1371", Results = new(0, EnumGrade.Pass) }),
                }
            };

            var mathReqFacts = new OrListRequirementFact<IRequirementFact>()
            {
                Facts = new()
                {
                    new UnitRequirementFact(new() { UnitCode = "MATH1020", Results = new(0, EnumGrade.Pass)}),
                    new UnitRequirementFact(new() { UnitCode = "MATH1025", Results = new(0, EnumGrade.Pass)}),
                    new UnitRequirementFact(new() { UnitCode = "MATH133", Results = new(0, EnumGrade.Pass)}),
                    new UnitRequirementFact(new() { UnitCode = "MATH135", Results = new(0, EnumGrade.Pass)}),
                }
            };

            var topOrList = new OrListRequirementFact<IRequirementFact>()
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
            UnitFact comp1000 = new() { UnitCode = "COMP1000", Results = new(95, null) };
            UnitFact comp1350 = new() { UnitCode = "COMP1350", Results = new(84, null) };

            Dictionary<string, ITranscriptFact> transcript = new()
            {
                { comp1000.UnitCode, comp1000 },
                { comp1350.UnitCode, comp1350 }
            };

            ITranscriptFactProvider transcriptProvider = new TranscriptFactDictionaryProvider(transcript);


            // COMP2250 requirements
            var programmingOr = new OrListRequirementFact<IRequirementFact>()
            {
                Facts = new()
                {
                    new UnitRequirementFact(new() { UnitCode = "COMP1000",  Results = new(50, null) }),
                    new UnitRequirementFact(new() { UnitCode = "COMP115",   Results = new(50, null) }),
                }
            };
            var databaseOr = new OrListRequirementFact<IRequirementFact>()
            {
                Facts = new()
                {
                    new UnitRequirementFact(new() { UnitCode = "COMP1350",  Results = new(50, null) }),
                    new UnitRequirementFact(new() { UnitCode = "ISYS114",   Results = new(50, null) })
                }
            };
            var parentOr = new OrListRequirementFact<IRequirementFact>()
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
            var comp1010Req = new OrListRequirementFact<IRequirementFact>()
            {
                Facts = new()
                {
                    new UnitRequirementFact(new() { UnitCode = "COMP1010", Results = new(50, null)}),
                    new UnitRequirementFact(new() { UnitCode = "COMP125", Results = new(50, null)})
                }
            };
            var requirement = new CreditPointRequirementFact(new(60), new(EnumStudyLevel.Level1000))
            {
                IncludingFact = comp1010Req,
            };

            //Transcript facts
            var comp1010 = new UnitFact() { UnitCode = "COMP1010", Results = new(51, null) };
            var comp1000 = new UnitFact() { UnitCode = "COMP1000", Results = new(50, null) };
            var math1000 = new UnitFact() { UnitCode = "MATH1000", Results = new(50, null) };
            var comp1350 = new UnitFact() { UnitCode = "COMP1350", Results = new(50, null) };
            var stat1170 = new UnitFact() { UnitCode = "STAT1170", Results = new(50, null) };
            var anat1001 = new UnitFact() { UnitCode = "ANAT1001", Results = new(50, null) };

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
            var transcriptFacts = new CreditPointRequirementFact(new(40), new StudyLevelDescriptor(EnumStudyLevel.Level3000, true));
            string expected = "40cp at 3000 level or above";
            Assert.AreEqual(expected, transcriptFacts.ToString());

            var comp1010Req = new OrListRequirementFact<IRequirementFact>()
            {
                Facts = new()
                {
                    new UnitRequirementFact(new() { UnitCode = "COMP1010", Results = new(50, null) }),
                    new UnitRequirementFact(new() { UnitCode = "COMP125", Results = new(50, null) })
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
            var requirementFact = new CreditPointRequirementFact(new(10), new StudyLevelDescriptor(EnumStudyLevel.Level2000, false), new OrListRequirementFact<UnitGroupRequirementFact>() { Facts = new(){ new UnitGroupRequirementFact("LING") }});
            string expected = "10cp in LING units at 2000 level";

            var randomLingUnit = new UnitFact() { UnitCode = "LING2000", Results = new(60, null) };
            var randomLingUnitTwo = new UnitFact() { UnitCode = "LING2010", Results = new(76, null) };
            var transcript = new Dictionary<string, ITranscriptFact>() {
                {randomLingUnit.UnitCode, randomLingUnit},
                {randomLingUnitTwo.UnitCode, randomLingUnitTwo}
            };

            Assert.AreEqual(expected, requirementFact.ToString());

            Assert.IsTrue(requirementFact.RequirementMet(new TranscriptFactDictionaryProvider(transcript)));


            var unitGroupRequirement = new OrListRequirementFact<UnitGroupRequirementFact>()
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

            var includingReq = new CreditPointRequirementFact(new(10), new StudyLevelDescriptor(EnumStudyLevel.Level2000, false), new OrListRequirementFact<UnitGroupRequirementFact>() { Facts = new() {new UnitGroupRequirementFact("LING")}});
            var requirement = new CreditPointRequirementFact(new(50), new StudyLevelDescriptor(EnumStudyLevel.Level2000, true))
            {
                IncludingFact = includingReq
            };

            Assert.AreEqual(expected, requirement.ToString());

            var transcript = new Dictionary<string, ITranscriptFact>() {
                {"LING2010", new UnitFact() { UnitCode = "LING2010", Results = new(51, null)}},
                {"LING2000", new UnitFact() { UnitCode = "LING2000", Results = new(50, null)}},
                {"MATH3000", new UnitFact() { UnitCode = "MATH3000", Results = new(53, null)}},
                {"COMP4050", new UnitFact() { UnitCode = "COMP4050", Results = new(50, null)}},
                {"STAT1170", new UnitFact() { UnitCode = "STAT1170", Results = new(50, null)}},
                {"ANAT2000", new UnitFact() { UnitCode = "ANAT2000", Results = new(50, null)}},
            };

            Assert.IsTrue(requirement.RequirementMet(new TranscriptFactDictionaryProvider(transcript)));
        }
    }
}