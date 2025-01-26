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
    private readonly CartService _cartService;

    public CheckoutViewModel(DatabaseContext context, CartService cartService)
    {
        _context = context;
        _cartService = cartService;
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
        await Shell.Current.GoToAsync(nameof(SuccessPage));
    }
}