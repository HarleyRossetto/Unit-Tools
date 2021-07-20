using System;
using System.Linq;
using Macquarie.Handbook.Data.Unit.Prerequisites.Facts;

namespace Macquarie.Handbook.Data.Transcript.Facts.Providers
{
    public class UnitGroupRequirementFact : IRequirementFact
    {
        private string _unitGroup = String.Empty;
        public string UnitGroup {
            get {
                return _unitGroup;
            }
            init {
                if (!String.IsNullOrWhiteSpace(value))
                    _unitGroup = value;
            }
        }

        public UnitGroupRequirementFact() { }

        public UnitGroupRequirementFact(string unitGroupCode) => UnitGroup = unitGroupCode;

        public bool RequirementMet(ITranscriptFactProvider resultsProvider) {
            //TODO Consider how to handle this situation in the future.
            if (resultsProvider is null || String.IsNullOrWhiteSpace(UnitGroup)) return false;

            return (from fact in resultsProvider
                    where fact is not null && fact is UnitFact
                    let unit = fact as UnitFact
                    where unit.UnitPrefix.Equals(UnitGroup)
                    select unit).Any();
        }

        public override string ToString() => UnitGroup;
    }
}