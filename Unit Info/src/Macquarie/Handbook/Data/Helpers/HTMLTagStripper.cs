using System.Text.RegularExpressions;

namespace Macquarie.Handbook.Data.Helpers {
    public static class HTMLTagStripper {
        private static Regex filterHtmlTags = new Regex("<.>(.*?)<\\/.>");
        public static string StripHtmlTags(string input) {
            if (input == null)
                return input;
                
            var output = filterHtmlTags.Match(input);
            if (output.Groups.Count >= 2)
                return output.Groups[1].Value;
            else
                return input;
        }
    }
}