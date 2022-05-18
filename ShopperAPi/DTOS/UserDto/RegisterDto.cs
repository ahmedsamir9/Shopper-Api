using System.ComponentModel.DataAnnotations;
namespace ShopperAPi.DTOS
{
    public class RegisterDto
    {
        [Required]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Full Name must contain alphabetic characters and spaces only")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Full Name Length must be between 8 and 50 characters")]
        public string? FullName { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required, StringLength(16, MinimumLength = 8, ErrorMessage = "User Name Length must be between 8 and 16 characters")]
        public string? UserName { get; set; }

        [Phone]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "Phone Number Length must be between 6 and 16 characters")]
        public string? PhoneNumber { get; set; }

        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$", ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non alphanumeric and at least 6 characters")]
        public string? Password { get; set; }
    }
}
