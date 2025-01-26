using CAMAUIGardenCentreApp.Services;

namespace CAMAUIGardenCentreApp.Views;

public partial class LoadingPage : ContentPage
{
    private readonly AuthService _authService;
    public string DestinationPage { get; set; }

    public LoadingPage(AuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // Simulação de carregamento
        await Task.Delay(2000);

        // Decide para onde ir após o carregamento
        if (!string.IsNullOrEmpty(DestinationPage))
        {
            await Shell.Current.GoToAsync($"//{DestinationPage}");
        }
        else if (await _authService.IsAuthenticatedAsync())
        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
        else
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}