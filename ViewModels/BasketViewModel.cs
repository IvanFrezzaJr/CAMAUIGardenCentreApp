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
    private readonly BasketService _cartService;

    public BasketViewModel(BasketService cartService)
    {
        _cartService = cartService;
        LoadCart();
    }

    [ObservableProperty]
    private ObservableCollection<CartItem> _cartItems = new();

    [ObservableProperty]
    private decimal _totalPrice;

    private void LoadCart()
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
    private void IncreaseQuantity(Product product)
    {
        var cartItem = CartItems.FirstOrDefault(i => i.Product.Id == product.Id);
        if (cartItem != null)
        {
            cartItem.Quantity++;
            UpdateTotalPrice();
        }
    }

    [RelayCommand]
    private void DecreaseQuantity(Product product)
    {
        var cartItem = CartItems.FirstOrDefault(i => i.Product.Id == product.Id);
        if (cartItem != null && cartItem.Quantity > 1)
        {
            cartItem.Quantity--;
            UpdateTotalPrice();
        }
        else if (cartItem != null && cartItem.Quantity == 1)
        {
            CartItems.Remove(cartItem);
            UpdateTotalPrice();
        }
    }

    [RelayCommand]
    private async Task GoToCheckoutAsync()
    {
        await Shell.Current.GoToAsync(nameof(CheckoutPage));
    }
}

