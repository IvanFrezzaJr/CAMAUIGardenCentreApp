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


public partial class ProductListViewModel : BaseViewModel
{
    private readonly ProductService _productService;
    private readonly BasketService _basketService;


    public ProductListViewModel(ProductService productService, BasketService basketService)
    {
        _productService = productService;
        _basketService = basketService;

    }


    [ObservableProperty]
    private ObservableCollection<Product> _products = new();

    [ObservableProperty]
    private bool _hasItemsInCart;

    [ObservableProperty]
    private int _cartItemCount;

    [ObservableProperty]
    private string _categoryName;

    [ObservableProperty]
    private string _categoryImage;

    [ObservableProperty]
    private string _categoryDescription;



    public async Task LoadProductsByCategoryAsync(int categoryId)
    {

        //await _loadingService.ShowLoadingWhile(async () =>
        await ExecuteAsync(async () =>
        {
            if (categoryId <= 0)
                return;

            IsBusy = true;
            Products.Clear();

            var castegoryResult = await _productService.GeCategoryById(categoryId);
            Category? category = castegoryResult.FirstOrDefault();
            CategoryName = category.Name;
            CategoryImage = category.ImageUrl;
            CategoryDescription = category.Description;


            var products = await _productService.GetProductsByCategoryId(categoryId);
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