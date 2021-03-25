//#define WRITE_ALL_JSON_TO_DISK

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Macquarie.Handbook.Data;
using Macquarie.Handbook.Data.Shared;
using Macquarie.Handbook.WebApi;
using static Macquarie.JSON.JsonSerialisationHelper;

namespace Macquarie.Handbook
{
    public static class MacquarieHandbook
    {
        static readonly HttpClient httpClient = new HttpClient();

        public static TimeSpan WebRequestTimeout {
            get { return httpClient.Timeout; }
            set { httpClient.Timeout = value; }
        }

        public static async Task<string> DownloadString(HandbookApiRequestBuilder apiRequest) {
            return await DownloadString(apiRequest.ToString());
        }

        public static async Task<string> DownloadString(string url) {
            Console.WriteLine(url);
            var response = await httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<IList<MacquarieUnit>> GetCMSUnitCollection(UnitApiRequestBuilder apiRequest) {
            return await GetCMSDataCollection<MacquarieUnit>(apiRequest);
        }

        public static async Task<IList<T>> GetCMSDataCollection<T>(HandbookApiRequestBuilder apiRequest) where T : MacquarieMetadata {
            return await GetCMSDataCollection<T>(apiRequest.ToString());
        }

        public static async Task<IList<T>> GetCMSDataCollection<T>(string url) where T : MacquarieMetadata {
            return DeserialiseJsonObject<MacquarieDataResponseCollection<T>>(await DownloadString(url));
        }

        public static async Task<MacquarieUnit> GetUnit(string unitCode, int? implementationYear = null) {
            HandbookApiRequestBuilder apiRequest = new UnitApiRequestBuilder(unitCode, implementationYear);
            var unitResultsCollection = await GetCMSDataCollection<MacquarieUnit>(apiRequest);

            //The JSON returned from the request is always a list, even if just one value.
            //CMS requests through this function should only every return 1 value.
            if (unitResultsCollection.Count == 1) {
                return unitResultsCollection[0];
            } else {
                return null;
            }
        }

        public static async Task<IList<MacquarieUnit>> GetAllUnits(int? implementationYear = null, int limit = 3000) {
            if (implementationYear == null)
                implementationYear = DateTime.Now.Year;

            var apiRequest = new UnitApiRequestBuilder() { ImplementationYear = implementationYear, Limit = limit };

            return await GetCMSDataCollection<MacquarieUnit>(apiRequest);
        }

        public static async Task<MacquarieCourse> GetCourse(string courseCode, int? implementationYear = null) {
            throw new NotImplementedException();
        }

        public static async Task<IList<MacquarieCourse>> GetAllCourses(int? implementationYear = null, int limit = 3000) {
            if (implementationYear == null)
                implementationYear = DateTime.Now.Year;

            var apiRequest = new CourseApiRequestBuilder() { ImplementationYear = implementationYear, Limit = limit };

            return await GetCMSDataCollection<MacquarieCourse>(apiRequest);
        }
    }
}