using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using CAMAUIGardenCentreApp.Services;


namespace CAMAUIGardenCentreApp.ViewModels;



public partial class LoginViewModel : ObservableObject
{

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

        if (await _authService.Authenticate(Name, Phone))
        {
            _authService.Login();

            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            Application.Current.MainPage.DisplayAlert("Error", "Invalid email or password", "OK");
        }
    }

    [RelayCommand]
    private async void OnRegister()
    {
        await Shell.Current.GoToAsync("//RegisterPage");
    }


}
