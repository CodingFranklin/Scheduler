using CommunityToolkit.Mvvm.ComponentModel;

namespace Scheduler.App.ViewModels;

/// <summary>
/// Main ViewModel for the application shell.
/// Will be expanded in later phases with calendar navigation, event lists, etc.
/// </summary>
public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string _welcomeMessage = "Scheduler";

    public MainViewModel()
    {
    }
}
