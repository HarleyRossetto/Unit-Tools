using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Unit_Info.JSON
{
    public static class JsonSerialisationHelper
    {
        public static T DeserialiseJsonObject<T>(string json) {
#if WRITE_ALL_JSON_TO_DISK
                WriteJsonToFile(json);
#endif
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string SerialiseObject(object obj) {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, Converters = { new StringEnumConverter() } });
        }

        public static async Task SerialiseObjectToJsonFile(object obj, string fileName, bool saveWithTimeStamp = false, bool humanReadable = true) {
            if (fileName == null || fileName.Length == 0) {
                System.Console.WriteLine("Cannot serialise object to file with null/empty fileName!");
            }

            if (saveWithTimeStamp)
                fileName = $"{fileName}_{DateTime.Now.ToString("yyMMdd_HHmmss_fffff")}";

            string filePath = $"{fileName}.json";

            //Delete the file if it already exists, we are going to update it.
            if (File.Exists(filePath)) {
                try {
                    File.Delete(filePath);
                } catch (Exception ex) {
                    System.Console.WriteLine($"Failed to delete file: {filePath}!\n {ex.ToString()}");

                    //Bail out of here!
                    return;
                }
            }

            FileInfo outputFileInfo;

            try {
                outputFileInfo = new FileInfo(filePath);

                if (!Directory.Exists(outputFileInfo.DirectoryName))
                    Directory.CreateDirectory(outputFileInfo.DirectoryName);

                var jsonString = SerialiseObject(obj);
                await File.WriteAllTextAsync(jsonString, outputFileInfo.FullName);
            } catch (Exception ex) {
                System.Console.WriteLine($"Failed to serialise object {obj.ToString()}\nReason:\n{ex.ToString()}");
                return;
            }
        }
    }
}