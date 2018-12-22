using System;
using HcGetProduct.Classes;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace HcGetProduct
{
    public static class HcHelpers
    {
        // The pattern of HashiCorp releases url
        public const string HcReleasesUrlPattern = "https://releases.hashicorp.com/{0}/index.json";

        static HcHelpers() {}

        /// <summary>
        /// Create a Task which will retrun an instance of the HcReleaseData class
        /// in which the properties will be populated from the HC release API
        /// Will use it's own HttpClient instance
        /// <summary>
        public static async Task<HcReleaseData> GetHcProductReleasesData (string product)
        {
            using (HttpClient webClient = new HttpClient())
            {
                return await GetHcProductReleasesData(product, webClient);
            }
        }

        /// <summary>
        /// An overload of GetHcProductReleasesData where the HttpClient can be passed
        /// <summary>
        public static async Task<HcReleaseData> GetHcProductReleasesData (string product, HttpClient webClient)
        {
            if (product == null || webClient == null)
            {
                throw new ArgumentNullException(); 
            }
            string baseUrl = String.Format(HcReleasesUrlPattern, product);
            HcReleaseData result = null;
            HttpResponseMessage response = await webClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject<HcReleaseData>(responseBody);
            return result;
        }
    }
}