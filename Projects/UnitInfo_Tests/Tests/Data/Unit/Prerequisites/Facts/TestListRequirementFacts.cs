using System;
using System.Collections.Generic;
using Macquarie.Handbook.Data.Transcript;
using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Unit.Prerequisites.Facts;
using Macquarie.Handbook.Data.Unit.Transcript.Facts.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unit_Info.src.Macquarie.Handbook.Data.Transcript.Facts.Providers;

namespace UnitInfo_Tests.Tests.Data.Unit.Prerequisites.Facts
{
    [TestClass]
    public class TestListRequirementFacts
    {

        [TestMethod]
        public void TestAndList() {
            IRequirementFact andList;

            //Null list
            andList = new AndListRequirementFact<IRequirementFact>();

            Assert.IsTrue(andList.RequirementMet(null));
            Assert.IsTrue(andList.RequirementMet(new TranscriptFactSingleUnitProvider(new UnitFact())));

            //Empty list
            andList = new AndListRequirementFact<IRequirementFact>()
            {
                Facts = new List<IRequirementFact>()
            };

            Assert.IsTrue(andList.RequirementMet(null));
            Assert.IsTrue(andList.RequirementMet(new TranscriptFactSingleUnitProvider(new UnitFact())));
            Assert.AreEqual(String.Empty, andList.ToString());

            //Contains Fact
            UnitFact fact = new()
            {
                UnitCode = "COMP1000",
                Results = new(58, null)
            };

            andList = new AndListRequirementFact<IRequirementFact>()
            {
                Facts = new List<IRequirementFact>() {
                    { new UnitRequirementFact(fact) }
                }
            };

            Assert.IsFalse(andList.RequirementMet(null));
            Assert.IsFalse(andList.RequirementMet(new TranscriptFactSingleUnitProvider(new UnitFact())));
            Assert.IsTrue(andList.RequirementMet(new TranscriptFactSingleUnitProvider(fact)));
            Assert.AreEqual("COMP1000 (P)", andList.ToString());

            UnitFact fact2 = new()
            {
                UnitCode = "COMP2250",
                Results = new(null, EnumGrade.Pass)
            };

            andList = new AndListRequirementFact<IRequirementFact>()
            {
                Facts = new List<IRequirementFact>() {
                    {new UnitRequirementFact(fact)},
                    {new UnitRequirementFact(fact2)}
                }
            };

            Assert.IsFalse(andList.RequirementMet(null));
            Assert.IsFalse(andList.RequirementMet(new TranscriptFactSingleUnitProvider(new UnitFact())));
            Assert.IsFalse(andList.RequirementMet(new TranscriptFactSingleUnitProvider(fact)));

            TranscriptFactDictionaryProvider factProvider = new(
                new Dictionary<string, ITranscriptFact>() {
                    {fact.UnitCode, fact},
                    {fact2.UnitCode, fact2},
                }
            );
            Assert.IsTrue(andList.RequirementMet(factProvider));

            Assert.AreEqual("(COMP1000 (P) and COMP2250 (P))", andList.ToString());

        }

        [TestMethod]
        public void TestOrList() {
            IRequirementFact orList;

            //Null list
            orList = new OrListRequirementFact<IRequirementFact>();

            Assert.IsTrue(orList.RequirementMet(null));
            Assert.IsTrue(orList.RequirementMet(new TranscriptFactSingleUnitProvider(new UnitFact())));

            //Empty list
            orList = new OrListRequirementFact<IRequirementFact>()
            {
                Facts = new List<IRequirementFact>()
            };

            Assert.IsTrue(orList.RequirementMet(null));
            Assert.IsTrue(orList.RequirementMet(new TranscriptFactSingleUnitProvider(new UnitFact())));

            //Contains Fact
            UnitFact fact = new()
            {
                UnitCode = "COMP1000",
                Results = new(58, null)
            };

            orList = new OrListRequirementFact<IRequirementFact>()
            {
                Facts = new List<IRequirementFact>() {
                    { new UnitRequirementFact(fact) }
                }
            };

            Assert.IsFalse(orList.RequirementMet(null));
            Assert.IsFalse(orList.RequirementMet(new TranscriptFactSingleUnitProvider(new UnitFact())));
            Assert.IsTrue(orList.RequirementMet(new TranscriptFactSingleUnitProvider(fact)));

            UnitFact fact2 = new()
            {
                UnitCode = "COMP2250",
                Results = new(null, EnumGrade.Pass)
            };

            orList = new OrListRequirementFact<IRequirementFact>()
            {
                Facts = new List<IRequirementFact>() {
                    {new UnitRequirementFact(fact)},
                    {new UnitRequirementFact(fact2)}
                }
            };

            Assert.IsFalse(orList.RequirementMet(null));
            Assert.IsFalse(orList.RequirementMet(new TranscriptFactSingleUnitProvider(new UnitFact())));
            Assert.IsTrue(orList.RequirementMet(new TranscriptFactSingleUnitProvider(fact)));

            TranscriptFactDictionaryProvider factProvider = new(
                new Dictionary<string, ITranscriptFact>() {
                    {fact.UnitCode, fact},
                    {fact2.UnitCode, fact2},
                }
            );
            Assert.IsTrue(orList.RequirementMet(factProvider));

            Assert.AreEqual("(COMP1000 (P) or COMP2250 (P))", orList.ToString());
        }
    }
}