using System.Collections.Generic;

namespace Unit_Info.Helpers
{
    public static class LocalDataDirectoryHelper
    {
        private static Dictionary<string, string> paths = new Dictionary<string, string>();
        private const string BASE_DIR = "data/";

        static LocalDataDirectoryHelper() {
            paths.Add("Local_Data_Cache",               BASE_DIR + "cache/");
            paths.Add("Unit",                           BASE_DIR + "units/");
            paths.Add("Unit_Individual",                BASE_DIR + "units/individual/");
            paths.Add("Unit_Filtered",                  BASE_DIR + "units/filtered/");
            paths.Add("Unit_Filtered_BySchool",         BASE_DIR + "units/filtered/");
            paths.Add("Unit_PreRequisite",              BASE_DIR + "units/prerequisites/");
            paths.Add("Unit_PreRequisite_Unparsed",     BASE_DIR + "units/prerequisites/unparsed/");
            paths.Add("Course",                         BASE_DIR + "courses/");
            paths.Add("Course_Individual",              BASE_DIR + "courses/individual/");
            paths.Add("Course_Filtered",                BASE_DIR + "courses/filtered/");
            paths.Add("Course_Filtered_BySchool",       BASE_DIR + "courses/filtered/bySchool/");
        }

        public static string CreateFilePath(LocalDirectories directory, string fileName) {
            if (!paths.ContainsKey(directory.ToString())) {
                return "";
            } else {
                return $"{paths[directory.ToString()]}{fileName}";
            }
        }

        public static string GetDirectory(LocalDirectories directory) {
             if (!paths.ContainsKey(directory.ToString())) {
                return "";
            } else {
                return paths[directory.ToString()];
            }
        }
    }

    public enum LocalDirectories
    {
        Local_Data_Cache,
        Unit,
        Unit_Individual,
        Unit_Filtered,
        Unit_Filtered_BySchool,
        Unit_PreRequisite,
        Unit_PreRequisite_Unparsed,
        Course,
        Course_Individual,
        Course_Filtered,
        Course_Filtered_BySchool,

    }
}