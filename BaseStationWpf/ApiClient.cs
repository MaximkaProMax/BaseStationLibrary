using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BaseStationWpf
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://192.168.0.100:100/");
        }

        public async Task<int?> GetHandoverByStationIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/basestation/{id}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                if (int.TryParse(content, out int handover))
                    return handover;
            }

            return null;
        }
    }
}