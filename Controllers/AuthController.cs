using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ThreatLens.Api.DTOs;
using ThreatLens.Api.Services;

namespace ThreatLens.Api.Controllers
{

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        

        public AuthController(IAuthService authService)
        {   
            _authService = authService;  
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var (succeeded, errors) = await _authService.RegisterAsync(
                request.Email,
                request.Password);

            if (!succeeded)
                return BadRequest(errors);

            return Ok(new { message = "User created." });
        }


        [HttpPost("jwt-login")]
        public async Task<IActionResult> JwtLogin(JwtLoginRequestDto request)
        {
            var token = await _authService.LoginAsync(
                request.Email,
                request.Password);

            if (token == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            return Ok(new
            {
                accessToken = token,
                tokenType = "Bearer",
                expiresIn = 1800
            });
        }
    }
}
