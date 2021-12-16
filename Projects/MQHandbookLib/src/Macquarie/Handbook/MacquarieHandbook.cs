//#define WRITE_ALL_JSON_TO_DISK

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Macquarie.Handbook.Data;
using Macquarie.Handbook.Data.Shared;
using Macquarie.Handbook.WebApi;
using Unit_Info.Helpers;
using static Macquarie.JSON.JsonSerialisationHelper;
//Bad separation...
using static Unit_Info.Helpers.LocalDataDirectoryHelper;
using static Unit_Info.Helpers.LocalDirectories;

namespace Macquarie.Handbook;

public class MacquarieHandbook : IMacquarieHandbook
{
    readonly HttpClient _httpClient = new();

    public TimeSpan WebRequestTimeout { get => _httpClient.Timeout; set => _httpClient.Timeout = value; }

    public MacquarieHandbook() {
        //TODO Consider setting up httpclient properly and append url options instead of generating new url each time?
        //httpClient.BaseAddress = ""
    }

    /// <summary>
    /// Downloads JSON data from the provided URL.
    /// In the even the http response from is not successful, an empty string is returned.
    /// </summary>
    /// <param name="url">The URL from which to download the JSON resource.</param>
    /// <returns>JSON retrieved from URL. In the event of a http failure, an empty string.</returns>
    public async Task<string> DownloadJsonDataFromUrl(string url) {
        Console.WriteLine(url);
        var response = await _httpClient.GetAsync(url);

        //If the http call has failed print the result to the console and return an empty string.
        if (!response.IsSuccessStatusCode) {
            Console.WriteLine(response.ToString());
            //Return an empty string to parse.
            return string.Empty;
        }

        return await response.Content.ReadAsStringAsync();
    }

    private async Task<MacquarieDataCollection<T>> DownloadDataAsCollection<T>(HandbookApiRequestBuilder apiRequest) where T : MacquarieMetadata {
        return await DownloadDataAsCollection<T>(apiRequest.ToString());
    }

    private async Task<MacquarieDataCollection<T>> DownloadDataAsCollection<T>(string url) where T : MacquarieMetadata {
        var jsonString = await DownloadJsonDataFromUrl(url);

        //
        if (string.IsNullOrEmpty(jsonString)) {
            return null;
        } else {
            var dataCollection = DeserialiseJsonObject<MacquarieDataCollection<T>>(jsonString);

            //string fileName = $"{Environment.CurrentDirectory}//{LocalDataDirectoryHelper.GetDirectory(LocalDirectories.Unit)}//Raw.json";

            //await File.WriteAllTextAsync(fileName, jsonString);

            return dataCollection;
        }
    }

    public static async Task<MacquarieDataCollection<T>> LoadAllLocalData<T>() where T : MacquarieMetadata {
        var dirPath = GetDirectory(Unit_Filtered);
        if (Directory.Exists(dirPath)) {
            try {
                var filesToLoad = new List<string>(Directory.GetFiles(dirPath, "*.json", SearchOption.AllDirectories));

                var results = new MacquarieDataCollection<T>();

                foreach (var file in filesToLoad) {
                    var jsonString = await File.ReadAllTextAsync(file);
                    var data = DeserialiseJsonObject<T>(jsonString);
                    results.Add(data);
                }
                return results;

            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                return null;
            }
        } else {
            return null;
        }
    }

    public async Task<MacquarieUnit> GetUnit(string unitCode, int? implementationYear = null, bool tryRetrieveFromLocalCache = true) {
        MacquarieUnit result = null;

        if (tryRetrieveFromLocalCache)
            result = await TryLoadLocalData<MacquarieUnit>(unitCode, APIResourceType.Unit);

        if (result is null) {
            Console.WriteLine($"Unable to load {unitCode} from local cache, loading from CMS..");
            HandbookApiRequestBuilder apiRequestBuilder = new(unitCode, implementationYear, APIResourceType.Unit);
            var resultsCollection = await DownloadDataAsCollection<MacquarieUnit>(apiRequestBuilder);
            if (resultsCollection.Count >= 1)
                result = resultsCollection[0];
        }

        //Save to local cache  while we at it
        await SerialiseObjectToJsonFile(result, CreateFilePath(Unit_Individual, unitCode));

        return result;
    }

    public async Task<string> GetUnitRawJson(string unitCode) {
        var request = new HandbookApiRequestBuilder(unitCode, 2022, APIResourceType.Unit);
        return await DownloadJsonDataFromUrl(request.ToString());
    }

    public static async Task<T> TryLoadLocalData<T>(string code, APIResourceType resourceType) where T : MacquarieMetadata {
        LocalDirectories path = resourceType switch
        {
            APIResourceType.Course => LocalDirectories.Course_Individual,
            APIResourceType.Unit => LocalDirectories.Unit_Individual,
            _ => LocalDirectories.NoDirectory
        };

        var localPath = CreateFilePath(path, $"{code}.json");

        if (File.Exists(localPath)) {
            Console.WriteLine($"Attempting to load {code} from local cache..");
            var result = await LoadObjectFromFile<T>(localPath);

            if (result is not null)
                Console.WriteLine($"Successfully loaded {code} ");

            return result;
        }

        return default;
    }

    private static async Task<T> LoadObjectFromFile<T>(string file) where T : MacquarieMetadata {
        try {
            var jsonString = await File.ReadAllTextAsync(file);
            var obj = DeserialiseJsonObject<T>(jsonString);

            return obj;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
        return null;
    }

    public async Task<MacquarieDataCollection<MacquarieUnit>> GetAllUnits(int? implementationYear = null, int limit = 3000, bool readFromDisk = false) {
        implementationYear ??= DateTime.Now.Year;

        if (readFromDisk) {
            return await LoadAllLocalData<MacquarieUnit>();
        } else {
            var apiRequest = new HandbookApiRequestBuilder() { ImplementationYear = implementationYear, Limit = limit, ResourceType = APIResourceType.Unit };
            return await DownloadDataAsCollection<MacquarieUnit>(apiRequest);
        }
    }

    public async Task<MacquarieCourse> GetCourse(string courseCode, int? implementationYear = null, bool tryRetrieveFromLocalCache = true) {
        //First attempt to load the requested data from the local cache.
        MacquarieCourse result = null;

        if (tryRetrieveFromLocalCache)
            result = await TryLoadLocalData<MacquarieCourse>(courseCode, APIResourceType.Course);

        //If local data could not be loaded and result is null, try load from CMS, if that fails return null/empty course object
        if (result is null) {
            Console.WriteLine($"Unable to load {courseCode} from local cache, loading from CMS..");
            HandbookApiRequestBuilder apiRequestBuilder = new(courseCode, implementationYear, APIResourceType.Course);
            var resultsCollection = await DownloadDataAsCollection<MacquarieCourse>(apiRequestBuilder);
            if (resultsCollection.Count >= 1)
                return resultsCollection[0];
            else
                return null; //TODO instead of returning null consider returning empty Course object? 
        }

        return result;
    }

    public async Task<MacquarieDataCollection<MacquarieCourse>> GetAllCourses(int? implementationYear = null, int limit = 3000) {
        //If the provided year is null, assume this current year.
        implementationYear ??= DateTime.Now.Year;

        var apiRequest = new CourseApiRequestBuilder() { ImplementationYear = implementationYear, Limit = limit };

        return await DownloadDataAsCollection<MacquarieCourse>(apiRequest);
    }
}
