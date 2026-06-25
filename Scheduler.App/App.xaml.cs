using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Scheduler.App;

/// <summary>
/// Application entry point. Configures the Dependency Injection container
/// before the MainWindow is displayed.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// The DI service provider, accessible throughout the application.
    /// </summary>
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // -- DI Configuration Entry Point --
        // Register all services, ViewModels, and windows here in later phases.
        var services = new ServiceCollection();

        ConfigureServices(services);

        Services = services.BuildServiceProvider();

        // Resolve MainWindow from DI to enable constructor injection
        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    /// <summary>
    /// Register application services. Expand this method in future phases
    /// to add DbContext, repositories, and other services.
    /// </summary>
    private static void ConfigureServices(IServiceCollection services)
    {
        // ViewModels
        services.AddTransient<ViewModels.MainViewModel>();

        // Windows
        services.AddTransient<MainWindow>();

        // Future registrations (examples, commented out for now):
        // services.AddDbContext<SchedulerDbContext>(...);
        // services.AddScoped<IEventRepository, EventRepository>();
        // services.AddScoped<ICategoryRepository, CategoryRepository>();
        // services.AddSingleton<IcsSyncService>();
    }
}
