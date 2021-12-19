using System;
using System.Diagnostics.CodeAnalysis;
using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Transcript.Facts.Providers;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts;

public class CoRequisiteRequirementFact : IRequirementFact
{
    private ListRequirementFact<UnitRequirementFact> Fact { get; init; } = new AndListRequirementFact<UnitRequirementFact>();

    public bool RequirementMet(ITranscriptFactProvider resultsProvider) {
        return Fact.RequirementMet(resultsProvider);
    }

    public override string ToString() {
        return $"corequisite {Fact}";
    }
}
