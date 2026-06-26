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
    /// True if this cell is a placeholder before or after the visible month.
    /// </summary>
    public bool IsPlaceholder { get; set; }

    /// <summary>
    /// The date this cell represents. Null for placeholder cells.
    /// </summary>
    public DateTime? Date { get; set; }

    /// <summary>
    /// The day number to display. Null for placeholder cells.
    /// </summary>
    public int? DayNumber { get; set; }

    /// <summary>
    /// Always true for real cells in this single-month view.
    /// </summary>
    public bool IsCurrentMonth { get; set; }

    public bool IsToday { get; set; }

    /// <summary>
    /// TODO items on this day. Reserved for the next data-loading phase.
    /// </summary>
    public ObservableCollection<CalendarEvent> Events { get; set; } = new();

    public static CalendarDayViewModel CreatePlaceholder()
    {
        return new CalendarDayViewModel
        {
            IsPlaceholder = true,
            Date = null,
            DayNumber = null,
            IsCurrentMonth = false,
            IsToday = false
        };
    }

    public static CalendarDayViewModel CreateDate(DateTime date, bool isToday)
    {
        return new CalendarDayViewModel
        {
            IsPlaceholder = false,
            Date = date,
            DayNumber = date.Day,
            IsCurrentMonth = true,
            IsToday = isToday
        };
    }
}
