using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        [Required]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Full Name Length must be between 8 and 50 characters")]
        public string? FullName { get; set; }
    }
}
