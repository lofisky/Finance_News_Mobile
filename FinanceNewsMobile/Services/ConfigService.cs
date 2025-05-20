using FinanceNewsMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace FinanceNewsMobile.Services
{
    class ConfigService
    {
        private const string configFileName = "config.json";
        private Config _config;

        public ConfigService()
        {
            loadConfig();
        }

        private async Task loadConfig()
        {
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(configFileName);
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();

                _config = JsonSerialize.Deserialize<Config>(json);
            }
        }
        public string GetApiKey()
        {
            return _config.ApiKey;
        }
    }
}
