using CAMAUIGardenCentreApp.Services;
using CAMAUIGardenCentreApp.ViewModels;


namespace CAMAUIGardenCentreApp.Views;

public partial class MainPage : ContentPage
{

    private readonly AuthService _authService;
    private readonly MainViewModel _viewModel;


    public MainPage(AuthService authService, MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadProductsAsync();
    }
}
