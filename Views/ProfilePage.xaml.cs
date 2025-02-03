using CAMAUIGardenCentreApp.Services;
using CAMAUIGardenCentreApp.ViewModels;

namespace CAMAUIGardenCentreApp.Views;

public partial class ProfilePage : ContentPage
{
    private readonly AuthService _authService;

    public ProfilePage(AuthService authService, ProfileViewModel profileViewModel)
    {
        InitializeComponent();
        BindingContext = profileViewModel;

        _authService = authService;
    }
}