using System;
using System.Collections.Generic;

namespace Macquarie.Handbook.Helpers.Prerequisites
{
    public class PrerequisiteElement
    {
        private static int counter = 0;

        public PrerequisiteElement(string prereq, Range range, int depth) {
            Prerequisite = prereq;
            RangeInOriginalString = range;
            Depth = depth;
            GUID = (counter++).ToString();
        }

        public string Prerequisite { get; set; }
        public Range RangeInOriginalString { get; init; }
        public int Depth { get; init; }
        public string GUID { get; init; }
        public string ParentGUID { get; set; }
        public PrerequisiteElement Parent { get; set; }
        public Range RangeInParentString { get; set; }
        public List<PrerequisiteElement> Children { get; set; } = new List<PrerequisiteElement>();

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