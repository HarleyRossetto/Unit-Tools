using System;
using System.Text.RegularExpressions;
using Macquarie.Handbook.Data.Unit.Transcript.Facts;
using Macquarie.Handbook.Helpers.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Macquarie.Handbook.Data.Transcript.Facts
{
    public class UnitFact : ITranscriptFact
    {
        private static readonly Regex subjectCodeRegex = new("([A-Z]){3,4}");
        private static readonly Regex subjectLevelRegex = new(@"(?<=(\w))(\d{3,4})");

        private string _unitCode;
        public string UnitCode {
            get => _unitCode;
            init {
                _unitCode = value.ToUpper();

                ExtractSubjectCodeHeaderFromUnitCode();

                ExtractStudyLevelFromUnitCode();
            }
        }

        private string _subjectCodeHeader;
        public string SubjectCodeHeader {
            get => _subjectCodeHeader;
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

        public UnitFact(string unitCode, int marks) {
            UnitCode = unitCode;
            Marks = marks;
            Grade = GradeConverter.ConvertFromMark(Marks);
        }

        public UnitFact(string unitCode, int marks, EnumStudyLevel studyLevel = EnumStudyLevel.NoLevel) {
            UnitCode = unitCode;
            Marks = marks;
            Grade = GradeConverter.ConvertFromMark(Marks);
            StudyLevel = studyLevel;
        }

        public UnitFact(string unitCode, EnumGrade grade, EnumStudyLevel studyLevel = EnumStudyLevel.NoLevel) {
            UnitCode = unitCode;
            Grade = grade;
            Marks = GradeConverter.ConvertToMark(Grade);
            StudyLevel = studyLevel;
        }

          private void ExtractStudyLevelFromUnitCode() {
            //Extracts the subject number string component, taking the first to assume the study level
            var match2 = subjectLevelRegex.Match(UnitCode);
            _studyLevel = (match2.Success) ? StudyLevelConverter.FromInt(Convert.ToInt32(match2.ToString()[0])) : EnumStudyLevel.NoLevel;
        }

        private void ExtractSubjectCodeHeaderFromUnitCode() {
            // Extracts the first string component out of the UnitCode
            var match = subjectCodeRegex.Match(UnitCode);
            _subjectCodeHeader = (match.Success) ? match.ToString() : string.Empty;
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