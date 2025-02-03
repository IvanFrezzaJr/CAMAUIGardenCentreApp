using CAMAUIGardenCentreApp.Data;
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
    private readonly RegisterService _registerService;

    public CheckoutViewModel(DatabaseContext context, BasketService cartService, CheckoutService checkoutService, AuthService authService, RegisterService registerService)
    {
        _context = context;
        _cartService = cartService;
        _checkoutService = checkoutService;
        _authService = authService;
        _registerService = registerService;

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


        var users = await _registerService.GetUserByIdAsync(userLogged);
        var user = users.FirstOrDefault();

        if (user == null)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Checkout Error. Please try again later", "OK");
            return;
        }

        Checkout checkout  = await _checkoutService.CreateCheckoutAsync(userLogged);
        if (checkout == null)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Checkout Error. Please try again later", "OK");
            return;
        }

        bool isCorportate = (user.Type == "corporate") ? true : false;

        checkout.IsPaid = !isCorportate;


        var items = _cartService.GetCartItems();
        decimal total = 0;
        decimal totalPerItem = 0;

        foreach (CartItem item in items)
        {
            totalPerItem = item.Quantity * item.Product.Price;
            await _checkoutService.AddItemAsync(checkout.Id, item.Product.Name, item.Quantity, totalPerItem);
            total = total + totalPerItem;
        }

        checkout.TotalAmount = total;

        var checkoutUpdated = await _checkoutService.UpdateCheckoutAsync(checkout);

        if (checkoutUpdated == null)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Checkout Error. It was not possible to update the totalAmount", "OK");
            return;
        } else
        {
            _cartService.CleanCart();
        }


        await Application.Current.MainPage.DisplayAlert("Success", "Purchase finished with success!", "OK");

        await Shell.Current.GoToAsync(nameof(SuccessPage));
    }



}