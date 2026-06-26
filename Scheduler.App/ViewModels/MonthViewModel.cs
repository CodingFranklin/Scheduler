using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Scheduler.App.ViewModels;

public partial class MonthViewModel : ObservableObject
{
    public DateTime MonthDate { get; private set; }

    [ObservableProperty]
    private string _monthTitle = string.Empty;

    [ObservableProperty]
    private ObservableCollection<CalendarDayViewModel> _days = new();

    [ObservableProperty]
    private int _rowCount = 5;

    public MonthViewModel() { }

    public MonthViewModel(DateTime monthDate)
    {
        LoadMonth(monthDate);
    }

    public void LoadMonth(DateTime monthDate)
    {
        MonthDate = new DateTime(monthDate.Year, monthDate.Month, 1);
        MonthTitle = MonthDate.ToString("yyyy年 M月");

        var firstOfMonth = MonthDate;
        var daysInMonth = DateTime.DaysInMonth(MonthDate.Year, MonthDate.Month);
        var today = DateTime.Today;

        int leadingBlanks = (int)firstOfMonth.DayOfWeek;
        int requiredSlots = leadingBlanks + daysInMonth;
        int rowCount = requiredSlots <= 35 ? 5 : 6;
        int totalSlots = rowCount * 7;

        var newDays = new ObservableCollection<CalendarDayViewModel>();

        for (int i = 0; i < leadingBlanks; i++)
        {
            newDays.Add(new CalendarDayViewModel
            {
                IsPlaceholder = true, Date = null, DayNumber = null,
                IsCurrentMonth = false, IsToday = false
            });
        }

        for (int day = 1; day <= daysInMonth; day++)
        {
            var date = new DateTime(MonthDate.Year, MonthDate.Month, day);
            newDays.Add(new CalendarDayViewModel
            {
                IsPlaceholder = false, Date = date, DayNumber = day,
                IsCurrentMonth = true, IsToday = date == today
            });
        }

        while (newDays.Count < totalSlots)
        {
            newDays.Add(new CalendarDayViewModel
            {
                IsPlaceholder = true, Date = null, DayNumber = null,
                IsCurrentMonth = false, IsToday = false
            });
        }

        Days = newDays;
        RowCount = rowCount;
    }
}
