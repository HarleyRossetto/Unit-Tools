using System;

namespace Macquarie.Handbook.Data.Helpers {
    public static class RangeExtensions {
        public static (int start, int length) GetRangeStartAndLength(this Range range) {
            return (range.Start.Value, range.End.Value - range.Start.Value + 1);
        }

        public static int GetLength(this Range range) {
            return (range.End.Value - range.Start.Value + 1);
        }
    }
}