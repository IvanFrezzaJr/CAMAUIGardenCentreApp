using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using CAMAUIGardenCentreApp.Services;


namespace CAMAUIGardenCentreApp.ViewModels;



public partial class LoginViewModel : ObservableObject
{
    // Propriedades observáveis (notificam a View quando alteradas)
    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _phone;

    private readonly AuthService _authService;

    public LoginViewModel(AuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private async void OnLogin()
    {
        // Implemente sua lógica de autenticação aqui
        if (await _authService.Authenticate(Name, Phone))
        {
            _authService.Login();
            // Navega para a página principal
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            // Exibe uma mensagem de erro
            Application.Current.MainPage.DisplayAlert("Error", "Invalid email or password", "OK");
        }
    }

    [RelayCommand]
    private async void OnRegister()
    {
        await Shell.Current.GoToAsync("RegisterPage");
    }


}
