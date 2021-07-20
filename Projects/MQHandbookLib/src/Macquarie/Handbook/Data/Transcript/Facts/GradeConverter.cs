namespace Macquarie.Handbook.Data.Transcript.Facts
{
    public static class GradeConverter
    {

        /// <summary>
        /// Converts a raw mark to it's associated EnumGrade value.
        /// </summary>
        /// <param name="mark">The mark to convert.</param>
        /// <returns></returns>
        public static EnumGrade ConvertFromMark(uint mark) {
            return mark switch
            {
                >= 85 => EnumGrade.HighDistinction,
                >= 75 => EnumGrade.Distinction,
                >= 65 => EnumGrade.Credit,
                >= 50 => EnumGrade.Pass,
                < 50 =>  EnumGrade.Fail
            };
        }

        public static uint ConvertToMark(EnumGrade grade) {
            return grade switch
            {
                EnumGrade.HighDistinction   => 85,
                EnumGrade.Distinction       => 75,
                EnumGrade.Credit            => 65,
                EnumGrade.Pass              => 50,
                _                           => 0,
            };
        }

        public static string ConvertToShortString(EnumGrade grade) {
            return grade switch
            {
                EnumGrade.Fail              => "F",
                EnumGrade.Pass              => "P",
                EnumGrade.Credit            => "Cr",
                EnumGrade.Distinction       => "D",
                EnumGrade.HighDistinction   => "HD",
                _                           => "?"
            };
        }

        public static string ConvertToLongString(EnumGrade grade) {
            return grade switch
            {
                EnumGrade.Fail              => "Fail",
                EnumGrade.Pass              => "Pass",
                EnumGrade.Credit            => "Credit",
                EnumGrade.Distinction       => "Distinction",
                EnumGrade.HighDistinction   => "High Distinction",
                _                           => "Unknown"
            };
        }
    }
}