using System;
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
using CAMAUIGardenCentreApp.Models;
using System.Diagnostics;
using System.Collections.ObjectModel;



namespace CAMAUIGardenCentreApp.ViewModels;



public partial class ProfileViewModel : ObservableObject
{

    private readonly DatabaseContext _context;
    private readonly AuthService _authService;
    private readonly RegisterService _registerService;
    private readonly CheckoutService _checkoutService;

    [ObservableProperty]
    private bool isCorporateUser;

    [ObservableProperty]
    private string nextBillingDate;

    private ObservableCollection<Checkout> _billingItems = new();
    public ObservableCollection<Checkout> BillingItems
    {
        get => _billingItems;
        set
        {
            SetProperty(ref _billingItems, value);
            OnPropertyChanged(nameof(BillingItems));
        }
    }

    int billingDay = 15;

    int currentUser = 0;

    public ProfileViewModel(AuthService authService, RegisterService registerService, CheckoutService checkoutService)
    {
        _authService = authService;
        _registerService = registerService;
        _checkoutService = checkoutService;

        currentUser = Preferences.Default.Get<int>("LogedUserId", 0);

        LoadData(currentUser);

    }

    private async void LoadData(int userId)
    {

        await GetCurrentUserBillingDate(userId);
        await _checkoutService.UpdateOverdueCheckoutsAsync(userId, billingDay);

        var users = await _registerService.GetUserByIdAsync(userId);
        var user = users.FirstOrDefault();

        if (user == null)
        {
            return;
        }
        IsCorporateUser = (user.Type == "corporate");

       
        if (IsCorporateUser)
        {
            NextBillingDate = _checkoutService.CalculateNextBillingDate(billingDay).ToString("dd/MM/yyyy");

            var items = await _checkoutService.GetUnpaidCheckoutsAsync(userId);

            

            BillingItems.Clear();
            foreach (var item in items)
            {
                BillingItems.Add(item);
            }
        }
    }



    private async Task GetCurrentUserBillingDate(int userId)
    {
        
        var accounts = await _registerService.GetUserAccountAsync(userId);

        foreach (var item in accounts)
        {
            billingDay = item.BillingDay;
            
        }
    }


    [RelayCommand]
    private void Logout()
    {
        _authService.Logout();
        Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}