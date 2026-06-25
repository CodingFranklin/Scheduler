namespace Scheduler.App.Models;

/// <summary>
/// A calendar event / todo item with a time range and optional category.
/// </summary>
public class CalendarEvent
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int CategoryId { get; set; }

    // Navigation
    public Category Category { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime ModifiedAt { get; set; } = DateTime.Now;
}
