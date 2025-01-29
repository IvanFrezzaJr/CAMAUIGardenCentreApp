using CAMAUIGardenCentreApp.Services;
using CAMAUIGardenCentreApp.ViewModels;

namespace CAMAUIGardenCentreApp.Views;

public partial class LoginPage : ContentPage
{
    private readonly AuthService _authService;
    private readonly LoginViewModel _loginViewModel;

    public LoginPage(LoginViewModel loginViewModel)
    {
        InitializeComponent();
        BindingContext = loginViewModel;
    }
}