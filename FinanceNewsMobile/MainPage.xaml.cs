using FinanceNewsMobile.Services;

namespace FinanceNewsMobile
{
    public partial class MainPage : ContentPage
    {
        private ApiService _apiService;
        public MainPage()
        {
            InitializeComponent();
            var configService = new ConfigService();
            _apiService = new ApiService(configService);
            LoadNews();
        }

        private async void LoadNews()
        {
            var newsList = await _apiService.GetNewsAsync();
            //NewsCollectionView.ItemsSource = newsList.Articles;
        }
    }
}
