using Macquarie.Handbook.Data.Transcript.Facts;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public class CompletionOfRequirementFact : BasicRequirementFact
    {
        public override string ToString() {
            return $"completion of {Fact.ToString()}";
        }
    }
}