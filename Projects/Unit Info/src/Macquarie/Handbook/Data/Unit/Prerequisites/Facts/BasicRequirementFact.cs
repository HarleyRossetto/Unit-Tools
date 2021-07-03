using Macquarie.Handbook.Data.Transcript.Facts;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public abstract class BasicRequirementFact
    {
        public IRequirementFact Fact { get; init; }
        public bool RequirementMet(ITranscriptFactProvider resultsProvider) {
            return Fact.RequirementMet(resultsProvider);
        }

        public override string ToString() => Fact.ToString();
    }
}