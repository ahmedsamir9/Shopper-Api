using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
