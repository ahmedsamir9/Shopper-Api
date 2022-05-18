namespace ShopperAPi.DTOS
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int NumberInStock { get; set; }
        public float Price { get; set; }
        public int CategoryId { get; set; } 
        public string CategoryName { get; set; }

    }
}
