using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Employee_CRUD_API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var result = await _authService.LoginAsync(request);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while processing login.");
                throw;
            }
        }
    }
}
