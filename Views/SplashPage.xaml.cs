using CAMAUIGardenCentreApp.Services;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace CAMAUIGardenCentreApp.Views;

public partial class SplashPage : ContentPage
{
    private readonly DatabaseInitializer _initializer;

    public SplashPage(DatabaseInitializer initializer)
    {
        InitializeComponent();
        _initializer = initializer;
        _ = InitializeApp();
    }

    private async Task InitializeApp()
    {
        await _initializer.InitializeAsync();

        Application.Current.MainPage = new AppShell();
    }
}
