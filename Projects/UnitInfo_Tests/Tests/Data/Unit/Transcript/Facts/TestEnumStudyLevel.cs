using System;
using Macquarie.Handbook.Data.Unit.Transcript.Facts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitInfo_Tests.Tests.Data.Unit.Transcript.Facts
{
    [TestClass]
    public class TestEnumStudyLevel
    {

        [TestInitialize]
        public void Initialise() {

        }

        [TestMethod]
        public void TestConverterToInt() {
            Assert.AreEqual(1000u,   StudyLevelConverter.ToInt(EnumStudyLevel.Level1000));
            Assert.AreEqual(2000u,   StudyLevelConverter.ToInt(EnumStudyLevel.Level2000));
            Assert.AreEqual(3000u,   StudyLevelConverter.ToInt(EnumStudyLevel.Level3000));
            Assert.AreEqual(4000u,   StudyLevelConverter.ToInt(EnumStudyLevel.Level4000));
            Assert.AreEqual(6000u,   StudyLevelConverter.ToInt(EnumStudyLevel.Level6000));
            Assert.AreEqual(7000u,   StudyLevelConverter.ToInt(EnumStudyLevel.Level7000));
            Assert.AreEqual(8000u,   StudyLevelConverter.ToInt(EnumStudyLevel.Level8000));
            Assert.AreEqual(0u,      StudyLevelConverter.ToInt(EnumStudyLevel.NoLevel));

            Assert.AreEqual(0u,     StudyLevelConverter.ToInt((EnumStudyLevel)10000u));
            Assert.AreEqual(0u,     StudyLevelConverter.ToInt((EnumStudyLevel)5u));

        }

        [TestMethod]
        public void TestConverterFromInt() {
            Assert.AreEqual(EnumStudyLevel.Level1000, StudyLevelConverter.FromInt(1000u));
            Assert.AreEqual(EnumStudyLevel.Level2000, StudyLevelConverter.FromInt(2000u));
            Assert.AreEqual(EnumStudyLevel.Level3000, StudyLevelConverter.FromInt(3000u));
            Assert.AreEqual(EnumStudyLevel.Level4000, StudyLevelConverter.FromInt(4000u));
            Assert.AreEqual(EnumStudyLevel.Level6000, StudyLevelConverter.FromInt(6000u));
            Assert.AreEqual(EnumStudyLevel.Level7000, StudyLevelConverter.FromInt(7000u));
            Assert.AreEqual(EnumStudyLevel.Level8000, StudyLevelConverter.FromInt(8000u));
            Assert.AreEqual(EnumStudyLevel.NoLevel, StudyLevelConverter.FromInt(0u));


            Assert.AreEqual(EnumStudyLevel.NoLevel, StudyLevelConverter.FromInt(10000u));
            Assert.AreEqual(EnumStudyLevel.NoLevel, StudyLevelConverter.FromInt(5u));
        }

        [TestMethod]
        public void TestConverterToString() {
            Assert.AreEqual("1000", StudyLevelConverter.ToString(EnumStudyLevel.Level1000));
            Assert.AreEqual("2000", StudyLevelConverter.ToString(EnumStudyLevel.Level2000));
            Assert.AreEqual("3000", StudyLevelConverter.ToString(EnumStudyLevel.Level3000));
            Assert.AreEqual("4000", StudyLevelConverter.ToString(EnumStudyLevel.Level4000));
            Assert.AreEqual("6000", StudyLevelConverter.ToString(EnumStudyLevel.Level6000));
            Assert.AreEqual("7000", StudyLevelConverter.ToString(EnumStudyLevel.Level7000));
            Assert.AreEqual("8000", StudyLevelConverter.ToString(EnumStudyLevel.Level8000));
            Assert.AreEqual("0",    StudyLevelConverter.ToString(EnumStudyLevel.NoLevel));


            Assert.AreEqual("0", StudyLevelConverter.ToString((EnumStudyLevel)10000u));
            Assert.AreEqual("0", StudyLevelConverter.ToString((EnumStudyLevel)5u));
        }
    }
}