using TicketingSystem.API.Models;

namespace TicketingSystem.API.Services.Interfaces
{
    public interface IAuthService
    {
        string GenerateToken(User user);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}