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

    public void RemoveFromCart(int productId)
    {
        var cartItem = CartItems.FirstOrDefault(i => i.Product.Id == productId);
        if (cartItem != null)
        {
            CartItems.Remove(cartItem);
        }
    }


    public void CleanCart()
    {
        CartItems.Clear();
    }


    public ObservableCollection<CartItem> GetCartItems()
    {
        return CartItems;
    }

    public decimal GetTotalPrice()
    {
        return CartItems.Sum(item => item.Product.Price * item.Quantity);
    }


    public void IncreaseQuantity(int productId)
    {
        var cartItem = CartItems.FirstOrDefault(i => i.Product.Id == productId);
        if (cartItem != null)
        {
            cartItem.Quantity++; 
        }
    }


    public int GetTotalQuantity(int productId)
    {
        var cartItem = CartItems.FirstOrDefault(i => i.Product.Id == productId);
        if (cartItem != null)
        {
            return cartItem.Quantity;
        }
        return 0;
    }


    public void DecreaseQuantity(int productId)
    {
        var cartItem = CartItems.FirstOrDefault(i => i.Product.Id == productId);
        if (cartItem != null)
        {
            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
            }
        }
    }
}
