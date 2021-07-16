using System;

namespace Macquarie.Handbook.Data.Unit.Transcript.Facts
{
    public static class StudyLevelConverter
    {
        public static string ToString(EnumStudyLevel studyLevel) {
            return ToInt(studyLevel).ToString();
        }

        public static uint ToInt(EnumStudyLevel studyLevel) => studyLevel switch
        {
            EnumStudyLevel.Level1000 => 1000,
            EnumStudyLevel.Level2000 => 2000,
            EnumStudyLevel.Level3000 => 3000,
            EnumStudyLevel.Level4000 => 4000,
            EnumStudyLevel.Level6000 => 6000,
            EnumStudyLevel.Level7000 => 7000,
            EnumStudyLevel.Level8000 => 8000,
            EnumStudyLevel.NoLevel => 0,
            _ => 0
        };


        public static EnumStudyLevel FromInt(uint v) => v switch
        {
            1000u => EnumStudyLevel.Level1000,
            2000u => EnumStudyLevel.Level2000,
            3000u => EnumStudyLevel.Level3000,
            4000u => EnumStudyLevel.Level4000,
            6000u => EnumStudyLevel.Level6000,
            7000u => EnumStudyLevel.Level7000,
            8000u => EnumStudyLevel.Level8000,
            _ => EnumStudyLevel.NoLevel
        };
    }
}