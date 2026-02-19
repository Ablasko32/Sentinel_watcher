namespace SentinelWebApi.DTOs.Auth
{
    public class UpdateUserDTO
    {
        public string? Email { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? UserName { get; set; }
    }
}