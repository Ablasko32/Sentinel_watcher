using SentinelCore.DAL.Data.Models;
using SentinelWebApi.DTOs;
using SentinelWebApi.DTOs.Auth;

namespace SentinelWebApi.Mapping.Auth;

public static class AuthExtensions
{
    extension(AppUser user)
    {
        public UserDTO ToUserDTO(IList<string> roles)
        {
            return new UserDTO
            {
                Id = user.Id,
                Username = user.UserName!,
                Email = user.Email!,
                Role = roles.FirstOrDefault() ?? AppRoles.User
            };
        }
    }

    extension(CreateUserDTO dto)
    {
        public AppUser ToAppUser()
        {
            return new AppUser
            {
                UserName = dto.Username,
                Email = dto.Email,
                EmailConfirmed = true
            };
        }
    }
}