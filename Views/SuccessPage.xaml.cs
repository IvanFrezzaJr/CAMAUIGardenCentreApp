using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace CAMAUIGardenCentreApp.Views;

public partial class SuccessPage : ContentPage
{
    private bool _isRedirecting = false; 

    public SuccessPage()
    {
        InitializeComponent();
    }

    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await Task.Delay(3000);

        // Ensure it doesn't trigger twice
        if (!_isRedirecting) 
        {
            _isRedirecting = true;
            await NavigateToHomePage();
        }
    }

    private async void OnRedirectButtonClicked(object sender, EventArgs e)
    {
        if (!_isRedirecting)
        {
            _isRedirecting = true;
            await NavigateToHomePage();
        }
    }

    private async Task NavigateToHomePage()
    {
        await Shell.Current.GoToAsync("//MainPage"); 
    }



    private async Task GoToCartAsync()
    {
        await Shell.Current.GoToAsync(nameof(BasketPage));
    }


    private async Task GoToProfileAsync()
    {
        await Shell.Current.GoToAsync(nameof(ProfilePage));
    }



    private async Task GoToCategoryAsync()
    {
        await Shell.Current.GoToAsync(nameof(CategoryPage));
    }
}
