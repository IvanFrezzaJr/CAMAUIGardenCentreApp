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

    public CategoryViewModel(DatabaseContext context)
    {
        _context = context;
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
}
