using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Macquarie.JSON;
using System.Linq;

namespace Unit_Info.Helpers
{
    public static class LocalDataMap
    {
        public static Dictionary<string, string> unitToDirectoryDictionary;
        static Regex unitTextRegex = new Regex(@"\D+");
        static Regex unitNumRegex = new Regex(@"\d");

        public const string CACHE_OUTPUT_FILE = "UnitDirectoryDictionary.json";
        public static readonly string CACHE_FULL_OUTPUT_PATH = LocalDataDirectoryHelper.CreateFilePath(LocalDirectories.Local_Data_Cache, CACHE_OUTPUT_FILE);

        public static void LoadCache() {
            if (File.Exists(CACHE_FULL_OUTPUT_PATH)) {
                unitToDirectoryDictionary = JsonSerialisationHelper.DeserialiseJsonObject<Dictionary<string, string>>(File.ReadAllText(CACHE_FULL_OUTPUT_PATH));
            } else {
                unitToDirectoryDictionary = new Dictionary<string, string>();
            }
        }

        public static bool Register(string unitCode, string directory) {
            //Strip the dept code off beginning add and that to the dictionary.
            var unitText = unitTextRegex.Match(unitCode).Value;
            return unitToDirectoryDictionary.TryAdd(unitText, directory);
        }

        public static bool GetDirectoryOut(string unitCode, out string directory) {
            var unitText = unitTextRegex.Match(unitCode).Value;
            var unitDigits = unitNumRegex.Match(unitCode).Value;
            bool success = unitToDirectoryDictionary.TryGetValue(unitText, out directory);
            //We append the final level onto the directory
            if (success) {
                directory += $"{unitDigits.First()}000/";
                return true;
            } else {
                return false;
            }
        }
    }
}