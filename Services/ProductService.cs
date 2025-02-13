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


    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _context.GetAllAsync<Category>();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId)
    {
        return await _context.GetFileteredAsync<Product>(p => p.CategoryId == categoryId);
      
    }

    public async Task<IEnumerable<Product>> GetProductById(int productId)
    {
        return await _context.GetFileteredAsync<Product>(p => p.Id == productId);

    }


    public async Task<IEnumerable<Category>> GeCategoryById(int categoryId)
    {
        return await _context.GetFileteredAsync<Category>(p => p.Id == categoryId);

    }


    public async Task<IEnumerable<Category>> GeCategoryByName(string categoryName)
    {
        return await _context.GetFileteredAsync<Category>(p => p.Name == categoryName);

    }
}
