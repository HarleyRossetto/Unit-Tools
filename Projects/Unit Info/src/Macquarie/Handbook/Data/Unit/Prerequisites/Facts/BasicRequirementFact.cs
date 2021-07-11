using Macquarie.Handbook.Data.Transcript.Facts.Providers;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public abstract class BasicRequirementFact : IRequirementFact
    {
        public IRequirementFact Fact { get; init; }
        public bool RequirementMet(ITranscriptFactProvider resultsProvider) {
            return Fact.RequirementMet(resultsProvider);
        }

        public abstract override string ToString();
    }
}