using System.Net.Http;
using System.Threading.Tasks;
using Macquarie.Handbook.Data;
using Macquarie.Handbook.Data.Shared;
using Macquarie.Handbook.WebApi;
using Newtonsoft.Json;

namespace Macquarie.Handbook 
{
    public static class MacquarieHandbook 
    {
        static readonly HttpClient httpClient = new HttpClient();

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
            return JsonConvert.DeserializeObject<MacquarieDataResponseCollection<T>>(await DownloadString(url));
        }

        public static async Task<MacquarieDataResponseCollection<MacquarieUnit>> GetUnitCollectionResponse(UnitApiRequestBuilder apiRequest) {
            return await GetDataResponseCollection<MacquarieUnit>(apiRequest);
        }
    }
}