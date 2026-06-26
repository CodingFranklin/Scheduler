using System.Collections.ObjectModel;
using Scheduler.App.ViewModels;

namespace Scheduler.App.Services;

public class CalendarMonthFactory : ICalendarMonthFactory
{
    public MonthViewModel Create(DateTime monthDate)
    {
        var month = new MonthViewModel();
        Load(month, monthDate);
        return month;
    }

    public void Load(MonthViewModel target, DateTime monthDate)
    {
        var firstOfMonth = new DateTime(monthDate.Year, monthDate.Month, 1);
        var daysInMonth = DateTime.DaysInMonth(monthDate.Year, monthDate.Month);
        var today = DateTime.Today;

        var leadingBlanks = (int)firstOfMonth.DayOfWeek;
        var requiredSlots = leadingBlanks + daysInMonth;
        var rowCount = requiredSlots <= 35 ? 5 : 6;
        var totalSlots = rowCount * 7;
        var days = new ObservableCollection<CalendarDayViewModel>();

        for (var i = 0; i < leadingBlanks; i++)
        {
            days.Add(CalendarDayViewModel.CreatePlaceholder());
        }

        for (var day = 1; day <= daysInMonth; day++)
        {
            var date = new DateTime(firstOfMonth.Year, firstOfMonth.Month, day);
            days.Add(CalendarDayViewModel.CreateDate(date, date == today));
        }

        while (days.Count < totalSlots)
        {
            days.Add(CalendarDayViewModel.CreatePlaceholder());
        }

        target.Load(firstOfMonth, firstOfMonth.ToString("yyyy年M月"), days, rowCount);
    }
}
