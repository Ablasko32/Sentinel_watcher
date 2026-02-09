using Microsoft.AspNetCore.Mvc;
using SentinelCore.DTOs.ModelDTOs;
using SentinelCore.DTOs.Pagination;
using SentinelWebApi.DTOs.ApiResponse;
using SentinelWebApi.DTOs.Filters;
using SentinelWebApi.Services;

namespace SentinelWebApi.Controllers
{
    [ApiController]
    [Route("/log-errors")]
    public class LogErrorsController : ControllerBase
    {
        private readonly ILogErrorService _logErrorService;
        private readonly ILogger<LogErrorsController> _logger;

        public LogErrorsController(ILogger<LogErrorsController> logger, ILogErrorService logErrorService)
        {
            _logger = logger;
            _logErrorService = logErrorService;
        }

        [HttpGet("filtered-list")]
        public async Task<ActionResult<ApiResponse<PaginatedResult<LogErrorDTO>>>> GetErrorListAsync([FromQuery] LogErrorListFilter filter, CancellationToken cancellationToken)
        {
            try
            {
                var errors = await _logErrorService.GetErrorListAsync(filter, cancellationToken);
                return Ok(ApiResponse<PaginatedResult<LogErrorDTO>>.ApiSuccess(errors));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the error list.");
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<LogErrorDTO>.ApiError(new[] { "An error occurred while retrieving the error list." }));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<LogErrorDTO>>> GetErrorByIdAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                var error = await _logErrorService.GetErrorByIdAsync(id, cancellationToken);
                if (error == null)
                {
                    return NotFound(ApiResponse<LogErrorDTO>.ApiError(new[] { $"Error with {id} was not found." }));
                }

                return Ok(ApiResponse<LogErrorDTO>.ApiSuccess(error));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the error with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<LogErrorDTO>.ApiError(new[] { $"An error occurred while retrieving the error with ID {id}." }));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteErrorAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                var success = await _logErrorService.DeleteErrorAsync(id, cancellationToken);
                if (!success)
                {
                    return NotFound(ApiResponse<LogErrorDTO>.ApiError(new[] { $"Error with {id} was not found." }));
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the error with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<LogErrorDTO>.ApiError(new[] { $"An error occurred while deleting the error with ID {id}." }));
            }
        }
    }
}