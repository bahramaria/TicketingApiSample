using TicketingSystem.API.DTOs;
using TicketingSystem.API.Models;

namespace TicketingSystem.API.Services.Interfaces;

public interface ITicketService
{
    Task<Ticket> CreateAsync(TicketCreateDto dto, Guid userId);
    Task<IEnumerable<Ticket>> GetAllAsync();
    Task<Ticket> GetByIdAsync(Guid id);
    Task<IEnumerable<Ticket>> GetByUserIdAsync(Guid guid);
}
