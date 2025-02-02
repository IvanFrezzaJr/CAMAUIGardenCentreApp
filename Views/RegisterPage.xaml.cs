using CAMAUIGardenCentreApp.Services;
using CAMAUIGardenCentreApp.ViewModels;


namespace CAMAUIGardenCentreApp.Views;

public partial class RegisterPage : ContentPage
{
    private readonly AuthService _authService;
    private readonly RegisterViewModel _registerViewModel;

    public RegisterPage(RegisterViewModel registerViewModel)
    {
        InitializeComponent();
        BindingContext = registerViewModel;
    }
}