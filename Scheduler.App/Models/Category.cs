namespace Scheduler.App.Models;

/// <summary>
/// Event category with color tag for visual distinction.
/// </summary>
public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Hex color code, e.g. "#FF6B6B"
    /// </summary>
    public string ColorHex { get; set; } = "#CCCCCC";

    /// <summary>
    /// Display order in sidebar (lower = first).
    /// </summary>
    public int SortOrder { get; set; }

    // Navigation
    public ICollection<CalendarEvent> Events { get; set; } = new List<CalendarEvent>();
}
