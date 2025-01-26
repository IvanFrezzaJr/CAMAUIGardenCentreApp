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
            var productExists = await GetAllAsync<Product>();

            if (!productExists.Any())
            {
                var product = new List<Product>
                {
                    new Product
                    {
                        Name = "Monstera Deliciosa",
                        Price = 25.99m,
                        Quantity = 50,
                        CategoryId = 1,
                        ImageUrl = "/data/data/com.CAMAUIGardenCentreApp/files/MonsteraDeliciosa.jpg"
                    },
                    new Product
                    {
                        Name = "Snake Plant (Sansevieria)",
                        Price = 20.99m,
                        Quantity = 30,
                        CategoryId = 1,
                        ImageUrl = "/data/data/com.CAMAUIGardenCentreApp/files/SnakePlantSansevieria.jpg"
                    },
                    new Product
                    {
                        Name = "Lavender Plant",
                        Price = 15.50m,
                        Quantity = 70,
                        CategoryId = 1,
                        ImageUrl = "/data/data/com.CAMAUIGardenCentreApp/files/LavenderPlant.jpg"
                    },
                    new Product
                    {
                        Name = "Bonsai Tree (Ficus Retusa)",
                        Price = 40.99m,
                        Quantity = 120,
                        CategoryId = 1,
                        ImageUrl = "/data/data/com.CAMAUIGardenCentreApp/files/BonsaiTreeFicusRetusa.jpg"
                    },
                    new Product
                    {
                        Name = "Orchid (Phalaenopsis)",
                        Price = 30.50m,
                        Quantity = 60,
                        CategoryId = 1,
                        ImageUrl = "/data/data/com.CAMAUIGardenCentreApp/files/OrchidPhalaenopsis.jpg"
                    },
                    new Product
                    {
                        Name = "Garden Pruning Shears",
                        Price = 18.50m,
                        Quantity = 60,
                        CategoryId = 2,
                        ImageUrl = "/data/data/com.CAMAUIGardenCentreApp/files/GardenPruningShears.jpg"
                    },
                    new Product
                    {
                        Name = "Stainless Steel Trowel",
                        Price = 12.50m,
                        Quantity = 20,
                        CategoryId = 2,
                        ImageUrl = "/data/data/com.CAMAUIGardenCentreApp/files/StainlessSteelTrowel.jpg"
                    },
                    new Product
                    {
                        Name = "Hand Rake",
                        Price = 10.50m,
                        Quantity = 60,
                        CategoryId = 2,
                        ImageUrl = "/data/data/com.CAMAUIGardenCentreApp/files/HandRake.jpg"
                    },
                    new Product
                    {
                        Name = "Garden Hoe",
                        Price = 25.50m,
                        Quantity = 40,
                        CategoryId = 2,
                        ImageUrl = "/data/data/com.CAMAUIGardenCentreApp/files/GardenHoe.jpg"
                    },
                    new Product
                    {
                        Name = "Multipurpose Garden Knife",
                        Price = 22.50m,
                        Quantity = 100,
                        CategoryId = 2,
                        ImageUrl = "/data/data/com.CAMAUIGardenCentreApp/files/MultipurposeGardenKnife.jpg"
                    },
                    new Product
                    {
                        Name = "Organic Fertilizer (5kg)",
                        Price = 30.50m,
                        Quantity = 10,
                        CategoryId = 3,
                        ImageUrl = "/data/data/com.CAMAUIGardenCentreApp/files/OrganicFertilizer.jpg"
                    },
                    new Product
                    {
                        Name = "Compost Bin (50L)",
                        Price = 45.50m,
                        Quantity = 120,
                        CategoryId = 3,
                        ImageUrl = "/data/data/com.CAMAUIGardenCentreApp/files/CompostBin.jpg"
                    },
                    new Product
                    {
                        Name = "Drip Irrigation Kit",
                        Price = 35.50m,
                        Quantity = 100,
                        CategoryId = 3,
                        ImageUrl = "/data/data/com.CAMAUIGardenCentreApp/files/DripIrrigationKit.jpg"
                    },
                    new Product
                    {
                        Name = "Natural Pest Repellent Spray (1L)",
                        Price = 15.50m,
                        Quantity = 75,
                        CategoryId = 3,
                        ImageUrl = "/data/data/com.CAMAUIGardenCentreApp/files/NaturalPestRepellentSpray.jpg"
                    },
                    new Product
                    {
                        Name = "Mulch Bag (20kg)",
                        Price = 25.50m,
                        Quantity = 25,
                        CategoryId = 3,
                        ImageUrl = "/data/data/com.CAMAUIGardenCentreApp/files/MulchBag.jpg"
                    }

                };

                foreach (var produto in product)
                {
                    await AddItemAsync(produto);
                }
            }
        }


        public async Task InitCategory()
        {
            var categoryExists = await GetAllAsync<Category>();

            if (!categoryExists.Any())
            {
                var product = new List<Category>
                {
                    new Category
                    {
                        Name = "Plants"
                    },
                    new Category
                    {
                        Name = "Tools"
                    },
                    new Category
                    {
                        Name = "Garden Care"
                    }
                };

                foreach (var prod in product)
                {
                    await AddItemAsync(prod);
                }
            }
        }


        public async ValueTask DisposeAsync() => await _connection?.CloseAsync();

    }
}