using SQLite;
namespace CAMAUIGardenCentreApp.Models;

[Table("checkout_item")]
public class CheckoutItem
{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public int Id { get; set; }
        
    [Column("checkout_id")]
    public int CheckoutId { get; set; }

    [Column("product_name")]
    public string ProductName { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

    [Column("added_at")]
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
