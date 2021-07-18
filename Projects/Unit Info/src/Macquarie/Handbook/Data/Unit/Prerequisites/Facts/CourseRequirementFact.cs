using System;
using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Transcript.Facts.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public class CourseRequirementFact : ICourseRequirement
    {
        public CourseFact RequiredCourse { get; init; }

        public CourseRequirementFact(CourseFact requiredCourse) => RequiredCourse = requiredCourse;

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public override string ToString() => RequiredCourse.ToString();

        public bool RequirementMet(ITranscriptFactProvider resultsProvider) {
            ITranscriptFact fact = null;
            resultsProvider?.GetFact(RequiredCourse.GetKey(), out fact);
            return RequiredCourse.Equals(fact);
        }
    }
}