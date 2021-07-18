using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Transcript.Facts.Providers;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public class CompletionOfRequirementFact : IRequirementFact
    {

        // This class is unnecessary, completion of is in relation to credit points, completion of can therefore 
        // be interpreted as such when parsing.

        public CreditPointRequirementFact Fact { get; init; }

        public bool RequirementMet(ITranscriptFactProvider resultsProvider) {
            throw new System.NotImplementedException();
        }

        public override string ToString() {
            return $"completion of {Fact.ToString()}";
        }
    }
}