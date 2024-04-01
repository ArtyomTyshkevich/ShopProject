using System.ComponentModel.DataAnnotations;

namespace ShopProject.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Brand { get; set; } = "";
        public int Price { get; set; }
        public string? ImageFileName { get; set; }
    }
}
