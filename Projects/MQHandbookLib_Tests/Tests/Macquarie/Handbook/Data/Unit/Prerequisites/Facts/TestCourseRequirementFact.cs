using System.Collections.Generic;
using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Transcript.Facts.Providers;
using Macquarie.Handbook.Data.Unit.Prerequisites.Facts;
using Macquarie.Handbook.Data.Unit.Transcript.Facts.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Macquarie.Handbook.Data.Unit.Prerequisites.Facts;

[TestClass]
public class TestCourseRequirementFact
{

    [TestMethod]
    public void TestRequiredCourseProperty() {
        CourseFact course = new() {
            CourseCode = "N000062",
            CourseName = "Bachelor of Information Technology"
        };

        CourseRequirementFact crf = new(course);

        Assert.ReferenceEquals(course, crf.RequiredCourse);
    }

    [TestMethod]
    public void TestRequirementMet() {
        CourseFact course = new() {
            CourseCode = "N000062",
            CourseName = "Bachelor of Information Technology"
        };

        CourseRequirementFact crf = new(course);

        ITranscriptFact courseTranscriptionFact = course;
        TranscriptFactIEnumerableProvider transcriptProvider = new(new List<ITranscriptFact>() { courseTranscriptionFact });

        Assert.IsTrue(crf.RequirementMet(transcriptProvider));
        Assert.IsFalse(crf.RequirementMet(null));
        Assert.IsFalse(crf.RequirementMet(new TranscriptFactDictionaryProvider(null)));
        Assert.IsFalse(crf.RequirementMet(new TranscriptFactDictionaryProvider(new Dictionary<string, ITranscriptFact>())));
    }
}
