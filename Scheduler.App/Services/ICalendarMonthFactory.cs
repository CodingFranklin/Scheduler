using Scheduler.App.ViewModels;

namespace Scheduler.App.Services;

public interface ICalendarMonthFactory
{
    MonthViewModel Create(DateTime monthDate);

    void Load(MonthViewModel target, DateTime monthDate);
}
