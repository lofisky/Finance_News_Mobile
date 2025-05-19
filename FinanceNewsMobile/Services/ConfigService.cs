using FinanceNewsMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinanceNewsMobile.Services
{
    class ConfigService
    {
        private const string configFileName = "config.json";
        private Config _config;

        public ConfigService() {
            loadConfig();
        }

        private void loadConfig()
        {
            {
                string configFilePath = Path.Combine(Environment.CurrentDirectory, "config.json");
                if (File.Exists(configFilePath))
                {
                    string json = File.ReadAllText(configFilePath);
                    _config = JsonSerializer.Deserialize<Config>(json);
                }
                else
                {
                    throw new FileNotFoundException("config.json not found!");
                }
            }
        }
        public string GetApiKey()
        {
            return _config?.ApiKey;
        }
    }
}
