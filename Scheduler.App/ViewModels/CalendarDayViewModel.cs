using CommunityToolkit.Mvvm.ComponentModel;
using Scheduler.App.Models;
using System.Collections.ObjectModel;

namespace Scheduler.App.ViewModels;

/// <summary>
/// ViewModel for a single day cell in the calendar grid.
/// </summary>
public partial class CalendarDayViewModel : ObservableObject
{
    /// <summary>
    /// True if this cell is a placeholder (before 1st or after last day of month).
    /// </summary>
    public bool IsPlaceholder { get; set; }

    /// <summary>
    /// The date this cell represents. Null for placeholder cells.
    /// </summary>
    public DateTime? Date { get; set; }

    /// <summary>
    /// The day number to display. Null for placeholder cells (shows empty).
    /// </summary>
    public int? DayNumber { get; set; }

    /// <summary>
    /// Always true for real cells in this single-month view.
    /// </summary>
    public bool IsCurrentMonth { get; set; }

    public bool IsToday { get; set; }

    /// <summary>
    /// Events on this day. Reserved for later phases.
    /// </summary>
    public ObservableCollection<CalendarEvent> Events { get; set; } = new();
}
