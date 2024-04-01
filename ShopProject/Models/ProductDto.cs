namespace ShopProject.Models
{
    public class ProductDto
    {
        public string Name { get; set; } = "";
        public string Brand { get; set; } = "";
        public int Price { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
