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
        await Task.Delay(2000); // Simula um pequeno atraso
        await _initializer.InitializeAsync(); // Inicializa o banco de dados

        // Navega para a página principal
        Application.Current.MainPage = new AppShell();
    }
}
