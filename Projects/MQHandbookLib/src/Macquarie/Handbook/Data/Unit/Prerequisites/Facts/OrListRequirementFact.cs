using System.Linq;
using System.Threading.Tasks;
using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Transcript.Facts.Providers;


namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts;

public class OrListRequirementFact<T> : ListRequirementFact<T> where T : IRequirementFact
{
    public override bool RequirementMet(ITranscriptFactProvider resultsProvider) {
        // If the underlying list contains no facts then we consider our requirements inherently met.
        if (!ContainsFacts()) return true;

        return Facts.Any((fact) => {
            return fact.RequirementMet(resultsProvider);
        });
    }

    public override string ToString() => GetFactListAsString("or");
}
