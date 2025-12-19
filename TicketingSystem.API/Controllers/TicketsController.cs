using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

using TicketingSystem.API.Data;
using TicketingSystem.API.DTOs;
using TicketingSystem.API.Models;

namespace TicketingSystem.API.Controllers;

[Authorize]
[ApiController]
[Route("Tickets")]
public class TicketsController(AppDbContext context) : ControllerBase
{
    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [Authorize(Roles = "Employee")]
    [HttpPost]
    public async Task<IActionResult> Create(TicketCreateDto dto)
    {
        var ticket = new Ticket
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            CreatedByUserId = GetUserId()
        };
        context.Tickets.Add(ticket);
        await context.SaveChangesAsync();
        return Ok(ticket);
    }

    [Authorize(Roles = "Employee")]
    [HttpGet("My")]
    public async Task<IActionResult> GetMyTickets()
        => Ok(await context.Tickets.Where(t => t.CreatedByUserId == GetUserId()).ToListAsync());

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await context.Tickets.ToListAsync());

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, TicketUpdateDto dto)
    {
        var ticket = await context.Tickets.FindAsync(id);
        if (ticket == null) return NotFound();

        ticket.Status = dto.Status;
        ticket.AssignedToUserId = dto.AssignedToUserId;
        ticket.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return Ok(ticket);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var stats = new TicketStatsDto
        {
            Open = await context.Tickets.CountAsync(t => t.Status == TicketStatus.Open),
            InProgress = await context.Tickets.CountAsync(t => t.Status == TicketStatus.InProgress),
            Closed = await context.Tickets.CountAsync(t => t.Status == TicketStatus.Closed)
        };
        return Ok(stats);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var ticket = await context.Tickets.FindAsync(id);
        if (ticket == null) return NotFound();

        var userId = GetUserId();
        var isAdmin = User.IsInRole("Admin");

        if (ticket.CreatedByUserId != userId && ticket.AssignedToUserId != userId && !isAdmin)
            return Forbid();

        return Ok(ticket);
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ticket = await context.Tickets.FindAsync(id);
        if (ticket == null) return NotFound();

        context.Tickets.Remove(ticket);
        await context.SaveChangesAsync();

        return NoContent();
    }
}