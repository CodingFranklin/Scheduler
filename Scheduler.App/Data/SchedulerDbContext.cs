using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Scheduler.App.Models;

namespace Scheduler.App.Data;

/// <summary>
/// EF Core database context backed by SQLite.
/// </summary>
public class SchedulerDbContext : DbContext
{
    public DbSet<CalendarEvent> Events => Set<CalendarEvent>();
    public DbSet<Category> Categories => Set<Category>();

    private readonly string _dbPath = string.Empty;

    public SchedulerDbContext()
    {
        var folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Scheduler");
        Directory.CreateDirectory(folder);
        _dbPath = Path.Combine(folder, "scheduler.db");
    }

    public SchedulerDbContext(DbContextOptions<SchedulerDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite($"Data Source={_dbPath}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---- CalendarEvent ----
        modelBuilder.Entity<CalendarEvent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);

            entity.HasOne(e => e.Category)
                  .WithMany(c => c.Events)
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict); // prevent deleting category with events
        });

        // ---- Category ----
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(50);
            entity.Property(c => c.ColorHex).HasMaxLength(9);
        });

        // ---- Seed default categories ----
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Work",     ColorHex = "#FF6B6B", SortOrder = 0 },
            new Category { Id = 2, Name = "Personal", ColorHex = "#4ECDC4", SortOrder = 1 },
            new Category { Id = 3, Name = "Study",    ColorHex = "#45B7D1", SortOrder = 2 }
        );
    }
}
