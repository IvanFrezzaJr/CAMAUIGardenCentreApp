using CAMAUIGardenCentreApp.Data;
using CAMAUIGardenCentreApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace CAMAUIGardenCentreApp.Services;

public class ProductService
{
    private readonly DatabaseContext _context;

    public ProductService(DatabaseContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.GetAllAsync<Product>();
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        return await _context.DeleteItemByKeyAsync<Product>(id);
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId)
    {
        return await _context.GetFileteredAsync<Product>(p => p.CategoryId == categoryId);
      
    }
}
