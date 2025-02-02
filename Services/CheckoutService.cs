using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAMAUIGardenCentreApp.Models;
using CAMAUIGardenCentreApp.Data;

public class CheckoutService
{
    private readonly DatabaseContext _dbContext;

    public CheckoutService(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<int> CreateCheckoutAsync(int userId)
    {
        var checkout = new Checkout
        {
            UserId = userId,
            TotalAmount = 0
        };
        return checkout.Id;
    }

    public async Task<bool> AddItemAsync(int checkoutId, string productName, int quantity, decimal price)
    {
        var item = new CheckoutItem
        {
            CheckoutId = checkoutId,
            ProductName = productName,
            Quantity = quantity,
            Price = price,
            AddedAt = DateTime.UtcNow
        };
        return await _dbContext.AddItemAsync(item);
    }

    public async Task<List<CheckoutItem>> GetItemsAsync()
    {
        var items = await _dbContext.GetAllAsync<CheckoutItem>();
        return new List<CheckoutItem>(items);
    }

    public async Task<bool> RemoveItemAsync(int id)
    {
        var item = await _dbContext.GetItemByKeyAsync<CheckoutItem>(id);
        if (item != null)
        {
            return await _dbContext.DeleteItemAsync(item);
        }
        return false;
    }

    public async Task<bool> ClearCheckoutAsync()
    {
        var items = await _dbContext.GetAllAsync<CheckoutItem>();
        foreach (var item in items)
        {
            await _dbContext.DeleteItemAsync(item);
        }
        return true;
    }
}
