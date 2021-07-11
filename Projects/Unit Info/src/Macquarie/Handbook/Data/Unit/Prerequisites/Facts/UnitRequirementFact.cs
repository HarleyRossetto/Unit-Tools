using System;
using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Transcript.Facts.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public class UnitRequirementFact : IRequirementFact
    {
        public UnitFact RequiredUnit { get; init; }

        public UnitRequirementFact(UnitFact requiredUnitResults) => RequiredUnit = requiredUnitResults;

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public override string ToString() => RequiredUnit.ToString();

        /// <summary>
        /// Example method for determining if a fact has had its requirements met
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public bool RequirementMet(ITranscriptFactProvider resultsProvider) {
            //TODO Consider how to handle this situation in the future.
            if (resultsProvider is null) return false;

            resultsProvider.GetFact(RequiredUnit.UnitCode, out ITranscriptFact fact);
            return RequiredUnit.Equals(fact);
        }
    }
}