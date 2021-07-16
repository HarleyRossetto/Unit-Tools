using System;
using Macquarie.Handbook.Data.Transcript.Facts;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public class AdmissionRequirementFact : BasicRequirementFact
    {
        private IRequirementFact _fact;
        public override IRequirementFact Fact {
            get {
                return _fact;
            }
            init {
                if (value is CourseRequirementFact) {
                    _fact = value;
                } else {
                    throw new ArgumentException($"Expected {typeof(CourseRequirementFact)} but received {value}");
                }
            }
        }

        public override string ToString() {
            return $"Admission to {Fact.ToString()}";
        }
    }
}