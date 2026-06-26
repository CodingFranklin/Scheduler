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

    public void Load(
        DateTime monthDate,
        string monthTitle,
        ObservableCollection<CalendarDayViewModel> days,
        int rowCount)
    {
        MonthDate = monthDate;
        MonthTitle = monthTitle;
        Days = days;
        RowCount = rowCount;
    }
}
