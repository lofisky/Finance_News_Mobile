using FinanceNewsMobile.Models;

namespace FinanceNewsMobile.Services
{
    class ConfigService
    {
        private const string configFileName = "config.json";
        private Config _config;

        public ConfigService()
        {
        }
        public async Task LoadConfigAsync()
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(configFileName);
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();

                _config = JsonSerialize.Deserialize<Config>(json);
                System.Diagnostics.Debug.WriteLine($"info:Config Loaded: {_config?.ApiKey}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Config load failed: {ex.Message}");
            }
        }
        public string GetApiKey() => _config?.ApiKey;
    }
}
