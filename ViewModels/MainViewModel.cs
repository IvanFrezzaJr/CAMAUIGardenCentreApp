using CAMAUIGardenCentreApp.Data;
using CAMAUIGardenCentreApp.Models;
using CAMAUIGardenCentreApp.Services;
using CAMAUIGardenCentreApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CAMAUIGardenCentreApp.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly ProductService _productService;
    private readonly BasketService _cartService;
    private readonly LoadingService _loadingService;


    public MainViewModel(ProductService productService, BasketService cartService, LoadingService loadingService)
    {
        _productService = productService;
        _cartService = cartService;
        _loadingService = loadingService;
    }


    [ObservableProperty]
    private ObservableCollection<Product> _products = new();


    [ObservableProperty]
    private bool _hasItemsInCart;

    [ObservableProperty]
    private int _cartItemCount;

    public async Task LoadProductsAsync()
    {

        //await _loadingService.ShowLoadingWhile(async () =>
        await ExecuteAsync(async () =>
        {
            var products = await _productService.GetAllProductsAsync();
            if (products?.Any() == true)
            {
                Products = new ObservableCollection<Product>(products);
            }
        }, "Fetching products...");
    }

    [RelayCommand]
    private async Task AddToCartAsync(Product product)
    {
        if (product is null)
            return;

        _cartService.AddToCart(product);

        // Update floating basket menu status 
        HasItemsInCart = _cartService.GetCartItems().Any();
        CartItemCount = _cartService.GetCartItems().Count();

        await Shell.Current.DisplayAlert("Basket", $"{product.Name} added to basket!", "OK");
    }


    [RelayCommand]
    private async Task GoToCartAsync()
    {
        await Shell.Current.GoToAsync(nameof(BasketPage));
    }
}