using SQLite;
using System.Linq.Expressions;
using CAMAUIGardenCentreApp.Models;
using System.Diagnostics;

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
            //DeleteDatabaseIfExists().Wait();
        }

        // Method to delete the database if it already exists
        private async Task DeleteDatabaseIfExists()
        {
            if (File.Exists(DbPath))
            {
                try
                {

                    Debug.WriteLine(" Deleting the database file ...");
                    Debug.WriteLine(DbPath);
                    File.Delete(DbPath); // Delete the database file
                    Debug.WriteLine(" Database file deleted.");
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

        public async Task<int> AddItemAndGetIdAsync<TTable>(TTable item) where TTable : class, new()
        {
            return await Execute<TTable, int>(async () =>
            {
                await Database.InsertAsync(item);
                var pkProperty = typeof(TTable).GetProperties().FirstOrDefault(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).Any());
                return pkProperty != null ? (int)pkProperty.GetValue(item) : 0;
            });
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
                        ImageUrl = "monstera_deliciosa.png"
                    },
                    new Product
                    {
                        Name = "Snake Plant (Sansevieria)",
                        Price = 20.99m,
                        Quantity = 30,
                        CategoryId = 1,
                        ImageUrl = "anake_plant_sansevieria.png"
                    },
                    new Product
                    {
                        Name = "Lavender Plant",
                        Price = 15.50m,
                        Quantity = 70,
                        CategoryId = 1,
                        ImageUrl = "lavender_plant.png"
                    },
                    new Product
                    {
                        Name = "Bonsai Tree (Ficus Retusa)",
                        Price = 40.99m,
                        Quantity = 120,
                        CategoryId = 1,
                        ImageUrl = "bonsai_tree_ficus_retusa.png"
                    },
                    new Product
                    {
                        Name = "Orchid (Phalaenopsis)",
                        Price = 30.50m,
                        Quantity = 60,
                        CategoryId = 1,
                        ImageUrl = "orchid_phalaenopsis.png"
                    },
                    new Product
                    {
                        Name = "Garden Pruning Shears",
                        Price = 18.50m,
                        Quantity = 60,
                        CategoryId = 2,
                        ImageUrl = "garden_pruning_shears.png"
                    },
                    new Product
                    {
                        Name = "Stainless Steel Trowel",
                        Price = 12.50m,
                        Quantity = 20,
                        CategoryId = 2,
                        ImageUrl = "stainless_steel_trowel.png"
                    },
                    new Product
                    {
                        Name = "Hand Rake",
                        Price = 10.50m,
                        Quantity = 60,
                        CategoryId = 2,
                        ImageUrl = "hand_rake.png"
                    },
                    new Product
                    {
                        Name = "Garden Hoe",
                        Price = 25.50m,
                        Quantity = 40,
                        CategoryId = 2,
                        ImageUrl = "garden_hoe.png"
                    },
                    new Product
                    {
                        Name = "Multipurpose Garden Knife",
                        Price = 22.50m,
                        Quantity = 100,
                        CategoryId = 2,
                        ImageUrl = "multipurpose_garden_knife.png"
                    },
                    new Product
                    {
                        Name = "Organic Fertilizer (5kg)",
                        Price = 30.50m,
                        Quantity = 10,
                        CategoryId = 3,
                        ImageUrl = "organic_fertilizer.png"
                    },
                    new Product
                    {
                        Name = "Compost Bin (50L)",
                        Price = 45.50m,
                        Quantity = 120,
                        CategoryId = 3,
                        ImageUrl = "compost_bin.png"
                    },
                    new Product
                    {
                        Name = "Drip Irrigation Kit",
                        Price = 35.50m,
                        Quantity = 100,
                        CategoryId = 3,
                        ImageUrl = "drip_irrigation_kit.png"
                    },
                    new Product
                    {
                        Name = "Natural Pest Repellent Spray (1L)",
                        Price = 15.50m,
                        Quantity = 75,
                        CategoryId = 3,
                        ImageUrl = "natural_pest_repellent_spray.png"
                    },
                    new Product
                    {
                        Name = "Mulch Bag (20kg)",
                        Price = 25.50m,
                        Quantity = 25,
                        CategoryId = 3,
                        ImageUrl = "mulch_bag.png"
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
                        Name = "Plants",
                        ImageUrl = "banner1.jpg",
                        Description = "Bring life to your garden with our diverse selection of plants. From vibrant flowers to lush greenery and exotic species, we have the perfect plants to suit any space, indoors or outdoors."
                    },
                    new Category
                    {
                        Name = "Tools",
                        ImageUrl = "banner2.jpg",
                        Description = "Make gardening easier with our high-quality tools. Whether you're planting, pruning, or landscaping, our durable and ergonomic tools help you get the job done with precision and ease."
                    },
                    new Category
                    {
                        Name = "Garden Care",
                        ImageUrl = "banner3.jpg",
                        Description = "Keep your garden thriving with our essential care products. From fertilizers and pest control to soil enhancers and watering solutions, we provide everything you need for a healthy and beautiful garden."
                    }
                };

                foreach (var prod in product)
                {
                    await AddItemAsync(prod);
                }
            }
        }


        public async Task InitUser()
        {
            var userExists = await GetAllAsync<User>();

            if (!userExists.Any())
            {
                var user = new List<User>
                {
                    new User
                    {      
                        Login = "ivan",
                        Password = PasswordHasher.HashPassword("1234"),
                        Type = "personal"
                    },
                    new User
                    {
                        Login = "bruna",
                        Password = PasswordHasher.HashPassword("1234"),
                        Type = "corporate"
                    }
                };
                foreach (var u in user)
                {
                    await AddItemAsync(u);
                }
            }
        }



        public async ValueTask DisposeAsync() => await _connection?.CloseAsync();

    }
}