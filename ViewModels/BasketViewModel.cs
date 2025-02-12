using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAMAUIGardenCentreApp.Models;
using CAMAUIGardenCentreApp.Services;
using CAMAUIGardenCentreApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;

namespace CAMAUIGardenCentreApp.ViewModels;

public partial class BasketViewModel : ObservableObject
{
    private readonly BasketService _basketService;

    public BasketViewModel(BasketService basketService)
    {
        _basketService = basketService;
        LoadCart();
    }

    [ObservableProperty]
    private ObservableCollection<CartItem> _cartItems = new();

    [ObservableProperty]
    private decimal _totalPrice;

    [ObservableProperty]
    private int _quantity;

    

    private void LoadCart()
    {
        var items = _basketService.GetCartItems();
        CartItems = new ObservableCollection<CartItem>(items);
        UpdateTotalPrice();
    }

    private void UpdateTotalPrice()
    {
        TotalPrice = _basketService.GetTotalPrice();
    }

    [RelayCommand]
    private void RemoveItem(Product product)
    {
        if (product != null)
        {
            var cartItem = CartItems.FirstOrDefault(i => i.Product.Id == product.Id);
            if (cartItem != null)
            {
                _basketService.RemoveFromCart(product.Id);
                CartItems.Remove(cartItem); 
                UpdateTotalPrice();
            }
        }
    }


    [RelayCommand]
    private void IncreaseQuantity(Product product)
    {
        _basketService.IncreaseQuantity(product.Id);
        UpdateTotalPrice();
    }



    [RelayCommand]
    private void DecreaseQuantity(Product product)
    {
        _basketService.DecreaseQuantity(product.Id);
        UpdateTotalPrice();
    }

    [RelayCommand]
    private async Task GoToCheckoutAsync()
    {
        //await Shell.Current.GoToAsync(nameof(CheckoutPage));
        await Shell.Current.GoToAsync("//MainPage/CheckoutPage");
    }


    [RelayCommand]
    private async Task GoToCartAsync()
    {
        if (Shell.Current.CurrentPage is BasketPage)
            return;
        await Shell.Current.GoToAsync("//BasketPage");

        Shell.Current.FlyoutIsPresented = false;
    }

    [RelayCommand]
    private async Task GoToProfileAsync()
    {
        if (Shell.Current.CurrentPage is ProfilePage)
            return;
        await Shell.Current.GoToAsync("ProfilePage");

        Shell.Current.FlyoutIsPresented = false;

    }


    [RelayCommand]
    private async Task GoToCategoryAsync()
    {
        if (Shell.Current.CurrentPage is CategoryPage)
            return;
        await Shell.Current.GoToAsync("CategoryPage");

        Shell.Current.FlyoutIsPresented = false;

    }


}

