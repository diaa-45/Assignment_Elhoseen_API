namespace Assignment_Elhoseen_API.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Category { get; set; } = "";
        public string Code { get; set; } = ""; 

        public string Name { get; set; } = "";

        public string ImageUrl { get; set; } = "";

        public decimal Price { get; set; }

        public int MinQuantity { get; set; }

        public double DiscountRate { get; set; }
    }
}
