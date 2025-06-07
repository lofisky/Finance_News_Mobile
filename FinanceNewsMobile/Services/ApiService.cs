using FinanceNewsMobile.Models;
using System.Net.Http.Headers;

namespace FinanceNewsMobile.Services
{
    class ApiService {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ApiService(string apiKey)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("FinanceNewsMobile", "1.0"));
            _apiKey = apiKey ?? throw new Exception("Cannot find apiKey");
        }

        public async Task<NewsList> GetNewsAsync()
        {
            string url = $"https://newsapi.org/v2/everything?q=finance&sortBy=publishedAt&apiKey={_apiKey}";
            try
            {
                HttpResponseMessage json = await _httpClient.GetAsync(url);
                if (!json.IsSuccessStatusCode)
                {
                    string errorContent = await json.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Http request failed: {errorContent}");
                    return null;
                }
                string response = await json.Content.ReadAsStringAsync();
                var newsList = JsonSerialize.Deserialize<NewsList>(response);
                return newsList;
                
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"Error fetching news: {ex.Message}");
                return null;
            }
        }
    }
}
