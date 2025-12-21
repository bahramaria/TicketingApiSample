
using Microsoft.EntityFrameworkCore;

using TicketingSystem.API.Data;
using TicketingSystem.API.Models;
using TicketingSystem.API.Repositories.Interfaces;

namespace TicketingSystem.API.Repositories;

public class TicketRepository(AppDbContext context) : ITicketRepository
{
    public async Task AddAsync(Ticket ticket)
    {
        context.Tickets.Add(ticket);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        var tickets = await context.Tickets.ToListAsync();
        return tickets;
    }

    public async Task<Ticket> GetById(Guid id)
    {
        var ticket = await context.Tickets.FindAsync(id);
        return ticket ?? throw new NullReferenceException();
    }

    public async Task<IEnumerable<Ticket>> GetByUserIdAsync(Guid userId)
    {
        return await context.Tickets
            .Where(t => t.CreatedByUserId == userId)
            .ToListAsync();
    }
}
