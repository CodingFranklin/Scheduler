using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Scheduler.App.ViewModels;

public partial class CalendarViewModel : ObservableObject
{
    [ObservableProperty]
    private DateTime _currentDate = DateTime.Today;

    [ObservableProperty]
    private string _monthTitle = string.Empty;

    [ObservableProperty]
    private ObservableCollection<CalendarDayViewModel> _days = new();

    /// <summary>
    /// Number of rows in the calendar grid. 5 or 6 depending on the month layout.
    /// </summary>
    [ObservableProperty]
    private int _rowCount = 5;

    public CalendarViewModel()
    {
        GenerateDays();
    }

    [RelayCommand]
    private void PreviousMonth()
    {
        CurrentDate = CurrentDate.AddMonths(-1);
        GenerateDays();
    }

    [RelayCommand]
    private void NextMonth()
    {
        CurrentDate = CurrentDate.AddMonths(1);
        GenerateDays();
    }

    [RelayCommand]
    private void GoToToday()
    {
        CurrentDate = DateTime.Today;
        GenerateDays();
    }

    private void GenerateDays()
    {
        var firstOfMonth = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
        var daysInMonth = DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month);
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
            var date = new DateTime(CurrentDate.Year, CurrentDate.Month, day);
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
        MonthTitle = CurrentDate.ToString("yyyy年 M月");
    }
}
