namespace Assignment_Elhoseen_API.DTOs
{
    public class ProductUpdateDto
    {
        public string Category { get; set; } = "";
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public int MinQuantity { get; set; }
        public double DiscountRate { get; set; }
        public IFormFile? Image { get; set; } 
    }
}
