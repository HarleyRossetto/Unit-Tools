using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Macquarie.JSON
{
    public static class JsonSerialisationHelper
    {
        public static T DeserialiseJsonObject<T>(string json) {
#if WRITE_ALL_JSON_TO_DISK
            _ = WriteJsonToFile($"data/units/raw/{DateTime.Now.ToString("yyMMdd_HHmmss_fffff")}", json);
#endif
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string SerialiseObject(object obj, Formatting formatting = Formatting.Indented) {
            return JsonConvert.SerializeObject(obj, formatting, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, Converters = { new StringEnumConverter() } });
        }

        public static async Task SerialiseObjectToJsonFile(object obj, string fileName, bool saveWithTimeStamp = false, Formatting formatting = Formatting.Indented) {
            var jsonString = SerialiseObject(obj, formatting);

            await WriteJsonToFile(fileName, jsonString, saveWithTimeStamp);
        }

        public static async Task WriteJsonToFile(string fileName, string jsonString, bool saveWithTimeStamp = false) {
            if (string.IsNullOrEmpty(fileName)) {
                System.Console.WriteLine("Cannot serialise object to file with null/empty fileName!");
                return;
            }

            if (saveWithTimeStamp)
                fileName = $"{fileName}_{DateTime.Now.ToString("yyMMdd_HHmmss_fffff")}";


            string filePath = fileName;
            //Only add .json to the end if it isn't already there.
            if (!fileName.EndsWith(".json"))
                filePath += ".json";

            if (TryCreateDirectory(filePath)) {
                try {
                    await File.WriteAllTextAsync(filePath, jsonString);
                } catch (Exception ex) {
                    System.Console.WriteLine($"Failed to serialise json string.\nReason:\n{ex.ToString()}");
                }
            }
        }

        private static bool TryCreateDirectory(string filePath) {
            FileInfo outputFileInfo;
            try {
                outputFileInfo = new FileInfo(filePath);

                if (!Directory.Exists(outputFileInfo.DirectoryName))
                    Directory.CreateDirectory(outputFileInfo.DirectoryName);
                return true;
            } catch (Exception ex) {
                System.Console.WriteLine($"Failed to create directory.\n {ex.Message}");
                return false;
            }
        }
    }
}