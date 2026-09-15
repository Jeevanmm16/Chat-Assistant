using System.Threading.Tasks;
using ChatAssistantCore.DTO.Auth;
using ChatAssistantCore.Service.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChatAssistantCore.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register User", Description = "Registers a new user and returns a JWT token.")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AuthResponseDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Username and Password are required.");

        var response = await _authService.RegisterAsync(request);
        return Ok(response);
    }

    [HttpPost("login")]
    [SwaggerOperation(Summary = "Login User", Description = "Authenticates a user and returns a JWT token.")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AuthResponseDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Username and Password are required.");

        var response = await _authService.LoginAsync(request);
        return Ok(response);
    }
}
