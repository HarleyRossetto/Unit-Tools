using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Macquarie.Handbook.Data.Shared;
using Macquarie.Handbook.Data.Unit.Prerequisites;
using Macquarie.JSON;
using Unit_Info.Helpers;

namespace Macquarie.Handbook.Helpers;

public static class PrerequisiteParserOld
{
    private static readonly Dictionary<string, TokenType> Keywords = new();

    static PrerequisiteParserOld() {
        Keywords.Add("admission", TokenType.Admission);
        Keywords.Add("and", TokenType.And);
        Keywords.Add("or", TokenType.Or);
        Keywords.Add("including", TokenType.Including);
    }

    //(Admission to BEd(Prim) and (EDUC258 or EDUC2580) and (EDUC260 or EDUC2600) and (EDUC267 or EDUC2670)) or (130cp including (EDUC258 or EDUC2580) and (EDUC260 or EDUC2600) and (EDUC267 or EDUC2670) and (EDTE353 or EDTE3530))

    /// <summary>
    /// 
    /// </summary>
    /// <param name="prerequisite">String representation of a pre-requisite enrolment rule.</param>
    public static void ParsePrerequisiteString(string prerequisite) {
        var split = prerequisite.Split(' ');

        string topLevelId = "topid";
        var grouping = EnrolmentRuleParentheseParser.BreakdownParentheseGroupingsRecursive(prerequisite, 0, topLevelId);

        foreach (var element in split) {

            var parentheseSplit = new List<string>(element.Split(new char[] { '(', ')' }));
            //If greater than 1 then we contain parenthese
            bool elementContainsParenthese = parentheseSplit.Count > 1;
            if (elementContainsParenthese) {
                int openIndex = element.IndexOf('(');
                int closeIndex = element.LastIndexOf(')');

                System.Console.WriteLine(openIndex + " " + closeIndex);
            }
            var cleanedElement = element.Replace("(", string.Empty).Replace(")", string.Empty);

            TokenType token;
            bool isToken = IsToken(element, out token);

            if (isToken) {
                //System.Console.WriteLine(token.ToString());
            }
        }
    }

    private static bool PrecedingValueIsLetter() {
        return false;
    }

    private static bool ContainsParenthese(string value) {
        return value.Contains('(') || value.Contains(')');
    }

    private static bool IsToken(string value, out TokenType token) {
        if (Keywords.ContainsKey(value.ToLower())) {
            token = Keywords[value.ToLower()];
            return true;
        }
        token = TokenType.None;
        return false;
    }


    public static void ParsePrerequisites(IEnumerable<EnrolmentRule> enrolmentRules, string unitCode) {
        //Parse some prereqs while we are at it?
        //Potentially move this later into some kind of observer / notifier system to accomodate
        //more runtime data extraction.
        IEnumerable<EnrolmentRule> preReqsRaw = from rule in enrolmentRules
                                                where rule.Type.Value == "prerequisite"
                                                select rule;


        //Get a dictionary of GUIDs and parenthese groups.
        var parentheseGroups = EnrolmentRuleParentheseParser.ParseParentheseGroups(preReqsRaw);

        //Dictionary to hold our connector types
        var connectorDictionary = ParseGroupsForStructures(parentheseGroups);

        //ID regex , Dictionary<string, Tuple<Connector, ParentheseGroup>> connectorDictionary
        Regex guidRegex = new Regex(@"([0-9a-f]){8}(-([0-9a-f]{4})){3}-([0-9a-f]{12})");

        //Run through all the connectors.
        foreach (var expression in connectorDictionary.Values) {
            /*
                If the expression is in its most basic form, find its parent
                and begin rebuilding the sequence as a graph.
            */
            if (expression.Item2.CanBeBrokenDownFurther) {
                //Leep a list of items that need to be removed after the loop has been completed.
                var removeOnCompletion = new List<string>(10);

                foreach (var parentGuid in expression.Item1.StringValues) {
                    var bracedValueGuid = guidRegex.Match(parentGuid);

                    try {
                        var connector = connectorDictionary[bracedValueGuid.Value].Item1;
                        expression.Item1.ConnectorValues.Add(connector);

                        //Slate the parent guid for removal
                        removeOnCompletion.Add(parentGuid);
                    }
                    catch (Exception ex) {
                        System.Console.WriteLine(ex.ToString());
                    }
                }

                //Remove the values.
                removeOnCompletion.ForEach(s => expression.Item1.StringValues.Remove(s));
            }
        }

        return;

        // if (connectorDictionary.Values.Count > 0) {
        //     var topLevelConnector = connectorDictionary.Values.Last().Item1;
        //     topLevelConnector.OriginalString = preReqsRaw.First().Description;


        //     //Discard so there is no await keyword warning
        //     _ = JsonSerialisationHelper.SerialiseObjectToJsonFile(topLevelConnector, LocalDataDirectoryHelper.CreateFilePath(LocalDirectories.Unit_Filtered, $"{unitCode}_PreReqs"));

        //     PrintPrereqGraph(topLevelConnector, 0);

        //     successfulCompletions++;
        // }
    }

    private static Dictionary<string, Tuple<Connector, ParentheseGroup>> ParseGroupsForStructures(Dictionary<string, ParentheseGroup> parentheseGroups) {
        var results = new Dictionary<string, Tuple<Connector, ParentheseGroup>>();
        //Work backwards through the list of parenthese groups and parse structures
        foreach (var group in parentheseGroups.Reverse()) {
            var type = AscertainConnector(group.Value);
            Connector result = TryParseStructure(type, group.Value);

            //If we cant find or parse the connector type then assign it a basic STATEMENT type
            result ??= new Connector() { ConnectionType = ConnectorType.STATEMENT };
            result.StringValues.Add(group.Value.GroupString);

            //Add the connector to the dictionary
            results.Add(group.Value.ID, new Tuple<Connector, ParentheseGroup>(result, group.Value));
        }
        return results;
    }

    private static void PrintPrereqGraph(Connector connector, int level) {
        if (connector.IsMostBasic) {
            var lastString = connector.StringValues.Last();
            foreach (var preReqs in connector.StringValues) {
                Console.WriteLine(preReqs);
                if (preReqs != lastString)
                    System.Console.WriteLine(connector.ConnectionType.ToString());
            }
        } else {
            var lastConnector = connector.ConnectorValues.Last();
            foreach (var child in connector.ConnectorValues) {
                PrintPrereqGraph(child, level++);
                if (child != lastConnector)
                    System.Console.WriteLine(connector.ConnectionType.ToString());
            }
        }
    }

    private static void WriteNTabs(int n) {
        for (int i = 0; i < n; i++)
            Console.Write("\t");
    }

    /// <summary>
    /// Find the first matching connector type in the group string
    /// </summary>
    /// <param name="group">Parenthese Group to parse.</param>
    /// <returns>First match for a valid connector type. If none are found returns ConnectorType.NONE</returns>
    public static ConnectorType AscertainConnector(ParentheseGroup group) {
        var types = typeof(ConnectorType).GetEnumNames();

        foreach (var type in types) {
            if (group.GroupString.ToLower().Contains($" {type.ToLower()} "))
                return (ConnectorType)Enum.Parse(typeof(ConnectorType), type.ToUpper());
        }

        return ConnectorType.NONE;
    }

    private static Connector TryParseStructure(ConnectorType type, ParentheseGroup group) {
        string filter = string.Format(" {0} ", type.ToString().ToLower());
        if (group.GroupString.Contains(filter)) {
            var split = group.GroupString.Split(filter, StringSplitOptions.TrimEntries);

            var connector = new Connector() { ConnectionType = type };
            connector.StringValues.AddRange(split);
            return connector;
        }
        return null;
    }


    #region REGEX_STUFF

    // Regex to capture all unit code variations
    public static Regex regexUnitCode = new(@"\b([A-Z]{3,4})(\s?)([\d]{3,4})");


    //Matches 4 characters and 4 digits, beginning and ending on word boundaries.
    //i.e. COMP1000
    public static Regex regex2020UnitCode = new Regex(@"\b([A-Z]{4})(\d{4})\b");
    //Matches 4 characters and 3 digits, beginning and ending on word boundaries.
    //i.e. COMP125
    public static Regex regexPre2020UnitCode_variation1 = new Regex(@"\b([A-Z]{4})(\d{3})\b");
    //Matches 3 characters and 3 digits, beginning and ending on word boundaries.
    //i.e. BCM102
    public static Regex regexPre2020UnitCode_variation2 = new Regex(@"\b([A-Z]{3})(\d{3})\b");
    //Matches 3 characters, a single whitespace and 3 digits, beginning and ending on word boundaries.
    //i.e. MAS 110
    public static Regex regexPre2020UnitCode_variation3 = new Regex(@"\b([A-Z]{3})(\s{1})(\d{3})\b");

    //Throw these in a list
    public static List<Regex> regexFilters = new List<Regex>() {    regex2020UnitCode,
                                                                        regexPre2020UnitCode_variation1,
                                                                        regexPre2020UnitCode_variation2,
                                                                        regexPre2020UnitCode_variation3 };


    private static IEnumerable<EnrolmentRule> ParseEnrolmentRulesWithRegex(IEnumerable<EnrolmentRule> rules) {

        //We need a temporary list to hold new rules because we cannot modify EnrolementRules
        //whilst we operating on the results of the LINQ query;
        List<EnrolmentRule> tempNewRules = new List<EnrolmentRule>(3);

        foreach (var prerequsite in rules) {
            foreach (var filter in regexFilters) {
                var matches = filter.Match(prerequsite.Description);

                foreach (var prerequisiteSubject in matches.Captures) {
                    EnrolmentRule newRule = new EnrolmentRule();
                    //Use "prerequsiteparsed" as a flag to let us know this is a value we can work with directly.
                    newRule.Type = new LabelledValue() { Label = "Pre-requsite Parsed", Value = "prerequisiteparsed" };
                    newRule.Description = prerequisiteSubject.ToString();
                    tempNewRules.Add(newRule);
                }
            }
        }
        return tempNewRules;
    }

    private static IEnumerable<EnrolmentRule> ParseEnrolmentRegexParentheses(IEnumerable<EnrolmentRule> rules) {
        //Capture Parenthese groups, timeout of 1 second.
        Regex regexParentheses = new Regex(@"\([\s\S]*?\)", RegexOptions.None, new TimeSpan(0, 0, 1));

        List<Match> matches = new List<Match>();

        foreach (var item in rules) {
            matches.Add(regexParentheses.Match(item.Description));
        }

        return null;
    }

    #endregion

}


public class Connector
{
    public string OriginalString { get; set; }
    public ConnectorType ConnectionType { get; init; }
    public List<string> StringValues { get; set; } = new List<string>(2);
    public List<Connector> ConnectorValues { get; set; } = new List<Connector>(2);

    public bool IsMostBasic {
        get {
            if (ConnectorValues is null) {
                return false;
            } else {
                return ConnectorValues.Count == 0;
            }
        }
    }

    public override string ToString() {
        return "Connector: " + ConnectionType.ToString();
    }
}

public enum ConnectorType
{
    AND,
    OR,
    NONE,
    STATEMENT,
    ADMISSION
}

internal enum TokenType
{
    None,
    Statement,
    OpenBrace,
    CloseBrace,
    And,
    Or,
    Admission,
    Including
}

public class PrerequisiteStatement
{
    string Statement { get; set; }
    public List<PrerequisiteStatement> Statements { get; set; }
}
