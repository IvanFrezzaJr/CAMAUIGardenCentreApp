using SQLite;
namespace CAMAUIGardenCentreApp.Models;

[Table("checkout")]
public class Checkout
{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("total_amount")]
    public decimal TotalAmount { get; set; }

    [Column("is_paid")]
    public bool IsPaid { get; set; }

    [Column("Created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
