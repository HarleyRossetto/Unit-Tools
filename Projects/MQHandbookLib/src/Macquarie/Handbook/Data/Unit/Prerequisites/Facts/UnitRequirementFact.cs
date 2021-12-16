namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts;

using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Transcript.Facts.Providers;

public class UnitRequirementFact : IUnitRequirement
{
    public UnitFact RequiredUnit { get; init; }

    public UnitRequirementFact(UnitFact requiredUnitResults) => RequiredUnit = requiredUnitResults;

    public override string ToString() => RequiredUnit.ToString();

    /// <summary>
    /// Example method for determining if a fact has had its requirements met
    /// </summary>
    /// <param name="resultsProvider"></param>
    /// <returns></returns>
    public bool RequirementMet(ITranscriptFactProvider resultsProvider) {
        ITranscriptFact fact = null;
        resultsProvider?.GetFact(RequiredUnit.UnitCode, out fact);
        return RequiredUnit.Equals(fact);
    }
}
