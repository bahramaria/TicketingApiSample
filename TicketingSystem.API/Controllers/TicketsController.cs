using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Runtime.InteropServices;
using System.Security.Claims;

using TicketingSystem.API.Data;
using TicketingSystem.API.DTOs;
using TicketingSystem.API.Models;
using TicketingSystem.API.Services;
using TicketingSystem.API.Services.Interfaces;

namespace TicketingSystem.API.Controllers;

[Authorize]
[ApiController]
[Route("Tickets")]
public class TicketsController(AppDbContext context, ITicketService ticketService) : ControllerBase
{
    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [Authorize(Roles = "Employee")]
    [HttpPost]
    public async Task<IActionResult> Create(TicketCreateDto dto)
    {
        var ticket = await ticketService.CreateAsync(dto, GetUserId());
        return Ok(ticket);
    }

    [Authorize(Roles = "Employee")]
    [HttpGet("My")]
    public async Task<IActionResult> GetMyTickets()
    {
        var tickets = await ticketService.GetByUserIdAsync(GetUserId());
        return Ok(tickets);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await ticketService.GetAllAsync());

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, TicketUpdateDto dto)
    {
        var ticket = await ticketService.GetByIdAsync(id);
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