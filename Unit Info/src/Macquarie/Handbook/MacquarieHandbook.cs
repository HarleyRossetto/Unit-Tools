//#define WRITE_ALL_JSON_TO_DISK

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Macquarie.Handbook.Data;
using Macquarie.Handbook.Data.Shared;
using Macquarie.Handbook.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Macquarie.Handbook
{
    public static class MacquarieHandbook
    {
        static readonly HttpClient httpClient = new HttpClient();

        static MacquarieHandbook() {
            if (!Directory.Exists("data/"))
                Directory.CreateDirectory("data/");
        }

        public static TimeSpan WebRequestTimeout {
            get { return httpClient.Timeout; }
            set { httpClient.Timeout = value; }
        }

        public static async Task<string> DownloadString(HandbookApiRequestBuilder apiRequest) {
            return await DownloadString(apiRequest.ToString());
        }

        public static async Task<string> DownloadString(string url) {
            var response = await httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
        public static async Task<MacquarieDataResponseCollection<T>> GetDataResponseCollection<T>(HandbookApiRequestBuilder apiRequest) where T : MacquarieMetadata {
            return await GetDataResponseCollection<T>(apiRequest.ToString());
        }

        public static async Task<MacquarieDataResponseCollection<T>> GetDataResponseCollection<T>(string url) where T : MacquarieMetadata {
            return DeserialiseJsonObject<MacquarieDataResponseCollection<T>>(await DownloadString(url));
        }

        public static async Task<MacquarieDataResponseCollection<MacquarieUnit>> GetUnitCollectionResponse(UnitApiRequestBuilder apiRequest) {
            return await GetDataResponseCollection<MacquarieUnit>(apiRequest);
        }

        public static T DeserialiseJsonObject<T>(string json) {
#if WRITE_ALL_JSON_TO_DISK
                WriteJsonToFile(json);
#endif

            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string SerialiseObject(object obj) {
            return JsonConvert.SerializeObject(obj, new StringEnumConverter());
        }

        public static async Task SerialiseObjectToFile(object obj, string file) {
            var jsonString = SerialiseObject(obj);
            await WriteJsonToFile(jsonString, file);
        }

        public static async Task WriteJsonToFile(string json) {
            await File.WriteAllTextAsync(
                                        string.Format(
                                                        "downloaded/{0}.json",
                                                        DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss-fffffff")),
                                        json);
        }

        public static async Task WriteJsonToFile(string json, string file) {
            if (File.Exists(file))
                File.Delete(file);
            await File.WriteAllTextAsync(file, json);
        }
    }
}