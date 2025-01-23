using SQLite;
using System.Linq.Expressions;
using CAMAUIGardenCentreApp.Models;
using System.Runtime.Intrinsics.X86;

namespace CAMAUIGardenCentreApp.Services;

public class DatabaseService
{
    private const string DBNAME = "Database.db3";
    private readonly SQLiteAsyncConnection _connection;


    public DatabaseService()
    {
        _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DBNAME));
        _connection.CreateTableAsync<Product>();
    }


    public async Task<List<Product>> GetProducts()
    {
        return await _connection.Table<Product>().ToListAsync();
    }


    public async Task<Product> GetProductById(int id)
    {
        return await _connection.Table<Product>().Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateProduct(Product product)
    {
        await _connection.InsertAsync(product);
    }

    public async Task UpdateProduct(Product product)
    {
        await _connection.UpdateAsync(product);
    }

    public async Task DeleteProduct(Product product)
    {
        await _connection.DeleteAsync(product);
    }

}

