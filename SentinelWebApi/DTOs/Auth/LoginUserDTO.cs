using System.ComponentModel.DataAnnotations;

namespace SentinelWebApi.DTOs
{
    public class LoginUserDTO
    {
        [Required]
        [EmailAddress]
        public required string Email
        {
            get;
            set => field = value.Trim();
        }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }
    
    }
}