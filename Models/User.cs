using SQLite;

namespace CAMAUIGardenCentreApp.Models
{
    [Table("user")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("type")]
        public string Type { get; set; }
    }
}