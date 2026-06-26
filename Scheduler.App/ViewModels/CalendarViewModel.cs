using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Scheduler.App.Services;

namespace Scheduler.App.ViewModels;

public partial class CalendarViewModel : ObservableObject
{
    private readonly ICalendarMonthFactory _monthFactory;

    [ObservableProperty]
    private DateTime _currentDate = DateTime.Today;

    [ObservableProperty]
    private string _monthTitle = string.Empty;

    public MonthViewModel PrevMonthData { get; }

    public MonthViewModel CurrentMonthData { get; }

    public MonthViewModel NextMonthData { get; }

    public CalendarViewModel(ICalendarMonthFactory monthFactory)
    {
        _monthFactory = monthFactory;
        PrevMonthData = _monthFactory.Create(CurrentDate.AddMonths(-1));
        CurrentMonthData = _monthFactory.Create(CurrentDate);
        NextMonthData = _monthFactory.Create(CurrentDate.AddMonths(1));
        MonthTitle = CurrentMonthData.MonthTitle;
    }

    [RelayCommand]
    private void PreviousMonth()
    {
        CurrentDate = CurrentDate.AddMonths(-1);
        LoadVisibleMonths();
    }

    [RelayCommand]
    private void NextMonth()
    {
        CurrentDate = CurrentDate.AddMonths(1);
        LoadVisibleMonths();
    }

    [RelayCommand]
    private void GoToToday()
    {
        CurrentDate = DateTime.Today;
        LoadVisibleMonths();
    }

    private void LoadVisibleMonths()
    {
        _monthFactory.Load(PrevMonthData, CurrentDate.AddMonths(-1));
        _monthFactory.Load(CurrentMonthData, CurrentDate);
        _monthFactory.Load(NextMonthData, CurrentDate.AddMonths(1));
        MonthTitle = CurrentMonthData.MonthTitle;
    }
}
