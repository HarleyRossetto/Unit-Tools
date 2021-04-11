using System.Text.RegularExpressions;

namespace Macquarie.Handbook.Helpers {
    public static class HTMLTagStripper {
        private static Regex filterHtmlTags = new Regex("<.>(.*?)<\\/.>");
        public static string StripHtmlTags(string input) {
            if (input is null)
                return null;
                
            var output = filterHtmlTags.Match(input);
            if (output.Groups.Count >= 2)
                return output.Groups[1].Value;
            else
                return input;
        }
    }
}