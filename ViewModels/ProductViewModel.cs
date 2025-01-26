using CAMAUIGardenCentreApp.Data;
using CAMAUIGardenCentreApp.Models;
using CAMAUIGardenCentreApp.Services;
using CAMAUIGardenCentreApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CAMAUIGardenCentreApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly DatabaseContext _context;
    private readonly CartService _cartService;
    private readonly LoadingService _loadingService;


    public MainViewModel(DatabaseContext context, CartService cartService, LoadingService loadingService)
    {
        _context = context;
        _cartService = cartService;
        _loadingService = loadingService;
    }




    [ObservableProperty]
    private ObservableCollection<Product> _products = new();

    [ObservableProperty]
    private Product _operatingProduct = new();

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _busyText;

    [ObservableProperty]
    private bool _hasItemsInCart;

    [ObservableProperty]
    private int _cartItemCount;

    public async Task LoadProductsAsync()
    {

        //await _loadingService.ShowLoadingWhile(async () =>
        await ExecuteAsync(async () =>
        {
            var products = await _context.GetAllAsync<Product>();
            if (products is not null && products.Any())
            {
                Products ??= new ObservableCollection<Product>();

                foreach (var product in products)
                {
                    Products.Add(product);
                }
            }
        }, "Fetching products...");
    }

    [RelayCommand]
    private async Task AddToCartAsync(Product product)
    {
        if (product is null)
            return;

        _cartService.AddToCart(product);

        // Atualiza o estado do botão flutuante
        HasItemsInCart = _cartService.GetCartItems().Any();
        CartItemCount = _cartService.GetCartItems().Count();

        await Shell.Current.DisplayAlert("Carrinho", $"{product.Name} foi adicionado ao carrinho!", "OK");
    }


    [RelayCommand]
    private async Task DeleteProductAsync(int id)
    {
        await ExecuteAsync(async () =>
        {
            if (await _context.DeleteItemByKeyAsync<Product>(id))
            {
                var product = Products.FirstOrDefault(p => p.Id == id);
                Products.Remove(product);
            }
            else
            {
                await Shell.Current.DisplayAlert("Delete Error", "Product was not deleted", "Ok");
            }
        }, "Deleting product...");
    }

    [RelayCommand]
    private async Task GoToCartAsync()
    {
        await Shell.Current.GoToAsync(nameof(CartPage));
    }

    private async Task ExecuteAsync(Func<Task> operation, string? busyText = null)
    {
        IsBusy = true;
        BusyText = busyText ?? "Processing...";
        try
        {
            await operation?.Invoke();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $" {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            BusyText = "Processing...";
        }
    }
}