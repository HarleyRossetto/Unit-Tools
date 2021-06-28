using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using static Macquarie.JSON.JsonSerialisationHelper;
using System.Linq;
using System.Threading.Tasks;

namespace Unit_Info.Helpers
{
    public static class LocalDataMap
    {
        private static Dictionary<string, string> unitToDirectoryDictionary;
        private static readonly Regex unitTextRegex = new(@"\D+");
        private static readonly Regex unitNumRegex = new(@"\d");
        public static bool HasBeenUpdated { get; private set; }


        public const string CACHE_OUTPUT_FILE = "UnitDirectoryDictionary.json";
        public static readonly string CACHE_FULL_OUTPUT_PATH = LocalDataDirectoryHelper.CreateFilePath(LocalDirectories.Local_Data_Cache, CACHE_OUTPUT_FILE);

        public static void LoadCache() {
            if (File.Exists(CACHE_FULL_OUTPUT_PATH)) {
                unitToDirectoryDictionary = DeserialiseJsonObject<Dictionary<string, string>>(File.ReadAllText(CACHE_FULL_OUTPUT_PATH));
            } else {
                unitToDirectoryDictionary = new Dictionary<string, string>();
            }
        }

        public static async Task SaveCacheAsync() {
            if (HasBeenUpdated) {
                HasBeenUpdated = false;
                await SerialiseObjectToJsonFile(LocalDataMap.unitToDirectoryDictionary, LocalDataMap.CACHE_FULL_OUTPUT_PATH);
            } else {
                return;
            }
        }

        public static bool Register(string unitCode, string directory) {
            //Strip the dept code off beginning add and that to the dictionary.
            var unitText = unitTextRegex.Match(unitCode).Value;
            HasBeenUpdated = true;
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