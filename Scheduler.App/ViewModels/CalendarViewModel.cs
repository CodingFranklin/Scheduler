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

    [ObservableProperty]
    private int _rowCount = 5;

    public MonthViewModel PrevMonthData { get; }
    public MonthViewModel NextMonthData { get; }

    public CalendarViewModel()
    {
        PrevMonthData = new MonthViewModel();
        NextMonthData = new MonthViewModel();
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
        GenerateSingleMonth(CurrentDate, out var days, out var rowCount, out var title);

        Days = days;
        RowCount = rowCount;
        MonthTitle = title;

        PrevMonthData.LoadMonth(CurrentDate.AddMonths(-1));
        NextMonthData.LoadMonth(CurrentDate.AddMonths(1));
    }

    private static void GenerateSingleMonth(DateTime month, out ObservableCollection<CalendarDayViewModel> days, out int rowCount, out string title)
    {
        var firstOfMonth = new DateTime(month.Year, month.Month, 1);
        var daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
        var today = DateTime.Today;

        int leadingBlanks = (int)firstOfMonth.DayOfWeek;
        int requiredSlots = leadingBlanks + daysInMonth;
        rowCount = requiredSlots <= 35 ? 5 : 6;
        int totalSlots = rowCount * 7;

        var newDays = new ObservableCollection<CalendarDayViewModel>();

        for (int i = 0; i < leadingBlanks; i++)
            newDays.Add(new CalendarDayViewModel { IsPlaceholder = true, Date = null, DayNumber = null, IsCurrentMonth = false, IsToday = false });

        for (int day = 1; day <= daysInMonth; day++)
        {
            var date = new DateTime(month.Year, month.Month, day);
            newDays.Add(new CalendarDayViewModel { IsPlaceholder = false, Date = date, DayNumber = day, IsCurrentMonth = true, IsToday = date == today });
        }

        while (newDays.Count < totalSlots)
            newDays.Add(new CalendarDayViewModel { IsPlaceholder = true, Date = null, DayNumber = null, IsCurrentMonth = false, IsToday = false });

        days = newDays;
        title = month.ToString("yyyy年 M月");
    }
}
