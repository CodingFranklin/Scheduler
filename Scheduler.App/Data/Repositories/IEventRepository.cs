using Scheduler.App.Models;

namespace Scheduler.App.Data.Repositories;

/// <summary>
/// Repository interface for CalendarEvent CRUD operations.
/// </summary>
public interface IEventRepository
{
    Task<List<CalendarEvent>> GetEventsByDateRangeAsync(DateTime start, DateTime end);
    Task<CalendarEvent?> GetByIdAsync(int id);
    Task AddAsync(CalendarEvent calendarEvent);
    Task UpdateAsync(CalendarEvent calendarEvent);
    Task DeleteAsync(int id);
}
