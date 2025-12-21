
using TicketingSystem.API.Models;

namespace TicketingSystem.API.Repositories.Interfaces;

public interface ITicketRepository
{
    Task AddAsync(Ticket ticket);
    Task<IEnumerable<Ticket>> GetAllAsync();
    Task<Ticket> GetById(Guid id);
    Task<IEnumerable<Ticket>> GetByUserIdAsync(Guid userId);
}
