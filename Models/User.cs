using SQLite;

namespace CAMAUIGardenCentreApp.Models;

[Table("user")]
public class User
{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public int Id { get; set; }

    [Column("Name")]
    public string Name { get; set; }

    [Column("phone")]
    public string Phone { get; set; }

    [Column("type")]
    public string Type { get; set; }
}
