using System;
using System.Diagnostics.CodeAnalysis;
using Macquarie.Handbook.Data.Transcript.Facts;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public class CourseRequirementFact : IRequirementFact
    {
        public CourseFact RequiredCourse { get; init; }

        public CourseRequirementFact(CourseFact requiredCourse) => RequiredCourse = requiredCourse;

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public override string ToString() => RequiredCourse.ToString();

        public bool RequirementMet(ITranscriptFactProvider resultsProvider) {
            //TODO Consider how to handle this situation in the future.
            if (resultsProvider is null) return false;

            resultsProvider.GetFact(RequiredCourse.CourseCode, out ITranscriptFact fact);
            return RequiredCourse.Equals(fact);
        }
    }
}