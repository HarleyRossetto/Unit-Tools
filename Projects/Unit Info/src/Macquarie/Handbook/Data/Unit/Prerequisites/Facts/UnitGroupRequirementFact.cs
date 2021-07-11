using System.Linq;
using Macquarie.Handbook.Data.Unit.Prerequisites.Facts;

namespace Macquarie.Handbook.Data.Transcript.Facts.Providers
{
    public class UnitGroupRequirementFact : IRequirementFact
    {
        public string UnitGroup { get; init; }

        public UnitGroupRequirementFact(string unitGroupCode) => UnitGroup = unitGroupCode;

        public bool RequirementMet(ITranscriptFactProvider resultsProvider) {
            //TODO Consider how to handle this situation in the future.
            if (resultsProvider is null) return false;

            return (from fact in resultsProvider
                    where fact is not null && fact is UnitFact
                    let unit = fact as UnitFact
                    where unit.SubjectCodeHeader.Equals(UnitGroup)
                    select unit).Any();
        }

        public override string ToString() => UnitGroup;
    }
}