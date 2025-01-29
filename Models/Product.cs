using SQLite;

namespace CAMAUIGardenCentreApp.Models
{
    [Table("product")]
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }


        private string _imageUrl;

        [Column("image_url")]
        public string ImageUrl {
            get {
                return _imageUrl;
            }
            set
            {
                _imageUrl = value; 
            } 
        }
    }
}