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
            Assert.AreEqual(1000,   StudyLevelConverter.ToInt(EnumStudyLevel.Level1000));
            Assert.AreEqual(2000,   StudyLevelConverter.ToInt(EnumStudyLevel.Level2000));
            Assert.AreEqual(3000,   StudyLevelConverter.ToInt(EnumStudyLevel.Level3000));
            Assert.AreEqual(4000,   StudyLevelConverter.ToInt(EnumStudyLevel.Level4000));
            Assert.AreEqual(6000,   StudyLevelConverter.ToInt(EnumStudyLevel.Level6000));
            Assert.AreEqual(7000,   StudyLevelConverter.ToInt(EnumStudyLevel.Level7000));
            Assert.AreEqual(8000,   StudyLevelConverter.ToInt(EnumStudyLevel.Level8000));
            Assert.AreEqual(0,      StudyLevelConverter.ToInt(EnumStudyLevel.NoLevel));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
                StudyLevelConverter.ToInt((EnumStudyLevel)5000);
            });
        }

        [TestMethod]
        public void Test() {
            
        }
    }
}