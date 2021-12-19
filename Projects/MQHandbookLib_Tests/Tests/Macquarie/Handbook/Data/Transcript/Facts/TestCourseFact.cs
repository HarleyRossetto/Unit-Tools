using System;
using Macquarie.Handbook.Data.Transcript.Facts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Macquarie.Handbook.Data.Transcript.Facts
{
    [TestClass]
    public class TestCourseFact
    {
        [TestMethod]
        public void TestCourseCodeAndNameProperty() {
            CourseFact course = new();
            Assert.AreEqual(String.Empty, course.CourseCode);
            Assert.AreEqual(String.Empty, course.CourseName);

            course = new()
            {
                CourseCode = "N000062",
                CourseName = "Bachelor of Information Technology"
            };

            Assert.AreEqual("N000062", course.CourseCode);
            Assert.AreEqual("Bachelor of Information Technology", course.CourseName);

            course = new()
            {
                CourseCode = null,
                CourseName = null
            };

            Assert.AreEqual(String.Empty, course.CourseCode);
            Assert.AreEqual(String.Empty, course.CourseName);
        }

        [TestMethod]
        public void TestEquals() {
            CourseFact course = new()
            {
                CourseCode = "N000062",
                CourseName = "Bachelor of Information Technology"
            };

            CourseFact courseTwo = null;

            CourseFact courseThree = new()
            {
                CourseCode = "ABCD",
                CourseName = "Letters"
            };

            CourseFact courseFour = new()
            {
                CourseCode = "N000062",
                CourseName = "Bachelor of Information Technology"
            };

            // Null test
            Assert.IsFalse(course.Equals(courseTwo));

            // Not null and of wrong type
            Assert.IsFalse(course.Equals("abcd"));

            // Not null and correct type, but no match
            Assert.IsFalse(course.Equals(courseThree));

            // Not null and correct type, with match
            Assert.IsTrue(course.Equals(courseFour));
        }

        [TestMethod]
        public void TestGetKey() {
            CourseFact course = new()
            {
                CourseCode = "N000062",
                CourseName = "Bachelor of Information Technology"
            };

            Assert.AreEqual("N000062", course.GetKey());

            course = new();

            Assert.AreEqual(String.Empty, course.GetKey());
        }

        [TestMethod]
        public void TestToString() {
            CourseFact course = new()
            {
                CourseCode = "N000062",
                CourseName = "Bachelor of Information Technology"
            };

            Assert.AreEqual("Bachelor of Information Technology", course.ToString());

            course = new();

            Assert.AreEqual(String.Empty, course.ToString());
        }
    }
}