using System.Collections.Generic;

namespace Unit_Info.Helpers;

public static class LocalDataDirectoryHelper
{
    private static readonly Dictionary<string, string> paths = new()
    {
        { nameof(LocalDirectories.Local_Data_Cache), BASE_DIR + "cache/" },
        { nameof(LocalDirectories.Unit), BASE_DIR + "units/" },
        { nameof(LocalDirectories.Unit_Individual), BASE_DIR + "units/individual/" },
        { nameof(LocalDirectories.Unit_Filtered), BASE_DIR + "units/filtered/" },
        { nameof(LocalDirectories.Unit_Filtered_BySchool), BASE_DIR + "units/filtered/" },
        { nameof(LocalDirectories.Unit_PreRequisite), BASE_DIR + "units/prerequisites/" },
        { nameof(LocalDirectories.Unit_PreRequisite_Unparsed), BASE_DIR + "units/prerequisites/unparsed/" },
        { nameof(LocalDirectories.Course), BASE_DIR + "courses/" },
        { nameof(LocalDirectories.Course_Individual), BASE_DIR + "courses/individual/" },
        { nameof(LocalDirectories.Course_Filtered), BASE_DIR + "courses/filtered/" },
        { nameof(LocalDirectories.Course_Filtered_BySchool), BASE_DIR + "courses/filtered/bySchool/" }
    };

    private const string BASE_DIR = "data/";

    public static string DirectoryHeader { get; set; } = "";

    public static string CreateFilePath(LocalDirectories directory, string fileName) {
        if (paths.ContainsKey(directory.ToString())) {
            return $"{DirectoryHeader}/{paths[directory.ToString()]}{fileName}";
        } else {
            return "";
        }
    }

    public static string GetDirectory(LocalDirectories directory) {
        if (paths.ContainsKey(directory.ToString())) {
            return $"{DirectoryHeader}/{paths[directory.ToString()]}";
        } else {
            return "";
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
    NoDirectory

}
