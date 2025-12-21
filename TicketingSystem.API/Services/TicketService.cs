using TicketingSystem.API.DTOs;
using TicketingSystem.API.Models;
using TicketingSystem.API.Repositories.Interfaces;
using TicketingSystem.API.Services.Interfaces;

namespace TicketingSystem.API.Services;

public class TicketService(ITicketRepository repository) : ITicketService
{
    public async Task<Ticket> CreateAsync(TicketCreateDto dto, Guid userId)
    {
        var ticket = new Ticket
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            CreatedByUserId = userId
        };

        await repository.AddAsync(ticket);
        return ticket;
    }

    public async Task<IEnumerable<Ticket>> GetAllAsync() => await repository.GetAllAsync();

    public async Task<Ticket> GetByIdAsync(Guid id) => await repository.GetById(id);

    public async Task<IEnumerable<Ticket>> GetByUserIdAsync(Guid userId)
    {
        return await repository.GetByUserIdAsync(userId);
    }
}
