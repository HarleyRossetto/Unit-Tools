using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Macquarie.Handbook.Data;
using Macquarie.Handbook.Data.Shared;
using Macquarie.Handbook.WebApi;
using Microsoft.Extensions.Logging;
using MQHandbookLib.src.Helpers;
using MQHandbookLib.src.Macquarie.Handbook.JSON;
using static MQHandbookLib.src.Helpers.LocalDataDirectoryHelper;
using static MQHandbookLib.src.Helpers.LocalDirectories;
using static MQHandbookLib.src.Macquarie.Handbook.JSON.JsonSerialisationHelper;

namespace MQHandbookLib.src.Macquarie.Handbook;

public class MacquarieHandbook : IMacquarieHandbook
{
    private readonly HttpClient _httpClient = new();
    private readonly ILogger<MacquarieHandbook> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;
    public TimeSpan WebRequestTimeout { get => _httpClient.Timeout; set => _httpClient.Timeout = value; }
    private readonly JsonSerialisationHelper _jsonSerialisationHelper;

    public MacquarieHandbook(ILogger<MacquarieHandbook> handbookLogger, ILogger<JsonSerialisationHelper> jsonLogger, IDateTimeProvider dateTimeProvider) {
        _logger = handbookLogger;
        _dateTimeProvider = dateTimeProvider;
        _jsonSerialisationHelper = new(dateTimeProvider, jsonLogger);
    }

    /// <summary>
    /// Downloads JSON data from the provided URL.
    /// In the even the http response from is not successful, an empty string is returned.
    /// </summary>
    /// <param name="url">The URL from which to download the JSON resource.</param>
    /// <returns>JSON retrieved from URL. In the event of a http failure, an empty string.</returns>
    public async Task<string> DownloadJsonDataFromUrl(string url, CancellationToken cancellationToken) {
        _logger.LogInformation("Retrieving json data from {url}.", url);

        var response = await _httpClient.GetAsync(url, cancellationToken);

        if (response.IsSuccessStatusCode) {
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }

        _logger.LogWarning("Http data request failed. Response: {response}", response);
        return string.Empty;
    }

    private async Task<MacquarieDataCollection<T>> DownloadDataAsCollection<T>(string url, CancellationToken cancellationToken = default) where T : MacquarieMetadata {
        var jsonString = await DownloadJsonDataFromUrl(url, cancellationToken);

        if (string.IsNullOrEmpty(jsonString)) {
            return default;
        } else {
            return DeserialiseJsonObject<MacquarieDataCollection<T>>(jsonString);
        }
    }

    private async Task<MacquarieDataCollection<T>> DownloadDataAsCollection<T>(HandbookApiRequestBuilder apiRequest, CancellationToken cancellationToken = default) where T : MacquarieMetadata {
        return await DownloadDataAsCollection<T>(apiRequest.ToString(), cancellationToken);
    }

    private async Task<T> LoadObjectFromFile<T>(string file) where T : MacquarieMetadata {
        try {
            var jsonString = await File.ReadAllTextAsync(file);
            var obj = DeserialiseJsonObject<T>(jsonString);

            return obj;
        } catch (Exception ex) {
            _logger.LogError("Exception thrown in {methodName}. Exception: {exMessage}", nameof(LoadObjectFromFile), ex.Message);
        }
        return default;
    }

    public async Task<T> TryLoadLocalData<T>(string code, APIResourceType resourceType) where T : MacquarieMetadata {
        LocalDirectories path = resourceType switch {
            APIResourceType.Course => Course_Individual,
            APIResourceType.Unit => Unit_Individual,
            _ => NoDirectory
        };

        var localPath = CreateFilePath(path, $"{code}.json");

        if (File.Exists(localPath)) {
            _logger.LogInformation("Attempting to load {code} from local cache at {localPath}", code, localPath);
            var result = await LoadObjectFromFile<T>(localPath);

            if (result is null) {
                _logger.LogError("Failed to load unit {unitCode} from {localPath}", code, localPath);
                return default;
            }

            _logger.LogInformation("Successfully loaded {code}", code);
            return result;
        }

        _logger.LogWarning("Unable to load local resource with path: {path}", path);
        return default;
    }

    public async Task<MacquarieDataCollection<T>> LoadAllLocalData<T>() where T : MacquarieMetadata {
        var dirPath = GetDirectory(Unit_Filtered);
        if (Directory.Exists(dirPath)) {
            try {
                var filesToLoad = new List<string>(Directory.GetFiles(dirPath, "*.json", SearchOption.AllDirectories));

                var results = new MacquarieDataCollection<T>();

                //Asynchronously load all tasks.
                List<Task> readAsyncTasks = new();
                filesToLoad.ForEach(file => {
                    readAsyncTasks.Add(new Task(async () => {
                        var data = DeserialiseJsonObject<T>(await File.ReadAllTextAsync(file));
                        results.Add(data);
                    }));
                });

                await Parallel.ForEachAsync(readAsyncTasks, new ParallelOptions() { MaxDegreeOfParallelism = 10 }, default);
               // await Task.WhenAll(readAsyncTasks);

                // foreach (var file in filesToLoad) {
                //     var jsonString = await File.ReadAllTextAsync(file);
                //     var data = DeserialiseJsonObject<T>(jsonString);
                //     results.Add(data);
                // }

                return results;

            } catch (Exception ex) {
                _logger.LogError("Failed to load get all files in {dirPath}. Exception: {ex}", dirPath, ex.ToString());
                return default;
            }
        } else {
            return default;
        }
    }

    public async Task<MacquarieUnit> GetUnit(string unitCode, int? implementationYear = null, bool tryRetrieveFromLocalCache = true) {
        MacquarieUnit result = null;

        if (tryRetrieveFromLocalCache)
            result = await TryLoadLocalData<MacquarieUnit>(unitCode, APIResourceType.Unit);

        if (result is null) {
            _logger.LogInformation("Unable to load {unitCode} from local cache, loading from CMS..", unitCode);
            HandbookApiRequestBuilder apiRequestBuilder = new(unitCode, implementationYear, APIResourceType.Unit);
            var resultsCollection = await DownloadDataAsCollection<MacquarieUnit>(apiRequestBuilder);
            if (resultsCollection.Count >= 1)
                result = resultsCollection[0];
        }

        //Save to local cache  while we at it
        await _jsonSerialisationHelper.SerialiseObjectToJsonFile(result, CreateFilePath(Unit_Individual, unitCode));

        return result;
    }

    public async Task<string> GetUnitRawJson(string unitCode) {
        var request = new HandbookApiRequestBuilder(unitCode, 2022, APIResourceType.Unit);
        return await DownloadJsonDataFromUrl(request.ToString(), default);
    }

    public async Task<MacquarieDataCollection<MacquarieUnit>> GetAllUnits(int? implementationYear = null, int limit = 3000, bool readFromDisk = false, CancellationToken cancellationToken = default) {
        implementationYear ??= _dateTimeProvider.DateTimeNow.Year;

        _logger.LogInformation("Loading all unit data for {implementationYear}.", implementationYear);

        if (readFromDisk) {
            return await LoadAllLocalData<MacquarieUnit>();
        } else {
            var apiRequest = new HandbookApiRequestBuilder() { ImplementationYear = implementationYear, Limit = limit, ResourceType = APIResourceType.Unit };
            return await DownloadDataAsCollection<MacquarieUnit>(apiRequest, cancellationToken);
        }
    }

    public async Task<MacquarieCourse> GetCourse(string courseCode, int? implementationYear = null, bool tryRetrieveFromLocalCache = true) {
        //First attempt to load the requested data from the local cache.
        MacquarieCourse result = null;

        if (tryRetrieveFromLocalCache)
            result = await TryLoadLocalData<MacquarieCourse>(courseCode, APIResourceType.Course);

        //If local data could not be loaded and result is null, try load from CMS, if that fails return null/empty course object
        if (result is null) {
            _logger.LogInformation("Unable to load {courseCode} from local cache, loading from CMS..", courseCode);
            HandbookApiRequestBuilder apiRequestBuilder = new(courseCode, implementationYear, APIResourceType.Course);
            var resultsCollection = await DownloadDataAsCollection<MacquarieCourse>(apiRequestBuilder);
            if (resultsCollection.Count >= 1)
                return resultsCollection[0];
            else
                return default;
        }

        return result;
    }

    public async Task<MacquarieDataCollection<MacquarieCourse>> GetAllCourses(int? implementationYear = null, int limit = 3000) {
        //If the provided year is null, assume this current year.
        implementationYear ??= _dateTimeProvider.DateTimeNow.Year;

        var apiRequest = new CourseApiRequestBuilder() { ImplementationYear = implementationYear, Limit = limit };

        return await DownloadDataAsCollection<MacquarieCourse>(apiRequest);
    }
}
