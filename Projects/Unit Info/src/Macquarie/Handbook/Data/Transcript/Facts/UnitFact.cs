using System;
using Macquarie.Handbook.Data.Unit.Transcript.Facts;
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

        private EnumStudyLevel _studyLevel;
        public EnumStudyLevel StudyLevel {
            get => _studyLevel;
            init {
                if ((int)value >= 0 && (int)value <= 7 && (int)value != 5) {
                    _studyLevel = value;
                } else {
                    throw new ArgumentOutOfRangeException($"Study Level value {value} is invalid.");
                }
            }
        }


        private int _marks;
        public int Marks {
            get => _marks;
            init => _marks = value.Clamp(GradeConverter.MINIMUM_MARK,
                                         GradeConverter.MAXIMUM_MARK);
        }

        public EnumGrade Grade { get; init; }

        public UnitFact(string unitCode, int marks, EnumStudyLevel studyLevel = EnumStudyLevel.NoLevel) {
            UnitCode = unitCode;
            Marks = marks;
            Grade = GradeConverter.ConvertFromMark(marks);
            StudyLevel = studyLevel;
        }

        public UnitFact(string unitCode, EnumGrade grade, EnumStudyLevel studyLevel = EnumStudyLevel.NoLevel) {
            UnitCode = unitCode;
            Grade = grade;
            Marks = GradeConverter.ConvertToMark(grade);
            StudyLevel = studyLevel;
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