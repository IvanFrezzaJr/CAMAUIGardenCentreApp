using SQLite;

namespace CAMAUIGardenCentreApp.Models
{
    [Table("credit_card")]
    public class CreditCard
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }


        [Column("card_holder_name")]
        public string CardholderName { get; set; }

        [Column("card_number")]
        public long CardNumber { get; set; }

        [Column("expiration_date")]
        public string ExpirationDate { get; set; }

        [Column("cvv")]
        public int cvv { get; set; }
    }
}