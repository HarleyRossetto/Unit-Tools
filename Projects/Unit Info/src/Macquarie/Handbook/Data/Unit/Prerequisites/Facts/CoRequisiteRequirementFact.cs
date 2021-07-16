using System;
using System.Diagnostics.CodeAnalysis;
using Macquarie.Handbook.Data.Transcript.Facts;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public class CoRequisiteRequirementFact : BasicRequirementFact
    {
        private IRequirementFact _fact;
        public override IRequirementFact Fact {
            get {
                return _fact;
            }
            init {
                if (value is UnitRequirementFact or ListRequirementFact) {
                    _fact = value;
                } else {
                    throw new ArgumentException($"Expected {typeof(UnitFact)} or {typeof(ListRequirementFact)} but received {value}");
                }
            }
        }

        public override string ToString() {
            return $"corequisite {Fact.ToString()}";
        }
    }
}