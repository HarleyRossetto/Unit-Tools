namespace Macquarie.Handbook.Data.Transcript.Facts;

using System;
using System.Text.RegularExpressions;
using Macquarie.Handbook.Data.Unit.Transcript.Facts;
using Macquarie.Handbook.Helpers.Extensions;


public class UnitFact : ITranscriptFact
{
    private const int RegexTimeoutMilliseconds = 50;
    private static readonly Regex subjectCodeRegex = new("([A-Z]){3,4}", RegexOptions.Compiled, TimeSpan.FromMilliseconds(RegexTimeoutMilliseconds));
    private static readonly Regex subjectLevelRegex = new(@"(?<=([A-Z]\s?))(\d{3,4})", RegexOptions.Compiled, TimeSpan.FromMilliseconds(RegexTimeoutMilliseconds));
    private static readonly Regex unitCodeRegex = new(@"\b([A-Z]{3,4})(\s?)([\d]{3,4})", RegexOptions.Compiled, TimeSpan.FromMilliseconds(RegexTimeoutMilliseconds));

    public const uint MINIMUM_MARK = 0;
    public const uint MAXIMUM_MARK = 100;

    private string _unitCode = string.Empty;
    public string UnitCode {
        get => _unitCode;
        init {
            /*
                Ensure supplied value matches the unit code format.
                Formats include:
                    - COMP1000
                    - COMP125
                    - BCM123
                    - MAS 110
            */
            var temp = value.ToUpper();
            var match = unitCodeRegex.Match(temp);
            if (match.Success)
                _unitCode = temp;

            ExtractSubjectCodeHeaderFromUnitCode();

            ExtractStudyLevelFromUnitCode();
        }
    }

    public string UnitName { get; init; }

    private string _unitPrefix = string.Empty;
    public string UnitPrefix => _unitPrefix;

    private EnumStudyLevel _studyLevel = EnumStudyLevel.NoLevel;
    public EnumStudyLevel StudyLevel {
        get => _studyLevel;
        init {
            if ((int)value >= (int)EnumStudyLevel.Level1000 && (int)value <= (int)EnumStudyLevel.Level8000 && (int)value != 5) {
                _studyLevel = value;
            } else {
                _studyLevel = EnumStudyLevel.NoLevel;
            }
        }
    }

    public (uint? Marks, EnumGrade? Grade) Results {
        get => (Marks, Grade); //Potentially remove the getter
        init {
            if (value.Marks is not null) {
                Marks = (uint)value.Marks;
                Grade = GradeConverter.ConvertFromMark(Marks);
            } else if (value.Grade is not null) {
                Grade = (EnumGrade)value.Grade;
                Marks = GradeConverter.ConvertToMark(Grade);
            } else { //Assume fail
                Marks = 0;
                Grade = GradeConverter.ConvertFromMark(Marks);
            }
        }
    }

    private uint _marks = 0;
    public uint Marks {
        get => _marks;
        private init {
            _marks = value.Clamp(MINIMUM_MARK, MAXIMUM_MARK);
        }
    }

    public EnumGrade Grade { get; private init; } = EnumGrade.Fail;

    private void ExtractStudyLevelFromUnitCode() {
        //Extracts the subject number string component, taking the first to assume the study level
        var match2 = subjectLevelRegex.Match(UnitCode);
        _studyLevel = (match2.Success) ? StudyLevelConverter.FromInt(Convert.ToUInt32($"{match2.ToString()[0]}000")) : EnumStudyLevel.NoLevel;
    }

    private void ExtractSubjectCodeHeaderFromUnitCode() {
        // Extracts the first string component out of the UnitCode
        var match = subjectCodeRegex.Match(UnitCode);
        _unitPrefix = (match.Success) ? match.ToString() : string.Empty;
    }

    public string GetKey() => UnitCode;

    public override bool Equals(object obj) {
        if (obj is not UnitFact otherFact) return false;
        return (UnitCode == otherFact.UnitCode) && (otherFact.Grade >= Grade);
    }

    // Unused
    public override int GetHashCode() => UnitCode.GetHashCode();

    public override string ToString() => $"{UnitCode} ({GradeConverter.ConvertToShortString(Grade)})";
}
