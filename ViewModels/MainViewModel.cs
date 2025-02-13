using CAMAUIGardenCentreApp.Data;
using CAMAUIGardenCentreApp.Models;
using CAMAUIGardenCentreApp.Services;
using CAMAUIGardenCentreApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CAMAUIGardenCentreApp.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly ProductService _productService;
    private readonly BasketService _basketService;
    private readonly LoadingService _loadingService;


    public MainViewModel(ProductService productService, BasketService basketService, LoadingService loadingService)
    {
        _productService = productService;
        _basketService = basketService;
        _loadingService = loadingService;


        //Items.Add(new CarouselItem { ImageUrl = "banner1.jpg", Title = "Plants", Description = "Bring life to your garden with our diverse selection of plants. From vibrant flowers to lush greenery and exotic species, we have the perfect plants to suit any space, indoors or outdoors." });
        //Items.Add(new CarouselItem { ImageUrl = "banner2.jpg", Title = "Tools", Description = "Make gardening easier with our high-quality tools. Whether you're planting, pruning, or landscaping, our durable and ergonomic tools help you get the job done with precision and ease." });
        //Items.Add(new CarouselItem { ImageUrl = "banner3.jpg", Title = "Garden Care", Description = "Keep your garden thriving with our essential care products. From fertilizers and pest control to soil enhancers and watering solutions, we provide everything you need for a healthy and beautiful garden." });

    }

    [ObservableProperty]
    private int _position; 


    //[ObservableProperty]
    //private ObservableCollection<CarouselItem> _items = new();


    [ObservableProperty]
    private ObservableCollection<Product> _products = new();


    [ObservableProperty]
    private ObservableCollection<Category> _categories = new();


    [ObservableProperty]
    private bool _hasItemsInCart;

    [ObservableProperty]
    private int _cartItemCount;


    public async Task LoadCarouselItemsAsync()
    {
        //await _loadingService.ShowLoadingWhile(async () =>
        await ExecuteAsync(async () =>
        {
            var categories = await _productService.GetAllCategoriesAsync();
            if (categories?.Any() == true)
            {
                Categories = new ObservableCollection<Category>(categories);
            }
        }, "Fetching categories...");
    }

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

        UpdateBasket();
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


    [RelayCommand]
    private async Task OpenProductListPage(Category categoryItem)
    {
        if (categoryItem != null)
        {


            var categoryRaw = await _productService.GeCategoryByName(categoryItem.Name);


            if (categoryRaw != null)
            {
                Category category = categoryRaw.FirstOrDefault();
                await Shell.Current.GoToAsync($"{nameof(ProductListPage)}?category_id={category.Id}");

            }

            Debug.WriteLine($"category not found: {categoryItem.Name}");

        }
    }


    [RelayCommand]
    private async Task OpenDetailPage(Product productItem)
    {

        if (productItem != null)
        {


            var productRaw = await _productService.GetProductById(productItem.Id);

            if (productRaw != null)
            {
                Product product = productRaw.FirstOrDefault();

                await Shell.Current.GoToAsync($"{nameof(DetailPage)}?product_id={product.Id}");

            }

            Debug.WriteLine($"category not found: {productItem.Name}");

        }
    }


    partial void OnPositionChanged(int oldValue, int newValue)
    {
        // Corrige flickering forçando a atualização da posição
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Task.Delay(10);
            Position = newValue;
        });
    }
}