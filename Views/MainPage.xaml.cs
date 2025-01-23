using CAMAUIGardenCentreApp.Services;
using CAMAUIGardenCentreApp.ViewModels;


namespace CAMAUIGardenCentreApp.Views;

public partial class MainPage : ContentPage
{

    private readonly AuthService _authService;
    private readonly ProductsViewModel _viewModel;


    public MainPage(AuthService authService, ProductsViewModel viewModel)
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
