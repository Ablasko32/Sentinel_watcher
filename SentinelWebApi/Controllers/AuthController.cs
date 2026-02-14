using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SentinelCore.DAL.Data.Models;
using SentinelWebApi.DTOs;
using SentinelWebApi.DTOs.ApiResponse;
using SentinelWebApi.DTOs.Auth;
using SentinelWebApi.Mapping.Auth;
using static SentinelCore.DAL.Data.Models.AppRoles;

namespace SentinelWebApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AuthController> _logger;

        public AuthController(SignInManager<AppUser> signInManager, ILogger<AuthController> logger, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<UserDTO>>> LoginUserAsync([FromBody] LoginUserDTO dto)
        {
            try
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(dto.Email);
                if (user == null)
                {
                    return NotFound(ApiResponse.ApiError("Account not found"));
                }

                var result = await _signInManager.PasswordSignInAsync(user, dto.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    return Ok(ApiResponse<UserDTO>.ApiSuccess(user.ToUserDTO(roles),"Login successful"));
                }
                else
                {
                    return Unauthorized(ApiResponse.ApiError("Invalid email or password"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to login the user.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse.ApiError("Unexpected error occurred while trying to login"));
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> LogoutUserAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok(ApiResponse.ApiSuccess("Logged out successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to log out the user.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse.ApiError("Unexpected error occurred while trying to logout"));
            }
        }

        [HttpDelete("{deleteUserId}")]
        [Authorize(Roles = Admin)]
        public async Task<ActionResult<ApiResponse>> DeleteUserAsync(string deleteUserId)
        {
            try
            {
                var userToDelete = await _userManager.FindByIdAsync(deleteUserId);
                if (userToDelete == null)
                {
                    return NotFound(ApiResponse.ApiError("User not found"));
                }

                var isDeletingAdmin = await _userManager.IsInRoleAsync(userToDelete, Admin);
                if (isDeletingAdmin)
                {
                    return StatusCode(StatusCodes.Status403Forbidden,
                        ApiResponse.ApiError("Cannot delete admin users"));
                }

                var result = await _userManager.DeleteAsync(userToDelete);
                if (result.Succeeded)
                {
                    return Ok(ApiResponse.ApiSuccess("User deleted successfully"));
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        ApiResponse.ApiError("Unexpected error occurred while trying to delete the user"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to delete the user.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse.ApiError("Unexpected error occurred while trying to delete the user"));
            }
        }

        [HttpGet("status")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserDTO>>> CheckAuthAsync()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized(ApiResponse.ApiError("User not authenticated"));
                }
                var roles = await _userManager.GetRolesAsync(user);

                return Ok(ApiResponse<UserDTO>.ApiSuccess(user.ToUserDTO(roles)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to check authentication.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse.ApiError("Unexpected error occurred while trying to check authentication"));
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = Admin)]
        public async Task<ActionResult<ApiResponse>> CreateUserAsync([FromBody] CreateUserDTO dto)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(dto.Email);
                if (existingUser != null)
                {
                    return Conflict(ApiResponse.ApiError("A user with the provided email already exists"));
                }
                var newUser = dto.ToAppUser();
                var result = await _userManager.CreateAsync(newUser);
                if (result.Succeeded)
                {
                    return Ok(ApiResponse.ApiSuccess("User created successfully"));
                }
                else
                {
                    throw new Exception("Identity failed to create the user.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to create a new user.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse.ApiError("Unexpected error occurred while trying to create a new user"));
            }
        }
    }
}