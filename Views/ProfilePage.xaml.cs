using CAMAUIGardenCentreApp.Services;
using CAMAUIGardenCentreApp.ViewModels;

namespace CAMAUIGardenCentreApp.Views;

public partial class ProfilePage : ContentPage
{
    
    public ProfilePage(ProfileViewModel profileViewModel)
    {
        InitializeComponent();
        BindingContext = profileViewModel;
    }
}