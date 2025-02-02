﻿using CAMAUIGardenCentreApp.Data;
using CAMAUIGardenCentreApp.Models;
using CAMAUIGardenCentreApp.Services;
using CAMAUIGardenCentreApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMAUIGardenCentreApp.ViewModels;

public partial class CheckoutViewModel : ObservableObject
{
    private readonly DatabaseContext _context;
    private readonly BasketService _cartService;
    private readonly CheckoutService _checkoutService;
    private readonly AuthService _authService;

    public CheckoutViewModel(DatabaseContext context, BasketService cartService, CheckoutService checkoutService, AuthService authService)
    {
        _context = context;
        _cartService = cartService;
        _checkoutService = checkoutService;
        _authService = authService;
        LoadCheckoutItems();
    }

    [ObservableProperty]
    private ObservableCollection<CartItem> _cartItems = new();

    [ObservableProperty]
    private decimal _totalPrice;

    private void LoadCheckoutItems()
    {
        var items = _cartService.GetCartItems();
        CartItems = new ObservableCollection<CartItem>(items);
        UpdateTotalPrice();

    }

    private void UpdateTotalPrice()
    {
        TotalPrice = CartItems.Sum(item => item.Product.Price * item.Quantity);
    }

    [RelayCommand]
    private async Task GoToPayAsync()
    {


        var userLogged = Preferences.Default.Get<int>("LogedUserId", 0);

        if (userLogged == 0)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "User not found. Please try to login again", "OK");
            return;
        }


        Checkout checkout  = await _checkoutService.CreateCheckoutAsync(userLogged);
        if (checkout == null)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Checkout Error. Please try again later", "OK");
            return;
        }


        var items = _cartService.GetCartItems();

        foreach (CartItem item in items)
        {
            await _checkoutService.AddItemAsync(checkout.Id, item.Product.Name, item.Quantity, (item.Quantity * item.Product.Price));
            
        }

        await Application.Current.MainPage.DisplayAlert("Success", "Purchase finished with success!", "OK");

        await Shell.Current.GoToAsync(nameof(SuccessPage));
    }
}