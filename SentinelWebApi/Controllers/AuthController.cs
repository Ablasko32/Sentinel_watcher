using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SentinelCore.DAL.Data.Models;
using SentinelCore.DTOs.Pagination;
using SentinelWebApi.DTOs;
using SentinelWebApi.DTOs.ApiResponse;
using SentinelWebApi.DTOs.Auth;
using SentinelWebApi.DTOs.Filters;
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
                    return Ok(ApiResponse<UserDTO>.ApiSuccess(user.ToUserDTO(roles), "Login successful"));
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
                var rolesResult = await _userManager.AddToRoleAsync(newUser, AppRoles.User);

                if (result.Succeeded && rolesResult.Succeeded)
                {
                    return Ok(ApiResponse.ApiSuccess("User created successfully"));
                }
                else
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)) + " " +
                                        string.Join(", ", rolesResult.Errors.Select(e => e.Description)));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to create a new user.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse.ApiError("Unexpected error occurred while trying to create a new user"));
            }
        }

        [HttpGet("users")]
        [Authorize(Roles = Admin)]
        public async Task<ActionResult<ApiResponse<PaginatedResult<UserDTO>>>> GetAllUsersAsync([FromQuery] BaseSearchFilter filter)
        {
            try
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(AppRoles.User);

                var totalCount = usersInRole.Count;

                var query = usersInRole.AsQueryable();

                if (!String.IsNullOrEmpty(filter.SearchTerm))
                {
                    var searchTerm = filter.SearchTerm.ToLower();
                    query = query.Where(u => (u.UserName != null && u.UserName.Contains(searchTerm)) 
                                              || (u.Email !=null && u.Email.Contains(searchTerm)));
                }

                if (filter.SortOrder == "asc")
                {
                    query = filter.SortField switch
                    {
                        "email" => query.OrderBy(u => u.Email),
                        "username" => query.OrderBy(u => u.UserName),
                        _ => query.OrderBy(u => u.UserName),
                    };
                }
                else
                {
                    query = filter.SortField switch
                    {
                        "email" => query.OrderByDescending(u => u.Email),
                        "username" => query.OrderByDescending(u => u.UserName),
                        _ => query.OrderBy(u => u.UserName),
                    };
                }

                var pagedUsers = query.Skip(filter.Skip).Take(filter.Take).Select(u => new UserDTO
                {
                    Email = u.Email!,
                    Username = u.UserName!,
                    Id = u.Id,
                    Role = AppRoles.User
                }).ToList();

                PaginatedResult<UserDTO> paginatedResult = new PaginatedResult<UserDTO>
                {
                    Items = pagedUsers,
                    TotalCount = totalCount,
                    Page = (filter.Skip / filter.Take) + 1,
                    PageSize = filter.Take,
                    TotalPages = (int)Math.Ceiling((double)totalCount / filter.Take)
                };

                return ApiResponse<PaginatedResult<UserDTO>>.ApiSuccess(paginatedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fething users");
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse.ApiError("Error fething users"));
            }
        }

        [HttpPut("update/{userId}")]
        [Authorize(Roles =Admin)]
        public async Task<ActionResult<ApiResponse>> UpdateUserAsync([FromBody] UpdateUserDTO dto, string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return BadRequest(ApiResponse.ApiError("User not found"));
                }
                if (!String.IsNullOrEmpty(dto.Email) && user.Email != dto.Email)
                {
                    user.Email = dto.Email;
                    user.NormalizedEmail = dto.Email.ToUpper();

                    var changeMailResult = await _userManager.UpdateAsync(user);
                    if (!changeMailResult.Succeeded)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError,
                            ApiResponse.ApiError("Unexpected error occurred while trying to change the email"));
                    }
                }
                if (!String.IsNullOrEmpty(dto.UserName) && dto.UserName != user.UserName)
                {
                    user.UserName = dto.UserName;
                    var usernameResult = await _userManager.UpdateAsync(user);
                    if (!usernameResult.Succeeded)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError,
                           ApiResponse.ApiError("Unexpected error occurred while trying to change the username"));
                    }
                }
                if (!String.IsNullOrEmpty(dto.NewPassword))
                {
                    user = await _userManager.FindByIdAsync(userId);

                    await _userManager.RemovePasswordAsync(user);
                    var addResult = await _userManager.AddPasswordAsync(user, dto.NewPassword);
                    if (!addResult.Succeeded)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError,
                            ApiResponse.ApiError(string.Join(", ", addResult.Errors.Select(e => e.Description))));
                    }
                }
                return ApiResponse.ApiSuccess("User updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to update the user.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse.ApiError("Unexpected error occurred while trying to update the user"));
            }
        }

        [HttpGet("email-exists")]
        [Authorize]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                var exists = user != null;
                if (!exists)
                {
                    return Ok(ApiResponse<bool>.ApiSuccess(exists,"Email avaliable"));
                }
                else return Ok(ApiResponse<bool>.ApiSuccess(exists,"User exists"));
            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error checking email for {email}", email);
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse.ApiError("Unexpected error trying to check email."));
            }
        }
    }
}