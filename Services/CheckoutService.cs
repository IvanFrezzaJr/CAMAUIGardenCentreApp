using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAMAUIGardenCentreApp.Models;
using CAMAUIGardenCentreApp.Data;
using System.Diagnostics;


public class CheckoutService
{
    private readonly DatabaseContext _dbContext;

    public CheckoutService(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Checkout> CreateCheckoutAsync(int userId)
    {
        var checkout = new Checkout
        {
            UserId = userId,
            TotalAmount = 0
        };

        int id  = await _dbContext.AddItemAndGetIdAsync(checkout);

        if (id != null)
        {
            checkout.Id = id;
            return checkout;
        } else
        {
            return null;
        }
    }

    public async Task<Checkout> UpdateCheckoutAsync(Checkout checkout)
    {

        bool status = await _dbContext.UpdateItemAsync<Checkout>(checkout);
        if (status)
        {
            return checkout;
        } else
        {
            return null;
        }
  
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


    public async Task<IEnumerable<Checkout>> GetUnpaidCheckoutsAsync(int userId)
    {
        return await _dbContext.GetFileteredAsync<Checkout>(c => c.UserId == userId && !c.IsPaid);
    }

    public async Task<IEnumerable<Checkout>> GetPaidCheckoutsAsync(int userId)
    {
        return await _dbContext.GetFileteredAsync<Checkout>(c => c.UserId == userId && c.IsPaid);
    }


    public async Task UpdateOverdueCheckoutsAsync(int userId, int billingDay)
    {
        var unpaidCheckouts = await GetUnpaidCheckoutsAsync(userId);
        DateTime today = DateTime.Today;

        foreach (var checkout in unpaidCheckouts)
        {
            DateTime nextBillingDate = CalculateNextBillingDate(billingDay);

            if (nextBillingDate < today)
            {
                checkout.IsPaid = true;
                await _dbContext.UpdateItemAsync(checkout);
            }
        }
    }


    public DateTime CalculateNextBillingDate(int billingDay)
    {
        DateTime today = DateTime.Today;
        DateTime thisMonthBilling = new DateTime(today.Year, today.Month, billingDay);


        return today <= thisMonthBilling ? thisMonthBilling : thisMonthBilling.AddMonths(1);
    }

}
