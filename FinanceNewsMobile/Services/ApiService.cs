using FinanceNewsMobile.Models;
using System.Diagnostics;
using System.Net.Http.Headers;


namespace FinanceNewsMobile.Services
{
    class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ApiService(string apiKey)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("FinanceNewsApp", "1.0"));
            _apiKey = apiKey ?? throw new Exception("API Key missing in config");
        }

        public async Task<NewsList> GetNewsAsync()
        {
            string url = $"https://newsapi.org/v2/everything?q=finance&apiKey={_apiKey}";

            try
            {
                Debug.WriteLine("Starting news api call..");
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                Debug.WriteLine($"API call complete with status: {response.StatusCode}");

                System.Diagnostics.Debug.WriteLine($"info: Request URL: {url}");
                System.Diagnostics.Debug.WriteLine($"info: Response Status Code: {response.StatusCode}");

                if (!response.IsSuccessStatusCode){
                    string errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"info: Error response content: {errorContent}");
                    return null;
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"info: Response content: {responseContent}");

                var newsList = JsonSerialize.Deserialize<NewsList>(responseContent);

                return newsList;


            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"info: Error fetching news: {ex.Message}");
                return null;
            }
            
        }
    }
}
