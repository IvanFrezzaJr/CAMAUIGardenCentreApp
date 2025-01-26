using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CAMAUIGardenCentreApp.Data;
using CAMAUIGardenCentreApp.Models;
using System.Collections.ObjectModel;


namespace CAMAUIGardenCentreApp.ViewModels;

public partial class ProductListViewModel : ObservableObject
{
    private readonly DatabaseContext _context;


    public ProductListViewModel(DatabaseContext context)
    {
        _context = context;
    }


    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _busyText;

    [ObservableProperty]
    private ObservableCollection<Product> _products = new();


    public async Task LoadProductsByCategory(int categoryId)
    {

        //await _loadingService.ShowLoadingWhile(async () =>
        await ExecuteAsync(async () =>
        {
            if (categoryId <= 0)
                return;

            IsBusy = true;
            Products.Clear();

            var products = await _context.GetFileteredAsync<Product>(p => p.CategoryId == categoryId);
            if (products is not null && products.Any())
            {
                Products ??= new ObservableCollection<Product>();

                foreach (var product in products)
                {
                    Products.Add(product);
                }
            }

            IsBusy = false;

        }, "Fetching products...");
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