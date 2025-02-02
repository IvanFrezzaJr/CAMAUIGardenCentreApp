﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;


using CAMAUIGardenCentreApp.Data;
using CAMAUIGardenCentreApp.Services;
using CAMAUIGardenCentreApp.Views;
using System.Diagnostics;



namespace CAMAUIGardenCentreApp.ViewModels;



public partial class LoginViewModel : ObservableObject
{
    // Propriedades observáveis (notificam a View quando alteradas)
    [ObservableProperty]
    private string _email;

    [ObservableProperty]
    private string _password;

    private readonly DatabaseContext _context;
    private readonly AuthService _authService;

    public LoginViewModel(DatabaseContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    [RelayCommand]
    private async void OnLogin()
    {
        // Implemente sua lógica de autenticação aqui
        if (await _authService.Authenticate(Email, Password))
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
        Debug.WriteLine("------------------------------------------------------------");
        await Shell.Current.GoToAsync("RegisterPage");
    }


}
