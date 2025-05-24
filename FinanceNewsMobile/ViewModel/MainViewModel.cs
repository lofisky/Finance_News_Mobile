using FinanceNewsMobile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceNewsMobile.Models;
using System.Windows.Input;

namespace FinanceNewsMobile.ViewModel
{
    public class MainViewModel : BindableObject
    {
        private ApiService _apiService;
        public List<News> NewsArticles { get; } = new List<News>();

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }
        public ICommand LoadNewsCommand { get; }
        public ICommand OpenUrlCommand { get; }

        public MainViewModel()
        {
            LoadNewsCommand = new Command(async () => await LoadNewsAsync());

            OpenUrlCommand = new Command<string>(async (url) =>
            {
                if (!string.IsNullOrWhiteSpace(url))
                {
                    try
                    {
                        await Launcher.Default.OpenAsync(url);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"failed to open url {ex.Message}");
                    }
                }

            });
            InitializeAsync();
        }

        public async Task InitializeAsync()
        {
            IsBusy = true;

            var configService = new ConfigService();
            await configService.LoadConfigAsync();

            var apiKey = configService.GetApiKey();

            if (string.IsNullOrEmpty(apiKey))
            {
                IsBusy = false;
                return;
            }
            _apiService = new ApiService(apiKey);

            await LoadNewsAsync();

            IsBusy = false;
        }

        private async Task LoadNewsAsync()
        {
            if (IsBusy) return;

            IsBusy = true;

            var newsList = await _apiService.GetNewsAsync();

            if (newsList == null)
            {
                System.Diagnostics.Debug.WriteLine("info: News list is null");
            }
            else if (newsList.Articles == null || newsList.Articles.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("info: No articles found");
            }

            NewsArticles.Clear();

            if (newsList?.Articles != null)
            {
                var sortedList = newsList.Articles
                    .Where(a => DateTime.TryParse(a.PublishedAt, out _))
                    .OrderByDescending(a => DateTime.Parse(a.PublishedAt))
                    .ToList();

                foreach (var article in sortedList)
                {
                    {
                        NewsArticles.Add(article);
                    }
                }
            }
            IsBusy = false;
        }
    }
}