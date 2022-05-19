using System.ComponentModel.DataAnnotations;

namespace ShopperAPi.DTOS.OrderDto
{
    public class AddressDto
    {   [Required(ErrorMessage ="The street is Required")]
        public string Street { get; set; }
        [Required(ErrorMessage = "The City is Required")]
        public string City { get; set; }
        [Required(ErrorMessage = "The State is Required")]
        public string State { get; set; }
        [Required(ErrorMessage = "The Detailed Address is Required")]
        public string Detailed { get; set; }
    }
}
