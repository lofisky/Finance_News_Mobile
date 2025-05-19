using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FinanceNewsMobile.Models;


namespace FinanceNewsMobile.Services
{
    class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ApiService(ConfigService configService)
        {
            _httpClient = new HttpClient();
            _apiKey = configService.GetApiKey() ?? throw new Exception("API Key missing in config");
        }

        public async Task<NewsList> GetNewsAsync()
        {
            string url = $"https://newsapi.org/v2/everything?q=finance&apiKey={_apiKey}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(_apiKey);

                if (!response.IsSuccessStatusCode) {
                    return null;
                }
                string responseContent = await response.Content.ReadAsStringAsync();
                var newsList = JsonSerializer.Deserialize<NewsList>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return newsList;

            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"Error fetching news: {ex.Message}");
                return null;
            }
            
        }
    }
}
