using System;
using Macquarie.Handbook.Data.Transcript.Facts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public class UnitRequirementFact : IRequirementFact
    {
        public UnitFact RequiredUnitResults { get; init; }

        public UnitRequirementFact(UnitFact requiredUnitResults) => RequiredUnitResults = requiredUnitResults;

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public override string ToString() => RequiredUnitResults.ToString();

        /// <summary>
        /// Example method for determining if a fact has had its requirements met
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public bool RequirementMet(ITranscriptFactProvider resultsProvider) {
            //TODO Consider how to handle this situation in the future.
            if (resultsProvider is null) return false;

            resultsProvider.GetFact(RequiredUnitResults.UnitCode, out ITranscriptFact fact);
            return RequiredUnitResults.Equals(fact);
        }
    }
}