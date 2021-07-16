using System;

namespace Macquarie.Handbook.Data.Transcript.Facts
{
    public class CourseFact : ITranscriptFact
    {
        private string _courseCode = "";
        private string _courseName = "";

        public CourseFact() { }
        public CourseFact(string courseName) => CourseName = courseName;

        public string CourseCode {
            get => _courseCode;
            init {
                if (String.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException("CourseCode cannot be null or empty.");
                
                _courseCode = value.ToUpper();
            }
        }

        public string CourseName {
            get => _courseName;
            init {
                if (String.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException("CourseCode cannot be null or empty.");

                _courseName = value;
            }
        }

        public override bool Equals(object obj) {
            if (obj is not null && obj is CourseFact) {
                var otherResult = obj as CourseFact;
                return CourseCode == otherResult.CourseCode;
            }
            return false;
        }

        public override int GetHashCode() => CourseName.GetHashCode();

        public override string ToString() => CourseName;
    }
}