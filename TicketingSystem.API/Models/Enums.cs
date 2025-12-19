using Microsoft.AspNetCore.Mvc;

namespace TicketingSystem.API.Models
{
    public enum UserRole { Employee, Admin }
    public enum TicketStatus { Open, InProgress, Closed }
    public enum TicketPriority { Low, Medium, High }
}
