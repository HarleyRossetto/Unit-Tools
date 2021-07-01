using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Macquarie.Handbook.Data.Unit.Prerequisites
{

    public class ResultsDictionary : Dictionary<string, ITranscriptFact>
    {
        public bool ContainsResult(string resultCode) => base.ContainsKey(resultCode);
        public bool TryGetResults(string resultCode, out ITranscriptFact result) => base.TryGetValue(resultCode, out result);
    }

    public interface ITranscriptFactStringArguments { }

    public interface ITranscriptFact { }

    public interface IFactAsString {
        public string FactAsString(ITranscriptFactStringArguments args = null);
    }

    public class TranscriptCourseFact : ITranscriptFact
    {
        public string CourseCode;

        public override bool Equals(object obj) {
            if (obj is not null && obj is TranscriptCourseFact) {
                var otherResult = obj as TranscriptCourseFact;
                return CourseCode == otherResult.CourseCode;
            }
            return false;
        }

        public string FactAsString(ITranscriptFactStringArguments args = null) {
            throw new NotImplementedException();
        }

        public override int GetHashCode() {
            return CourseCode.GetHashCode();
        }
    }

    public class TranscriptUnitFact : ITranscriptFact, IFactAsString
    {
        private string _unitCode;
        public string UnitCode {
            get => _unitCode;
            init => _unitCode = value.ToUpper();
        }

        private int Marks { get; init; }

        public Grade Grade { get; init; }

        public TranscriptUnitFact(string unitCode, int grade) {
            UnitCode = unitCode;
            Marks = grade.Clamp(0, 100);
            Grade = Marks switch
            {
                >= 85   => Grade.HighDistinction,
                >= 75   => Grade.Distinction,
                >= 65   => Grade.Credit,
                >= 50   => Grade.Pass,
                <  50   => Grade.Fail
            };
        }

        public TranscriptUnitFact(string unitCode, Grade grade) {
            UnitCode = unitCode;
            Grade = grade;
        }

        public override bool Equals(object obj) {
            if (obj is not null && obj is TranscriptUnitFact) {
                var otherResult = obj as TranscriptUnitFact;
                return (UnitCode == otherResult.UnitCode) && (otherResult.Grade >= Grade);
            }
            return false;
        }

        public override int GetHashCode() {
            return UnitCode.GetHashCode() ^ (int)Grade;
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public string FactAsString(ITranscriptFactStringArguments args = null) {
            StringBuilder sb = new(UnitCode);
            if (args is not null
                && args is TranscriptUnitFactStringArguments
                && (args as TranscriptUnitFactStringArguments).Argument == TranscriptUnitFactStringArguments.EnumTranscriptUnitFactStringArgument.WithGrade) {
                sb.Append($" {GradeToStringCoverter.Convert(Grade)}");
            }
            return sb.ToString();
        }
    }

    public record TranscriptUnitFactStringArguments : ITranscriptFactStringArguments {
        public TranscriptUnitFactStringArguments(EnumTranscriptUnitFactStringArgument argument) {
            Argument = argument;
        }

        public enum EnumTranscriptUnitFactStringArgument
        {
            NoGrade,
            WithGrade
        };

        public EnumTranscriptUnitFactStringArgument Argument { get; init; }
    }

    public static class IntExtensions {
        public static int Clamp(this int value, int minimumValue, int maximumValue) {
            if (value < minimumValue)   return minimumValue;
            if (value > maximumValue)   return maximumValue;
            return value;
        }
    }


    public enum Grade
    {
        Fail = 0,
        Pass = 50,
        Credit = 65,
        Distinction = 75,
        HighDistinction = 85
    }

    public static class GradeToStringCoverter 
    {
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public static string Convert(Grade grade) {
            return grade switch
            {
                Grade.Fail              => "F",
                Grade.Pass              => "P",
                Grade.Credit            => "Cr", //Or CR?
                Grade.Distinction       => "D",
                Grade.HighDistinction   => "HD",
                _                       => throw new ArgumentOutOfRangeException(nameof(grade), grade, "Grade not Fail, Pass, Credit, Distinction or High Distinction")
            };
        }
    }

    public interface IRequirementFact
    {
        public bool RequirementMet(ResultsDictionary results);
    }

    public record RequirementUnit : IRequirementFact, IFactAsString     
    {
        public TranscriptUnitFact UnitResultRequirements;

        public RequirementUnit(TranscriptUnitFact unitResultRequirements) {
            UnitResultRequirements = unitResultRequirements;
        }

        public RequirementUnit(string unitCode, Grade grade) {
            UnitResultRequirements = new(unitCode, grade);
        }

        public RequirementUnit(string unitCode, int marks) {
            UnitResultRequirements = new(unitCode, marks);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public string FactAsString(ITranscriptFactStringArguments args) {
            return UnitResultRequirements.FactAsString(args);
        }

        /// <summary>
        /// Example method for determining if a fact has had its requirements met
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public bool RequirementMet(ResultsDictionary results) {
            results.TryGetResults(UnitResultRequirements.UnitCode, out ITranscriptFact completedUnitResult);
            if (completedUnitResult is not null && completedUnitResult is TranscriptUnitFact) {
                return UnitResultRequirements.Equals(completedUnitResult);
            }
            return false;
        }
    }

    public record RequirementCourse : IRequirementFact
    {
        public TranscriptCourseFact CourseResultRequirements;

        public bool RequirementMet(ResultsDictionary results) {
            results.TryGetResults(CourseResultRequirements.CourseCode, out ITranscriptFact completeCourseResult);
            if (completeCourseResult is not null && completeCourseResult is TranscriptCourseFact) {
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