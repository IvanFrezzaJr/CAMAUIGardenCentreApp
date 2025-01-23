using SQLite;
using System.Linq.Expressions;
using CAMAUIGardenCentreApp.Models;

namespace CAMAUIGardenCentreApp.Data
{
    public class DatabaseContext : IAsyncDisposable
    {
        private const string DbName = "MyDatabase.db3";
        private static string DbPath => Path.Combine(FileSystem.AppDataDirectory, DbName);

        private SQLiteAsyncConnection _connection;
        private SQLiteAsyncConnection Database =>
            (_connection ??= new SQLiteAsyncConnection(DbPath,
                SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache));


        public DatabaseContext()
        {
            // Calls the method to check and delete the database if it exists
            DeleteDatabaseIfExists().Wait();
        }

        // Method to delete the database if it already exists
        private async Task DeleteDatabaseIfExists()
        {
            if (File.Exists(DbPath))
            {
                try
                {
                    File.Delete(DbPath); // Delete the database file
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting the database: {ex.Message}");
                }
            }
        }

        private async Task CreateTableIfNotExists<TTable>() where TTable : class, new()
        {
            await Database.CreateTableAsync<TTable>();
        }

        private async Task<AsyncTableQuery<TTable>> GetTableAsync<TTable>() where TTable : class, new()
        {
            await CreateTableIfNotExists<TTable>();
            return Database.Table<TTable>();
        }

        public async Task<IEnumerable<TTable>> GetAllAsync<TTable>() where TTable : class, new()
        {
            var table = await GetTableAsync<TTable>();
            return await table.ToListAsync();
        }

        public async Task<IEnumerable<TTable>> GetFileteredAsync<TTable>(Expression<Func<TTable, bool>> predicate) where TTable : class, new()
        {
            var table = await GetTableAsync<TTable>();
            return await table.Where(predicate).ToListAsync();
        }

        private async Task<TResult> Execute<TTable, TResult>(Func<Task<TResult>> action) where TTable : class, new()
        {
            await CreateTableIfNotExists<TTable>();
            return await action();
        }

        public async Task<TTable> GetItemByKeyAsync<TTable>(object primaryKey) where TTable : class, new()
        {
            //await CreateTableIfNotExists<TTable>();
            //return await Database.GetAsync<TTable>(primaryKey);
            return await Execute<TTable, TTable>(async () => await Database.GetAsync<TTable>(primaryKey));
        }

        public async Task<bool> AddItemAsync<TTable>(TTable item) where TTable : class, new()
        {
            //await CreateTableIfNotExists<TTable>();
            //return await Database.InsertAsync(item) > 0;
            return await Execute<TTable, bool>(async () => await Database.InsertAsync(item) > 0);
        }

        public async Task<bool> UpdateItemAsync<TTable>(TTable item) where TTable : class, new()
        {
            await CreateTableIfNotExists<TTable>();
            return await Database.UpdateAsync(item) > 0;
        }

        public async Task<bool> DeleteItemAsync<TTable>(TTable item) where TTable : class, new()
        {
            await CreateTableIfNotExists<TTable>();
            return await Database.DeleteAsync(item) > 0;
        }

        public async Task<bool> DeleteItemByKeyAsync<TTable>(object primaryKey) where TTable : class, new()
        {
            await CreateTableIfNotExists<TTable>();
            return await Database.DeleteAsync<TTable>(primaryKey) > 0;
        }

       
        public async Task InitProduct()
        {
            var produtosExistentes = await GetAllAsync<Product>();

            if (!produtosExistentes.Any())
            {
                var produtos = new List<Product>
                {
                    new Product
                    {
                        Name = "Product 1",
                        Price = 10.99m,
                        Quantity = 50,
                        CategoryId = 1,
                        ImageUrl = "https://example.com/product1.jpg"
                    },
                    new Product
                    {
                        Name = "Product 2",
                        Price = 20.99m,
                        Quantity = 30,
                        CategoryId = 2,
                        ImageUrl = "https://example.com/product2.jpg"
                    },
                    new Product
                    {
                        Name = "Product 3",
                        Price = 15.50m,
                        Quantity = 70,
                        CategoryId = 1,
                        ImageUrl = "https://example.com/product3.jpg"
                    },
                    new Product
                    {
                        Name = "Product 4",
                        Price = 7.99m,
                        Quantity = 120,
                        CategoryId = 3,
                        ImageUrl = "https://example.com/product4.jpg"
                    },
                    new Product
                    {
                        Name = "Product 5",
                        Price = 12.50m,
                        Quantity = 60,
                        CategoryId = 2,
                        ImageUrl = "https://example.com/product5.jpg"
                    }
                };

                foreach (var produto in produtos)
                {
                    await AddItemAsync(produto);
                }
            }
        }

        public async ValueTask DisposeAsync() => await _connection?.CloseAsync();

    }
}