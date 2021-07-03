using Macquarie.Handbook.Data.Transcript.Facts;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public class AdmissionRequirementFact : BasicRequirementFact
    {
        public override string ToString() {
            return $"Admission to {Fact.ToString()}";
        }
    }
}