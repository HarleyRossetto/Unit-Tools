using System.Collections.Generic;
using System.Linq;
using System.Text;
using Macquarie.Handbook.Data.Transcript;
using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Transcript.Facts.Providers;
using Macquarie.Handbook.Data.Unit.Transcript.Facts;
using Unit_Info.src.Macquarie.Handbook.Data.Transcript.Facts.Providers;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts;

public class CreditPointRequirementFact : IRequirementFact
{
    private const int CREDIT_POINT_VALUE_2020 = 10;

    public CreditPoint CreditPoints { get; init; }

    //TODO can we hide this interface through a property?
    public ListRequirementFact<UnitGroupRequirementFact> UnitGroup { get; init; }

    public StudyLevelDescriptor StudyLevelRequirement { get; init; } = new StudyLevelDescriptor(EnumStudyLevel.Level1000, true);

    public IRequirementFact IncludingFact { get; init; } = null;

    public CreditPointRequirementFact(CreditPoint credits, StudyLevelDescriptor studyLevelRequirement = null, ListRequirementFact<UnitGroupRequirementFact> unitGroup = null) {
        CreditPoints = credits;

        if (studyLevelRequirement is not null)
            StudyLevelRequirement = studyLevelRequirement;

        UnitGroup = unitGroup;
    }

    // TODO
    // Check this
    // 40cp in (BCA units at 8000 level or STAT8601-STAT8652) including 20cp from (BCA804 or BCA808 or STAT8606 or STAT8609)

    public bool RequirementMet(ITranscriptFactProvider resultsProvider) {
        //TODO Consider how to handle this situation in the future.
        // Don't let null provider pass this point
        if (resultsProvider is null) return false;

        // Get all units which are at least a pass grade and match the study level requirements.
        var passedUnits = GetUnitsThatMeetPassCriteria(resultsProvider);

        bool creditPointRequirementMet = IsCreditPointRequirementMet(passedUnits);

        if (UnitGroup is not null) {
            passedUnits = GetUnitsWhichMeetUnitGroupRequirement(passedUnits);
            // Determine that the credit point requirement has been met
            creditPointRequirementMet &= IsCreditPointRequirementMet(passedUnits);
        }

        if (creditPointRequirementMet) {
            bool includingFactMet = IsIncludingFactMet(resultsProvider);
            return IncludingFact is null || includingFactMet;
        }

        return false;
    }

    private IEnumerable<UnitFact> GetUnitsWhichMeetUnitGroupRequirement(IEnumerable<UnitFact> passedUnits) {
        return (from unit in passedUnits
                where UnitGroup.RequirementMet(new TranscriptFactSingleUnitProvider(unit))
                select unit);
    }

    private bool IsCreditPointRequirementMet(IEnumerable<UnitFact> passedUnits) {
        return TotalCreditPointsAttainedMeetingRequirements(passedUnits) >= CreditPoints.Value;
    }

    private static int TotalCreditPointsAttainedMeetingRequirements(IEnumerable<UnitFact> passedUnits) {
        return passedUnits.Count() * CREDIT_POINT_VALUE_2020;
    }

    private bool IsIncludingFactMet(ITranscriptFactProvider resultsProvider) {
        bool includingFactMet = false;

        if (IncludingFact is not null)
            includingFactMet = IncludingFact.RequirementMet(resultsProvider);

        return includingFactMet;
    }

    public IEnumerable<UnitFact> GetUnitsThatMeetPassCriteria(ITranscriptFactProvider resultsProvider) {
        return from fact in resultsProvider
               where fact is not null && fact is UnitFact
               let unit = fact as UnitFact
               where unit.Grade >= EnumGrade.Pass && StudyLevelRequirement.Comparator(unit.StudyLevel)
               select unit;
    }

    public override string ToString() {
        var sb = new StringBuilder().Append(CreditPoints).Append("cp");

        if (UnitGroup is not null) {
            sb.AppendFormat(" in {0} units", UnitGroup.ToString());
        }

        if (StudyLevelRequirement is not null) {
            sb.AppendFormat(" at {0} level", StudyLevelConverter.ToInt(StudyLevelRequirement.StudyLevel));
            if (StudyLevelRequirement.OrAbove) {
                sb.Append(" or above");
            }
        }
        if (IncludingFact is not null) {
            sb.AppendFormat(" including {0}", IncludingFact.ToString());
        }
        return sb.ToString();
    }
}
