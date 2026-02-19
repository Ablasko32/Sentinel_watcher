using System.ComponentModel.DataAnnotations;

namespace SentinelWebApi.DTOs.Auth
{
    public class CreateUserDTO
    {
        [EmailAddress]
        public required string Email
        {
            get;
            set=>field= value.Trim();
        }
        public required string Username
        {
            get;
            set=> field = value.Trim();
        }
        [MinLength(6)]
        public required string Password { get; set;  }
      
    }
}
