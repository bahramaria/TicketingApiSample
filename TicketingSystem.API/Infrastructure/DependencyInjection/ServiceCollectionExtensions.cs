using Microsoft.EntityFrameworkCore;

using TicketingSystem.API.Data;
using TicketingSystem.API.Services;
using TicketingSystem.API.Services.Interfaces;

namespace TicketingSystem.API.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=ticketing.db"));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITicketService, TicketService>();
        }
    }
}
