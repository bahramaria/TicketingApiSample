using Microsoft.AspNetCore.Mvc;

using TicketingSystem.API.Data;
using TicketingSystem.API.DTOs;
using TicketingSystem.API.Services;
using TicketingSystem.API.Services.Interfaces;

namespace TicketingSystem.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(AppDbContext context, IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null || !authService.VerifyPassword(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = authService.GenerateToken(user);
            return Ok(new { Token = token });
        }
    }
}
