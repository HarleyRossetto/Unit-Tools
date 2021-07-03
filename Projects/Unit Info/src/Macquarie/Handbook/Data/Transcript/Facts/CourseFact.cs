using System;

namespace Macquarie.Handbook.Data.Transcript.Facts
{
    public class CourseFact : ITranscriptFact
    {
        private string _courseCode = "";

        public CourseFact(string courseCode) => CourseCode = courseCode;

        public string CourseCode {
            get => _courseCode;
            init {
                if (String.IsNullOrEmpty(value.Trim()))
                    throw new NullReferenceException("CourseCode cannot be null or empty.");
                
                _courseCode = value.ToUpper();
            }
        }

        public override bool Equals(object obj) {
            if (obj is not null && obj is CourseFact) {
                var otherResult = obj as CourseFact;
                return CourseCode == otherResult.CourseCode;
            }
            return false;
        }

        public override int GetHashCode() => CourseCode.GetHashCode();

        public override string ToString() => CourseCode;
    }
}