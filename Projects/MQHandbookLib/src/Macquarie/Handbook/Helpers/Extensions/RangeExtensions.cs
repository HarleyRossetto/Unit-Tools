using System;

namespace Macquarie.Handbook.Helpers.Extensions {
    public static class RangeExtensions {
        public static (int start, int length) GetRangeStartAndLength(this Range range) {
            return (range.Start.Value, range.End.Value - range.Start.Value + 1);
        }
    
        public static int GetLength(this Range range) {
            return range.End.Value - range.Start.Value + 1;
        }

        public static bool IsInRange(this Range range, Range other) {
            if (range.Start.Value >= other.Start.Value) {
                if (range.End.Value <= other.End.Value) {
                    return true;
                }
            }
            return false;
        }
    }
}