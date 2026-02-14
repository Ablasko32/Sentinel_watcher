using System.ComponentModel.DataAnnotations;

namespace SentinelWebApi.DTOs.Auth
{
    public class CreateUserDTO
    {
        [Required]
        [EmailAddress]
        public required string Email
        {
            get;
            set=>field= value.Trim();
        }
        [Required]
        public required string Username
        {
            get;
            set=> field = value.Trim();
        }
        [Required]
        [MinLength(6)]
        public required string Password { get; set;  }
      
    }
}
