using System.ComponentModel.DataAnnotations;

namespace SentinelWebApi.DTOs
{
    public class LoginUserDTO
    {
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public required string Email { get; set; }
     
        [Required(ErrorMessage = "Paswword is required")]
        public required string Password { get; set; }
    }
}