using FinanceNewsMobile.Services;
using System.Collections.ObjectModel;
using FinanceNewsMobile.Models;
using System.Windows.Input;
using System.Diagnostics;

namespace FinanceNewsMobile.ViewModel
{
    public class MainViewModel : BindableObject
    {
        private ApiService _apiService;
        public ObservableCollection<News> NewsArticles { get; } = new ObservableCollection<News>();
        private List<News> AllNewsArticles = new List<News>();
        private string _textSearch;
        private CancellationTokenSource _searchCts;
        private bool _isBusy;
        private bool _isEmpty;
        public string TextSearch
        {
            get => _textSearch;
            set
            {
                _textSearch = value;
                OnPropertyChanged();
                DebouncedSearch();
            }
        }
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public bool IsEmpty
        {
            get => _isEmpty;
            set
            {
                _isEmpty = value;
                OnPropertyChanged();
            }
        }

        private async void DebouncedSearch()
        {
            _searchCts?.Cancel();
            _searchCts = new CancellationTokenSource();
            var token = _searchCts.Token;

            try
            {
                await Task.Delay(700, token);
                if (token.IsCancellationRequested) return;

                if (!string.IsNullOrEmpty(_textSearch)) await Task.Run(() => OnSearchCommand());
                else OnEmptySearchCommand.Execute(null);
            }
            catch (TaskCanceledException)
            {
            }
        }

        public ICommand LoadNewsCommand { get; }
        public ICommand OpenUrlCommand { get; }
        public ICommand OnEmptySearchCommand { get; }

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
                        Debug.WriteLine($"failed to open url {ex.Message}");
                    }
                }

            });
            OnEmptySearchCommand = new Command(async () => await LoadNewsAsync());
            InitializeAsync();
        }

        private void OnSearchCommand()
        {
            var foundNews = AllNewsArticles.Where(found =>
                found != null &&
                (
                    (!string.IsNullOrEmpty(found.Title) && found.Title.Contains(TextSearch, StringComparison.OrdinalIgnoreCase)) || 
                    (!string.IsNullOrEmpty(found.Description) && found.Description.Contains(TextSearch, StringComparison.OrdinalIgnoreCase))
                )
            ).ToList();

            NewsArticles.Clear();
            foreach (var item in foundNews)
            {
                NewsArticles.Add(item);
            }
            IsEmpty = NewsArticles.Count == 0;
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
                Debug.WriteLine("info: News list is null");
            }
            else if (newsList.Articles == null || newsList.Articles.Count == 0)
            {
                Debug.WriteLine("info: No articles found");
            }

            NewsArticles.Clear();
            AllNewsArticles.Clear();

            if (newsList?.Articles != null)
            {
                var sortedList = newsList.Articles
                    .GroupBy(x => x.Url?.Trim().ToLowerInvariant())
                    .Select(y => y.First())
                    .ToList();

                AllNewsArticles.AddRange(sortedList);

                foreach (var article in sortedList)
                {
                    NewsArticles.Add(article);
                }
            }
            IsEmpty = NewsArticles.Count == 0;

            IsBusy = false;
        }
    }
}