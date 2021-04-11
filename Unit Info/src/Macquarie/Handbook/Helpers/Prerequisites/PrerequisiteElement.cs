using System;

namespace Macquarie.Handbook.Helpers.Prerequisites
{
    public class PrerequisiteElement
    {
        public PrerequisiteElement(string prereq, Range range, int depth) {
            Prerequisite = prereq;
            RangeInOriginalString = range;
            Depth = depth;
            GUID = Guid.NewGuid().ToString();
        }

        public string Prerequisite { get; set; }
        public Range RangeInOriginalString { get; init; }
        public int Depth { get; init; }
        public string GUID { get; init; }
        public string ParentGUID { get; set; }
        public Range RangeInParentString { get; set; }

        public bool IsInRange(PrerequisiteElement other) {
            if (this == other)
                return false;
            if (other.Depth != this.Depth - 1)
                return false;

            return this.RangeInOriginalString.IsInRange(other.RangeInOriginalString);
        }

        public override string ToString() {
            return $"{GUID.Substring(0, 8)} {Prerequisite}";
        }
    }
}