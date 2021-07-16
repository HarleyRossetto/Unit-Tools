using System;
using Macquarie.Handbook.Data.Transcript;
using Macquarie.Handbook.Data.Transcript.Facts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitInfo_Tests.Tests.Data.Unit.Transcript.Facts
{
    [TestClass]
    public class TestGradeConverter
    {
        [TestMethod]
        public void TestConvertFromMark() {
            for (uint i = 0; i <= 120; i += 10) {
                var result = GradeConverter.ConvertFromMark(i);

                switch (i) {
                    case >= 85:
                        Assert.AreEqual(EnumGrade.HighDistinction, result);
                        break;
                    case >= 75:
                        Assert.AreEqual(EnumGrade.Distinction, result);
                        break;
                    case >= 65:
                        Assert.AreEqual(EnumGrade.Credit, result);
                        break;
                    case >= 50:
                        Assert.AreEqual(EnumGrade.Pass, result);
                        break;
                    case <= 50:
                        Assert.AreEqual(EnumGrade.Fail, result);
                        break;
                }
            }
        }

        [TestMethod]
        public void TestConvertToMark() {
            Assert.AreEqual(85u, GradeConverter.ConvertToMark(EnumGrade.HighDistinction));
            Assert.AreEqual(75u, GradeConverter.ConvertToMark(EnumGrade.Distinction));
            Assert.AreEqual(65u, GradeConverter.ConvertToMark(EnumGrade.Credit));
            Assert.AreEqual(50u, GradeConverter.ConvertToMark(EnumGrade.Pass));
            Assert.AreEqual(0u, GradeConverter.ConvertToMark(EnumGrade.Fail));
            Assert.AreEqual(0u, GradeConverter.ConvertToMark((EnumGrade)200));
        }

        [TestMethod]
        public void TestGradeToStringConverter() {
            Assert.AreEqual("F", GradeConverter.ConvertToShortString(EnumGrade.Fail));
            Assert.AreEqual("P", GradeConverter.ConvertToShortString(EnumGrade.Pass));
            Assert.AreEqual("Cr", GradeConverter.ConvertToShortString(EnumGrade.Credit));
            Assert.AreEqual("D", GradeConverter.ConvertToShortString(EnumGrade.Distinction));
            Assert.AreEqual("HD", GradeConverter.ConvertToShortString(EnumGrade.HighDistinction));
            Assert.AreEqual("?", GradeConverter.ConvertToShortString((EnumGrade)2));
        }

        [TestMethod]
        public void TestConvertToLongString() {
            Assert.AreEqual("Fail",             GradeConverter.ConvertToLongString(EnumGrade.Fail));
            Assert.AreEqual("Pass",             GradeConverter.ConvertToLongString(EnumGrade.Pass));
            Assert.AreEqual("Credit",           GradeConverter.ConvertToLongString(EnumGrade.Credit));
            Assert.AreEqual("Distinction",      GradeConverter.ConvertToLongString(EnumGrade.Distinction));
            Assert.AreEqual("High Distinction", GradeConverter.ConvertToLongString(EnumGrade.HighDistinction));
            Assert.AreEqual("Unknown",          GradeConverter.ConvertToLongString((EnumGrade)3));
        }

    }
}