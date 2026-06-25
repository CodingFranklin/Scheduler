using Microsoft.EntityFrameworkCore;
using Scheduler.App.Models;

namespace Scheduler.App.Data.Repositories;

/// <summary>
/// EF Core implementation of IEventRepository.
/// </summary>
public class EventRepository : IEventRepository
{
    private readonly SchedulerDbContext _db;

    public EventRepository(SchedulerDbContext db)
    {
        _db = db;
    }

    public async Task<List<CalendarEvent>> GetEventsByDateRangeAsync(DateTime start, DateTime end)
    {
        return await _db.Events
            .Include(e => e.Category)
            .Where(e => e.StartTime < end && e.EndTime > start)
            .OrderBy(e => e.StartTime)
            .ToListAsync();
    }

    public async Task<CalendarEvent?> GetByIdAsync(int id)
    {
        return await _db.Events
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddAsync(CalendarEvent calendarEvent)
    {
        calendarEvent.CreatedAt = DateTime.Now;
        calendarEvent.ModifiedAt = DateTime.Now;
        _db.Events.Add(calendarEvent);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(CalendarEvent calendarEvent)
    {
        calendarEvent.ModifiedAt = DateTime.Now;
        _db.Events.Update(calendarEvent);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _db.Events.FindAsync(id);
        if (entity is not null)
        {
            _db.Events.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
