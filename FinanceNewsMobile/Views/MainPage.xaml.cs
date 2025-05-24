using FinanceNewsMobile.ViewModel;

namespace FinanceNewsMobile
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel _viewModel;
        public MainPage()
        {
            InitializeComponent();

            _viewModel = new MainViewModel();
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.InitializeAsync();
        }

        private void OnUrlTapped(object sender, EventArgs e)
        {
            if(sender is Span span)
            {
                span.TextColor = span.TextColor == Colors.Blue ? Colors.Red : Colors.Blue;
;            }
        }
    }
}
