using Microsoft.EntityFrameworkCore;

using TicketingSystem.API.Models;

namespace TicketingSystem.API.Data
{
    public interface IAppDbContext
    {
        DbSet<Ticket> Tickets { get; set; }
        DbSet<User> Users { get; set; }
    }
}