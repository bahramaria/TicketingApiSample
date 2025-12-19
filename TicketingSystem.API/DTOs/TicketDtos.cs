using TicketingSystem.API.Models;

namespace TicketingSystem.API.DTOs
{
    public class TicketCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketPriority Priority { get; set; }
    }

    public class TicketUpdateDto
    {
        public TicketStatus Status { get; set; }
        public Guid? AssignedToUserId { get; set; }
    }

    public class TicketStatsDto
    {
        public int Open { get; set; }
        public int InProgress { get; set; }
        public int Closed { get; set; }
    }
}
