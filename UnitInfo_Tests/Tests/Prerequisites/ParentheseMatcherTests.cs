using Microsoft.VisualStudio.TestTools.UnitTesting;

using Macquarie.Handbook.Helpers.Prerequisites;
using System.Collections.Generic;

namespace UnitTools_Tests
{
    [TestClass]
    public class ParentheseMatcherTests
    {
        [TestMethod]
        public void TestParentheseMatcher() {
            string inputOne = "(Admission to BEd(Prim) and (EDUC258 or EDUC2580) and (EDUC260 or EDUC2600) and (EDUC267 or EDUC2670)) or (130cp including (EDUC258 or EDUC2580) and (EDUC260 or EDUC2600) and (EDUC267 or EDUC2670) and (EDTE353 or EDTE3530))";
            var resultOne = ParentheseMatcher.GetParenthesePairings(inputOne);
            Assert.AreEqual<int>(18, new List<(int, ParentheseType, int)>(resultOne).Count);

            string inputTwo = "Admission to MPICT or MCPICT or GradDipPICT or GradDipCPICT or PGCertPICT or MPICTMIntSecSt or MCPICTMIntSecSt or MIntSecStud or GradDipIntSecStud or GradCertIntell or MCTerrorism or MCyberSec or GradDipSecStudCr or GradCertSecStudCr or MIntell or MSecStrategicStud or MCrim or MSecStrategicStudMCrim or MSecStrategicStudMIntell or MSecStrategicStudMCyberSec or MSecStrategicStudMCTerrorism or MIntellMCrim or MIntellMCyberSec or MIntellMCTerrorism or MCyberSecMCTerrorism or MCyberSecMCrim or MCTerrorismMCrim or Master of Cyber Security Analysis or (10cps at 6000 level or 10cps 8000 level)";
            var resultTwo = ParentheseMatcher.GetParenthesePairings(inputTwo);
            Assert.AreEqual<int>(2, new List<(int, ParentheseType, int)>(resultTwo).Count);

            string inputThree = "Admission to MPICT or MCPICT or GradDipPICT or GradDipCPICT or PGCertPICT or MPICTMIntSecSt or MCPICTMIntSecSt or MIntSecStud or GradDipIntSecStud or GradCertIntell or MCTerrorism or MCyberSec or GradDipSecStudCr or GradCertSecStudCr or MIntell or MSecStrategicStud or MCrim or MSecStrategicStudMCrim or MSecStrategicStudMIntell or MSecStrategicStudMCyberSec or MSecStrategicStudMCTerrorism or MIntellMCrim or MIntellMCyberSec or MIntellMCTerrorism or MCyberSecMCTerrorism or MCyberSecMCrim or MCTerrorismMCrim or Master of Cyber Security Analysis or ((Admission to BSecStudMCTerrorism or BSecStudMCrim or MBusAnalytics or BSecStudMCyberSecAnalysis or BSecStudMIntell or BSecStudMSecStrategicStud) and (10cp at 6000 level or 10cp at 8000 level))";
            var resultThree = ParentheseMatcher.GetParenthesePairings(inputThree);
            Assert.AreEqual<int>(6, new List<(int, ParentheseType, int)>(resultThree).Count);
        }

        [TestMethod]
        public void TestIsPreviousCharacterALetterOrDigit() {
            string test0 = null;
            Assert.IsFalse(ParentheseMatcher.IsPreviousCharacterALetterOrDigit(test0, 1));
            Assert.IsFalse(ParentheseMatcher.IsPreviousCharacterALetterOrDigit(test0, -5));
            Assert.IsFalse(ParentheseMatcher.IsPreviousCharacterALetterOrDigit(test0, 20));

            string test1 = "";
            Assert.IsFalse(ParentheseMatcher.IsPreviousCharacterALetterOrDigit(test1, 1));
            Assert.IsFalse(ParentheseMatcher.IsPreviousCharacterALetterOrDigit(test1, -1));

            string test2 = "batman243_+{>2a";
            Assert.IsFalse(ParentheseMatcher.IsPreviousCharacterALetterOrDigit(test2, 0));
            Assert.IsTrue(ParentheseMatcher.IsPreviousCharacterALetterOrDigit(test2, 1));
            Assert.IsTrue(ParentheseMatcher.IsPreviousCharacterALetterOrDigit(test2, 9));
            Assert.IsFalse(ParentheseMatcher.IsPreviousCharacterALetterOrDigit(test2, 10));

            string test3 = ";'<>?[P)(*&@^%~`4";
            Assert.IsFalse(ParentheseMatcher.IsPreviousCharacterALetterOrDigit(test3, 25)); //Out of bounds
            Assert.IsFalse(ParentheseMatcher.IsPreviousCharacterALetterOrDigit(test3, 5));
            Assert.IsFalse(ParentheseMatcher.IsPreviousCharacterALetterOrDigit(test3, -2));
            Assert.IsFalse(ParentheseMatcher.IsPreviousCharacterALetterOrDigit(test3, 1));
        }
    }
}
