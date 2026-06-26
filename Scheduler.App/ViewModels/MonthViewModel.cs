using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Scheduler.App.ViewModels;

/// <summary>
/// ViewModel for a single month block in the calendar pager.
/// Contains the month date, title, and the 7×6 day grid.
/// </summary>
public partial class MonthViewModel : ObservableObject
{
    public DateTime MonthDate { get; private set; }

    [ObservableProperty]
    private string _monthTitle = string.Empty;

    [ObservableProperty]
    private ObservableCollection<CalendarDayViewModel> _days = new();

    public MonthViewModel() { }

    public MonthViewModel(DateTime monthDate)
    {
        LoadMonth(monthDate);
    }

    /// <summary>
    /// Regenerate the day grid for the given month.
    /// </summary>
    public void LoadMonth(DateTime monthDate)
    {
        MonthDate = new DateTime(monthDate.Year, monthDate.Month, 1);
        MonthTitle = MonthDate.ToString("yyyy年 M月");

        var firstOfMonth = MonthDate;
        var daysInMonth = DateTime.DaysInMonth(MonthDate.Year, MonthDate.Month);
        var today = DateTime.Today;

        int leadingBlanks = (int)firstOfMonth.DayOfWeek;
        var newDays = new ObservableCollection<CalendarDayViewModel>();

        // Leading placeholders
        for (int i = 0; i < leadingBlanks; i++)
        {
            newDays.Add(new CalendarDayViewModel
            {
                IsPlaceholder = true,
                Date = null,
                DayNumber = null,
                IsCurrentMonth = false,
                IsToday = false
            });
        }

        // Real days
        for (int day = 1; day <= daysInMonth; day++)
        {
            var date = new DateTime(MonthDate.Year, MonthDate.Month, day);
            newDays.Add(new CalendarDayViewModel
            {
                IsPlaceholder = false,
                Date = date,
                DayNumber = day,
                IsCurrentMonth = true,
                IsToday = date == today
            });
        }

        // Trailing placeholders to 42
        while (newDays.Count < 42)
        {
            newDays.Add(new CalendarDayViewModel
            {
                IsPlaceholder = true,
                Date = null,
                DayNumber = null,
                IsCurrentMonth = false,
                IsToday = false
            });
        }

        Days = newDays;
    }
}
