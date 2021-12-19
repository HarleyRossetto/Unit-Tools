using System;
using System.Collections.Generic;
using System.Diagnostics;

using Macquarie.Handbook.Helpers.Extensions;

namespace Macquarie.Handbook.Helpers.Prerequisites;

[DebuggerDisplay("{ID} {Prerequisite}")]
public class PrerequisiteElement
{
    public PrerequisiteElement(string prereq, Range range, int depth, string id) {
        Prerequisite = prereq;
        RangeInOriginalString = range;
        Depth = depth;
        ID = id;
    }

    public string Prerequisite { get; set; }
    public Range RangeInOriginalString { get; init; }
    public int Depth { get; init; }
    public string ID { get; init; }
    public PrerequisiteElement Parent { get; set; }
    public Range RangeInParentString { get; set; }
    public List<PrerequisiteElement> Children { get; set; } = new List<PrerequisiteElement>();

    public bool IsInRange(PrerequisiteElement other) {
        if (this == other)
            return false;
        if (other.Depth != this.Depth - 1)
            return false;

        return RangeInOriginalString.IsInRange(other.RangeInOriginalString);
    }

    public override string ToString() => Prerequisite;
}
