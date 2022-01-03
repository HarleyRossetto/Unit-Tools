using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MQHandbookLib.src.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MQHandbookLib.src.Macquarie.Handbook.JSON;

public class JsonSerialisationHelper
{
    private readonly ILogger<JsonSerialisationHelper> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;

    public JsonSerialisationHelper(IDateTimeProvider dateTimeProvider, ILogger<JsonSerialisationHelper> logger) {
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public static T DeserialiseJsonObject<T>(string json) {
        return JsonConvert.DeserializeObject<T>(json);
    }

    public static string SerialiseObject(object obj, Formatting formatting = Formatting.Indented) {
        return JsonConvert.SerializeObject(obj, formatting, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, Converters = { new StringEnumConverter() } });
    }

    public async Task SerialiseObjectToJsonFile(object obj, string fileName, bool saveWithTimeStamp = false, Formatting formatting = Formatting.Indented) {
        var jsonString = SerialiseObject(obj, formatting);

        await WriteJsonToFile(fileName, jsonString, saveWithTimeStamp);
    }

    public async Task WriteJsonToFile(string fileName, string jsonString, bool saveWithTimeStamp = false) {
        if (string.IsNullOrEmpty(fileName)) {
            _logger.LogWarning("Cannot serialise object to file with null or empty fileName.");
            return;
        }

        if (saveWithTimeStamp)
            fileName = $"{fileName}_{_dateTimeProvider.DateTimeNow:yyMMdd_HHmmss_fffff}";


        string filePath = fileName;
        //Only add .json to the end if it isn't already there.
        if (!fileName.EndsWith(".json"))
            filePath += ".json";

        if (DirectoryUtilities.TryCreateDirectory(filePath)) {
            try {
                await File.WriteAllTextAsync(filePath, jsonString);
            } catch (Exception ex) {
                _logger.LogError("Failed to serialise json string.\nReason:\n{exception}", ex);
            }
        }
    }

}
