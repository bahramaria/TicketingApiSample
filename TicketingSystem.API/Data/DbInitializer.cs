using Microsoft.AspNetCore.Mvc;

using TicketingSystem.API.Services;
using TicketingSystem.API.Models;

namespace TicketingSystem.API.Data;

public static class DbInitializer
{
    public static void Seed(AppDbContext context, AuthService authService)
    {
        context.Database.EnsureCreated();
        if (context.Users.Any()) return;

        var admin = new User
        {
            FullName = "System Admin",
            Email = "admin@org.com",
            PasswordHash = authService.HashPassword("Admin123"),
            Role = UserRole.Admin
        };

        var employee = new User
        {
            FullName = "John Employee",
            Email = "john@org.com",
            PasswordHash = authService.HashPassword("User123"),
            Role = UserRole.Employee
        };

        context.Users.AddRange(admin, employee);
        context.SaveChanges();
    }
}
