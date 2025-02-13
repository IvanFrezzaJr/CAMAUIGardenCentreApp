using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CAMAUIGardenCentreApp.Data;
using CAMAUIGardenCentreApp.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

using CAMAUIGardenCentreApp.Views;
using CAMAUIGardenCentreApp.Services;

namespace CAMAUIGardenCentreApp.ViewModels;



public partial class DetailViewModel : BaseViewModel
{
    private readonly ProductService _productService;
    private readonly BasketService _basketService;



    public DetailViewModel(ProductService productService, BasketService basketService)
    {
        _productService = productService;
        _basketService = basketService;

    }

    [ObservableProperty]
    private bool _hasItemsInCart;

    [ObservableProperty]
    private int _cartItemCount;


    [ObservableProperty]
    private ObservableCollection<Product> _products = new();

    public async Task LoadProductByIdAsync(int productId)
    {

        //await _loadingService.ShowLoadingWhile(async () =>
        await ExecuteAsync(async () =>
        {
            if (productId <= 0)
                return;

            IsBusy = true;
            Products.Clear();

            var products = await _productService.GetProductById(productId);
            if (products is not null && products.Any())
            {
                Products ??= new ObservableCollection<Product>();

                foreach (var product in products)
                {
                    Products.Add(product);
                }
            }

            UpdateBasket();

            IsBusy = false;

        }, "Fetching products...");
    }



    [RelayCommand]
    private async Task AddToCartAsync(Product product)
    {
        if (product is null)
            return;

        _basketService.AddToCart(product);

        // Update floating basket menu status 
        UpdateBasket();

        //await Shell.Current.DisplayAlert("Basket", $"{product.Name} added to basket!", "OK");
    }

    private void UpdateBasket()
    {
        // Update floating basket menu status 
        HasItemsInCart = _basketService.GetCartItems().Any();
        CartItemCount = _basketService.GetCartItems().Count();
    }




    [RelayCommand]
    private async Task GoToCartAsync()
    {
        await Shell.Current.GoToAsync(nameof(BasketPage));
    }

    [RelayCommand]
    private async Task GoToProfileAsync()
    {
        await Shell.Current.GoToAsync(nameof(ProfilePage));
    }


    [RelayCommand]
    private async Task GoToCategoryAsync()
    {
        await Shell.Current.GoToAsync(nameof(CategoryPage));
    }


}