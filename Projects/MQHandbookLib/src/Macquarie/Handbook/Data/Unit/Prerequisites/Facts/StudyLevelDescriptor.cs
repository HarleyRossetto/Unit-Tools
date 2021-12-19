using System;
using Macquarie.Handbook.Data.Unit.Transcript.Facts;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts;

public class StudyLevelDescriptor
{
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

    public bool OrAbove { get; init; }

    public StudyLevelComparison Comparator {
        get {
            return OrAbove ? StudyLevelEqualOrGreaterComparison : StudyLevelEqualComparison;
        }
    }

    public StudyLevelDescriptor(EnumStudyLevel studyLevel, bool orAbove = false) {
        StudyLevel = studyLevel;
        OrAbove = orAbove;
    }

    public delegate bool StudyLevelComparison(EnumStudyLevel otherLevel);

    public bool StudyLevelEqualComparison(EnumStudyLevel otherLevel) {
        return StudyLevel == otherLevel;
    }

    public bool StudyLevelEqualOrGreaterComparison(EnumStudyLevel otherRequirement) {
        return otherRequirement >= StudyLevel;
    }
}
