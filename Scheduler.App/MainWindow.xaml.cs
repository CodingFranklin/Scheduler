using System.Windows;

namespace Scheduler.App;

/// <summary>
/// Main application window. Injected with MainViewModel via DI.
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(ViewModels.MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
