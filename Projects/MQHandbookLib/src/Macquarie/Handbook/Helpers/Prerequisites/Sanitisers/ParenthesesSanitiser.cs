namespace Macquarie.Handbook.Helpers.Prerequisites.Sanitisers;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class ParenthesesSanitiser
{
    private const int RegexTimeoutMilliseconds = 50;
    private static readonly TimeSpan RegexTimeout = TimeSpan.FromMilliseconds(RegexTimeoutMilliseconds);
    //Matches (P|C|D|HD) elements
    static readonly Regex _regexGradeRequirement = MakeRegex(@"\((P|CR|D|HD)\)");
    //Matches (OUA|ECE|ECS)
    static readonly Regex _regexOuaRequirement = MakeRegex(@"\(.{3}\)");
    //Matches (waiver)
    static readonly Regex _regexWaiverRequirement = MakeRegex(@"\(waiver\)");
    //Matches (Beijing)
    static readonly Regex _regexBeijingRequirement = MakeRegex(@"\(Beijing\)");
    //Matches (Prim|Hons)
    static readonly Regex _regexPrimHonsRequirement = MakeRegex(@"\(\w{4}\)");
    //Matches (Singe or double degrees)
    static readonly Regex _regexSglOrDblDegreeRequirement = MakeRegex(@"\(Single or double degrees\)");
    //Matches (0 to 5)
    static readonly Regex _regexZeroToFiveNumericRequirement = MakeRegex(@"\(0 to 5\)");
    //Matches (Birth to five)
    static readonly Regex _regexZeroToFiveCharRequirement = MakeRegex(@"\(Birth to five\)");
    //Matches "Pre-requisite "
    static readonly Regex _regexPrerequisiteRequirement = MakeRegex(@"Pre-requisite ");

    static readonly List<Regex> _regexFilters = new()
    {
        _regexGradeRequirement,
        _regexOuaRequirement,
        _regexWaiverRequirement,
        _regexBeijingRequirement,
        _regexPrimHonsRequirement,
        _regexSglOrDblDegreeRequirement,
        _regexZeroToFiveNumericRequirement,
        _regexZeroToFiveCharRequirement,
        _regexPrerequisiteRequirement
    };

    private const char OPEN_SYMBOL_DELIMIT = '[';
    private const char CLOSE_SYMBOL_DELIMIT = ']';

    private static Regex MakeRegex(string pattern) {
        return new Regex(pattern, RegexOptions.Compiled, RegexTimeout);
    }

    public static string Sanitise(string prerequsite) {
        var standardisedBrackets = ReplaceSquareBrackets(prerequsite);
        var qualifiersCorrected = ReplaceBracketedQualifiers(standardisedBrackets);
        return qualifiersCorrected;
    }

    public static string ReplaceBracketedQualifiers(string input) {
        string result = input;
        foreach (var filter in _regexFilters) {
            result = QualifyFilterBrackets(result, filter.Matches(result));
        }
        return result;
    }

    public static string QualifyFilterBrackets(string prerequsites, MatchCollection matches) {
        foreach (Match match in matches) {
            var replaced = ReplaceParenthesesWithDelimitedSymbols(match.Value);
            var inputWithMatchRemoved = prerequsites.Remove(match.Index, match.Length);
            prerequsites = inputWithMatchRemoved.Insert(match.Index, replaced);
        }

        return prerequsites;
    }

    public static string ReplaceSquareBrackets(string input) {
        return input.Replace('[', '(').Replace(']', ')');
    }

    public static string ReplaceParenthesesWithDelimitedSymbols(string input) {
        return input.Replace('(', OPEN_SYMBOL_DELIMIT).Replace(')', CLOSE_SYMBOL_DELIMIT);
    }
}
