using System.ComponentModel.DataAnnotations;

namespace ShopperAPi.DTOS.ProductDTOs
{
    public class ProductMainpulationsDto
    {
       [Required(ErrorMessage ="The Product Name is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Product Description is Required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "The Product Price is Required")]
        public float Price { get; set; }
        [Required(ErrorMessage = "The Product Image is Required")]
        public IFormFile ImageFile { get; set; }
        [Required(ErrorMessage = "The Product NumberInStock is Required")]
        public int NumberInStock { get; set; }
        [Required(ErrorMessage = "The Product Category is Required")]
        public int CategoryId { get; set; }
    }
}
