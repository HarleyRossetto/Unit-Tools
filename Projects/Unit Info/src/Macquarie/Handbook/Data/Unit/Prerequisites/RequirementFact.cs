using System;
using System.Collections.Generic;
using System.Linq;

namespace Macquarie.Handbook.Data.Unit.Prerequisites
{

    public class ResultsDictionary : Dictionary<string, IResult>
    {
        public bool ContainsResult(string resultCode) => base.ContainsKey(resultCode);
        public bool TryGetResults(string resultCode, out IResult result) => base.TryGetValue(resultCode, out result);
    }

    public interface IResult { }

    public class CourseResult : IResult {
        public string CourseCode;

        public override bool Equals(object obj) {
            if (obj is not null && obj is CourseResult) {
                var otherResult = obj as CourseResult;
                return CourseCode == otherResult.CourseCode;
            }
            return false;
        }

         public override int GetHashCode() {
            return CourseCode.GetHashCode();
        }
    }

    public class UnitResult : IResult
    {
        public string UnitCode;
        public Grade Grade;

        public override bool Equals(object obj) {
            if (obj is not null && obj is UnitResult) {
                var otherResult = obj as UnitResult;
                return (UnitCode == otherResult.UnitCode) && (otherResult.Grade >= Grade);
            }
            return false;
        }

        public override int GetHashCode() {
            return UnitCode.GetHashCode() ^ (int)Grade;
        }
    }

    
    public enum Grade
    {
        Fail,
        Pass,
        Credit,
        Distinction,
        HighDistinction
    }

    public interface IRequirementFact
    {
        public bool RequirementMet(ResultsDictionary results);
    }

    public record RequirementUnit : IRequirementFact
    {
        public UnitResult UnitResultRequirements;

        /// <summary>
        /// Example method for determining if a fact has had its requirements met
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public bool RequirementMet(ResultsDictionary results) {
            results.TryGetResults(UnitResultRequirements.UnitCode, out IResult completedUnitResult);
            if (completedUnitResult is not null && completedUnitResult is UnitResult) {
                return UnitResultRequirements.Equals(completedUnitResult);
            }
            return false;
        }
    }

    public record RequirementCourse : IRequirementFact
    {
        public CourseResult CourseResultRequirements;

        public bool RequirementMet(ResultsDictionary results) {
            results.TryGetResults(CourseResultRequirements.CourseCode, out IResult completeCourseResult);
            if (completeCourseResult is not null && completeCourseResult is CourseResult) {
                return CourseResultRequirements.Equals(completeCourseResult);
            }
            return false;
        }
    }

    public abstract record RequirementList : IRequirementFact
    {
        public List<IRequirementFact> Facts { get; set; }

        public abstract bool RequirementMet(ResultsDictionary results);
    }

    public record RequirementListAnd : RequirementList
    {
        //If any requirement is not met, return false.
        public override bool RequirementMet(ResultsDictionary results) {
            return Facts.All((fact) =>
            {
                return fact.RequirementMet(results);
            });
        }
    }

    public record RequirementListOr : RequirementList
    {
        //If any requirement is met, return true.
        public override bool RequirementMet(ResultsDictionary results) {
            return Facts.Any((fact) =>
            {
                return fact.RequirementMet(results);
            });
        }
    }

    public record RequirementCoRequisite : IRequirementFact
    {
        public RequirementList Facts { get; set; }

        public bool RequirementMet(ResultsDictionary results) {
            return Facts.RequirementMet(results);
        }
    }

    public record RequirementAdmission : IRequirementFact
    {
        public RequirementList Facts { get; set; }

        public bool RequirementMet(ResultsDictionary results) {
            return Facts.RequirementMet(results);
        }
    }

    public record RequirementCompletionOf : IRequirementFact
    {
        public IRequirementFact Fact { get; set; }

        public bool RequirementMet(ResultsDictionary results) {
            return Fact.RequirementMet(results);
        }
    }

}