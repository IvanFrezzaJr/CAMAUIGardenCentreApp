using System.Collections.ObjectModel;
using CAMAUIGardenCentreApp.Models;

namespace CAMAUIGardenCentreApp.Services;

public class BasketService
{
    public ObservableCollection<CartItem> CartItems { get; } = new();

    public void AddToCart(Product product)
    {
        if (product != null)
        {
            CartItem newCartItem = new CartItem();

            newCartItem.Product = product;
             
            CartItems.Add(newCartItem);
        }
    }

    public ObservableCollection<CartItem> GetCartItems()
    {
        return CartItems;
    }
}
