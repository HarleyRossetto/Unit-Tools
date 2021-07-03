using System.Linq;
using Macquarie.Handbook.Data.Transcript.Facts;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public class OrListRequirementFact : ListRequirementFact
    {
        public override bool RequirementMet(ITranscriptFactProvider resultsProvider) {
            return Facts.Any((fact) =>
           {
               return fact.RequirementMet(resultsProvider);
           });
        }
    }
}