using System.ComponentModel.DataAnnotations;

namespace SentinelWebApi.DTOs
{
    public class LoginUserDTO
    {
        [EmailAddress]
        public required string Email
        {
            get;
            set => field = value.Trim();
        }

        [MinLength(6)]
        public required string Password { get; set; }
    
    }
}