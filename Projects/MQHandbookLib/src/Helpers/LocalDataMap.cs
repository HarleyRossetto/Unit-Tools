
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MQHandbookLib.src.Macquarie.Handbook.JSON;

namespace MQHandbookLib.src.Helpers;

public class LocalDataMap
{
    private Dictionary<string, string> unitToDirectoryDictionary;
    private readonly Regex _unitTextRegex = new(@"\D+",
                                                    RegexOptions.IgnoreCase | RegexOptions.Compiled,
                                                    TimeSpan.FromMilliseconds(50));
    private readonly Regex _unitNumRegex = new(@"\d",
                                                    RegexOptions.IgnoreCase | RegexOptions.Compiled,
                                                    TimeSpan.FromMilliseconds(50));
    public bool HasBeenUpdated { get; private set; }

    private readonly JsonSerialisationHelper _jsonSerialisationHelper;

    public LocalDataMap(ILogger<JsonSerialisationHelper> logger ,IDateTimeProvider dateTimeProvider) {
        _jsonSerialisationHelper = new(dateTimeProvider, logger);
    }

    public const string CACHE_OUTPUT_FILE = "UnitDirectoryDictionary.json";
    public readonly string CACHE_FULL_OUTPUT_PATH = LocalDataDirectoryHelper.CreateFilePath(LocalDirectories.Local_Data_Cache, CACHE_OUTPUT_FILE);

    public void LoadCache() {
        if (File.Exists(CACHE_FULL_OUTPUT_PATH)) {
            unitToDirectoryDictionary = JsonSerialisationHelper.DeserialiseJsonObject<Dictionary<string, string>>(File.ReadAllText(CACHE_FULL_OUTPUT_PATH));
        } else {
            unitToDirectoryDictionary = new Dictionary<string, string>();
        }
    }

    public async Task SaveCacheAsync() {
        if (HasBeenUpdated) {
            HasBeenUpdated = false;
            await _jsonSerialisationHelper.SerialiseObjectToJsonFile(unitToDirectoryDictionary, CACHE_FULL_OUTPUT_PATH);
        } else {
            return;
        }
    }

    public bool Register(string unitCode, string directory) {
        //Strip the dept code off beginning add and that to the dictionary.
        var unitText = _unitTextRegex.Match(unitCode).Value;
        HasBeenUpdated = true;
        return unitToDirectoryDictionary.TryAdd(unitText, directory);
    }

    public bool GetDirectoryOut(string unitCode, out string directory) {
        var unitText = _unitTextRegex.Match(unitCode).Value;
        var unitDigits = _unitNumRegex.Match(unitCode).Value;
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
