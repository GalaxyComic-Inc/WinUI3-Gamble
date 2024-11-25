using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PRA_C3_DJ_SA_CH_AL.Data
{
    internal class DataApi
    {
        private readonly HttpClient _httpClient;

        public DataApi()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetDataAsync(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                throw new Exception($"API call failed with status code: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calling API: {ex.Message}");
            }
        }
    }
}
