using CAMAUIGardenCentreApp.ViewModels;


namespace CAMAUIGardenCentreApp.Views;

public partial class RegisterPage : ContentPage
{

    public RegisterPage(RegisterViewModel registerViewModel)
    {
        InitializeComponent();
        BindingContext = registerViewModel;
    }
}