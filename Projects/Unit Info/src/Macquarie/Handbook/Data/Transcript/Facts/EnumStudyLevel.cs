using System;

namespace Macquarie.Handbook.Data.Unit.Transcript.Facts
{
    public enum EnumStudyLevel
    {
        NoLevel = 0,
        Level1000,
        Level2000,
        Level3000,
        Level4000,
        Level6000 = 6,
        Level7000,
        Level8000
    }

    public static class StudyLevelConverter
    {
        public static string ToString(EnumStudyLevel studyLevel) {
            return ToInt(studyLevel).ToString();
        }

        public static uint ToInt(EnumStudyLevel studyLevel){
            return studyLevel switch
            {
                EnumStudyLevel.Level1000 => 1000,
                EnumStudyLevel.Level2000 => 2000,
                EnumStudyLevel.Level3000 => 3000,
                EnumStudyLevel.Level4000 => 4000,
                EnumStudyLevel.Level6000 => 6000,
                EnumStudyLevel.Level7000 => 7000,
                EnumStudyLevel.Level8000 => 8000,
                EnumStudyLevel.NoLevel   => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(studyLevel), studyLevel, "Study level is not valid")
            };
        }
    }
}