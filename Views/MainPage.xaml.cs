using CAMAUIGardenCentreApp.Services;
using CAMAUIGardenCentreApp.ViewModels;
using CAMAUIGardenCentreApp.Models;
using System.Collections.ObjectModel;


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
        SizeChanged += OnSizeChanged;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadProductsAsync();
        await _viewModel.LoadCarouselItemsAsync();
    }

    private void OnSizeChanged(object sender, EventArgs e)
    {
        if (GridLayout != null)
        {
            double width = this.Width;

            if (width < 400)
                GridLayout.Span = 1; // Para telas pequenas -> 1 coluna
            else if (width < 800)
                GridLayout.Span = 2; // Para telas médias -> 2 colunas
            else if (width < 1200)
                GridLayout.Span = 3; // Para telas grandes -> 3 colunas
            else
                GridLayout.Span = 4; // Para telas muito grandes -> 4 colunas
        }
    }
}
