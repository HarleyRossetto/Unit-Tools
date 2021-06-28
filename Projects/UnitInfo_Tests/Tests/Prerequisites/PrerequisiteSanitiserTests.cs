using Microsoft.VisualStudio.TestTools.UnitTesting;

using Macquarie.Handbook.Helpers.Prerequisites;
using System.Collections.Generic;

namespace UnitTools_Tests
{
    [TestClass]
    public class PrerequisiteSanitiserTests 
    {
        [TestMethod]
        public void TestBracketedQualifiers() {
            string inputOne = "Admission to MCrWrit (OUA)";
            inputOne = ParenthesesSanitiser.ReplaceSquareBrackets(inputOne);
            const string resultOneExpected = "Admission to MCrWrit [OUA]";
            var resultOne = ParenthesesSanitiser.ReplaceBracketedQualifiers(inputOne);
            Assert.AreEqual(resultOneExpected, resultOne);

            string inputTwo = "20cp at 2000 level including PSY234(P) or PSY2234(P)";
            inputTwo = ParenthesesSanitiser.ReplaceSquareBrackets(inputTwo);
            const string resultTwoExpected = "20cp at 2000 level including PSY234[P] or PSY2234[P]";
            var resultTwo = ParenthesesSanitiser.ReplaceBracketedQualifiers(inputTwo);
            Assert.AreEqual(resultTwoExpected, resultTwo);

            string inputThree = "Admission to MAppFin(Beijing) and ECFL866 or AFCL8003";
            inputThree = ParenthesesSanitiser.ReplaceSquareBrackets(inputThree);
            const string resultThreeExpected = "Admission to MAppFin[Beijing] and ECFL866 or AFCL8003";
            var resultThree = ParenthesesSanitiser.ReplaceBracketedQualifiers(inputThree);
            Assert.AreEqual(resultThreeExpected, resultThree);
        }

        [TestMethod]
        public void TestSquareBracketsRemoval(){
            string inputOne = "[Admission to (MTeach(0-5) or GradCertEChild) and (ECED600 or ECHE6000)] or [admission to MEChild or MEd or MEdLead or MIndigenousEd or MSpecEd or GradCertEdS]";
            var resultsOne = ParenthesesSanitiser.ReplaceSquareBrackets(inputOne);
            Assert.IsFalse(resultsOne.Contains('['));
            Assert.IsFalse(resultsOne.Contains(']'));
        }

        [TestMethod]
        public void TestNormaliseeCreditPointRepresentations() {
            (string expected, string input)[] testStrings = {
                ("40 cp", "40 credit points"),
                ("40cp", "40cp"),
                ("40cp", "40cps")
            };

            foreach (var (expected, input) in testStrings) {
                Assert.AreEqual(expected, PrerequisiteSanitiser.NormaliseCreditPointRepresentations(input));
            }
        }

        [TestMethod]
        public void TestRemoveEscapeSequences() {
            (string expected, string input)[] testStrings = {
                ("40 credit points", "40 credit points\n"),
                ("Admission to MCrWrit", "Admission to MCrWrit\n"),
                ("Permission by special approval (waiver)", "Permission by special approval (waiver)\n"),
                ("Pre-requisite 40cp at 1000 level or above", "Pre-requisite\t40cp at 1000 level or above"),
                ("Admission to BClinSc and (MEDI206 or MEDI2400)", "\tAdmission to BClin\rSc and (MEDI206 or MEDI2400)")
            };

            foreach (var (expected, input) in testStrings) {
                var result = PrerequisiteSanitiser.RemoveWhitespaceEscapeSequences(input);
                Assert.AreEqual(expected, result);
            }
        }
    }
}
