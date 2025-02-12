using CAMAUIGardenCentreApp.Data;
using CAMAUIGardenCentreApp.Models;
using CAMAUIGardenCentreApp.Services;
using CAMAUIGardenCentreApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CAMAUIGardenCentreApp.ViewModels;

public partial class CategoryViewModel : ObservableObject
{
    private readonly DatabaseContext _context;
    private readonly BasketService _basketService;

    public CategoryViewModel(DatabaseContext context, BasketService basketService)
    {
        _context = context;
        _basketService = basketService;
    }

    [ObservableProperty]
    private ObservableCollection<Category> _categories = new();


    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _busyText;

    [ObservableProperty]
    private bool _hasItemsInCart;

    [ObservableProperty]
    private int _cartItemCount;


    public async Task LoadCategoriesAsync()
    {
        await ExecuteAsync(async () =>
        {
            var categories = await _context.GetAllAsync<Category>();
            if (categories is not null && categories.Any())
            {
                Categories = new ObservableCollection<Category>();

                foreach (var product in categories)
                {
                    Categories.Add(product);
                }
            }
        }, "Fetching categories...");

        UpdateBasket();
    }

    private void UpdateBasket()
    {
        // Update floating basket menu status 
        HasItemsInCart = _basketService.GetCartItems().Any();
        CartItemCount = _basketService.GetCartItems().Count();
    }

    [RelayCommand]
    private async Task GoToProductAsync(int categoryId)
    {
        if (categoryId <= 0)
            return;

        await Shell.Current.GoToAsync($"{nameof(ProductListPage)}?category_id={categoryId}");
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

    [RelayCommand]
    private async Task GoToCartAsync()
    {
        if (Shell.Current.CurrentPage is BasketPage)
            return;

        await Shell.Current.GoToAsync("BasketPage");

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

        await Shell.Current.GoToAsync("//CategoryPage");

        Shell.Current.FlyoutIsPresented = false;
    }

}
