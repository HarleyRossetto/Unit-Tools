using System.Diagnostics.CodeAnalysis;
using Macquarie.Handbook.Data.Transcript.Facts;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public class CoRequisiteRequirementFact : BasicRequirementFact
    {
        public override string ToString() {
            return $"corequisite {Fact.ToString()}";
        }
    }
}