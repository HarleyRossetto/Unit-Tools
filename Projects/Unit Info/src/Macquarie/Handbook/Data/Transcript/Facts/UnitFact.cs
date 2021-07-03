using System;
using Macquarie.Handbook.Helpers.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Macquarie.Handbook.Data.Transcript.Facts
{
    public class UnitFact : ITranscriptFact
    {
        private string _unitCode;
        public string UnitCode {
            get => _unitCode;
            init => _unitCode = value.ToUpper();
        }

        private int _marks;
        public int Marks {
            get => _marks;
            init => _marks = value.Clamp(GradeConverter.MINIMUM_MARK,
                                         GradeConverter.MAXIMUM_MARK);
        }

        public EnumGrade Grade { get; init; }

        public UnitFact(string unitCode, int marks) {
            UnitCode = unitCode;
            Marks = marks;
            Grade = GradeConverter.ConvertFromMark(marks);
        }

        public UnitFact(string unitCode, EnumGrade grade) {
            UnitCode = unitCode;
            Grade = grade;
            Marks = GradeConverter.ConvertToMark(grade);
        }

        public override bool Equals(object obj) {
            if (obj is not null && obj is UnitFact) {
                var otherFact = obj as UnitFact;
                return (UnitCode == otherFact.UnitCode) && (otherFact.Grade >= Grade);
            }
            return false;
        }

        public override int GetHashCode() => UnitCode.GetHashCode() ^ (int)Grade;

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public override string ToString() => $"{UnitCode} ({GradeConverter.ConvertToShortString(Grade)})";
    }
}