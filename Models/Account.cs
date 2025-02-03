using SQLite;

namespace CAMAUIGardenCentreApp.Models
{
    [Table("account")]
    public class Account
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("company_name")]
        public string CompanyName { get; set; }

        [Column("company_tax_id")]
        public string CompanyTaxID { get; set; }

        [Column("billing_email")]
        public string BillingEmail { get; set; }

        [Column("billing_day")]
        public int BillingDay { get; set; }


    }
}