using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scheduler.App.Data;
using Scheduler.App.Data.Repositories;
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

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // -- DI Configuration Entry Point --
        var services = new ServiceCollection();

        ConfigureServices(services);

        Services = services.BuildServiceProvider();

        // Ensure database and seed data are created before UI loads
        using (var scope = Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<SchedulerDbContext>();
            db.Database.EnsureCreated();

            // Ensure repositories and seed data work
            var categoryRepo = scope.ServiceProvider.GetRequiredService<ICategoryRepository>();
            var categories = await categoryRepo.GetAllAsync();
        }

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
        // Database
        var dbFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Scheduler");
        Directory.CreateDirectory(dbFolder);
        var dbPath = Path.Combine(dbFolder, "scheduler.db");

        services.AddDbContext<SchedulerDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        // Repositories
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        // ViewModels
        services.AddTransient<ViewModels.CalendarViewModel>();
        services.AddTransient<ViewModels.MainViewModel>();

        // Windows
        services.AddTransient<MainWindow>();

        // Future registrations:
        // services.AddSingleton<IcsSyncService>();
    }
}
