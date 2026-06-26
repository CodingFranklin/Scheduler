using CommunityToolkit.Mvvm.ComponentModel;

namespace Scheduler.App.ViewModels;

/// <summary>
/// Root ViewModel for the main application shell.
/// Hosts the CalendarViewModel for the primary content area.
/// </summary>
public partial class MainViewModel : ObservableObject
{
    /// <summary>
    /// The calendar view model driving the month view.
    /// </summary>
    public CalendarViewModel Calendar { get; }

    public MainViewModel(CalendarViewModel calendarViewModel)
    {
        Calendar = calendarViewModel;
    }
}
