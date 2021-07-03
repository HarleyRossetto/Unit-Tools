using System.Linq;
using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Unit.Prerequisites;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public class ListAndRequirementFact : ListRequirementFact
    {
        public override bool RequirementMet(ITranscriptFactProvider resultsProvider) {
             return Facts.All((fact) =>
            {
                return fact.RequirementMet(resultsProvider);
            });
        }
    }
}