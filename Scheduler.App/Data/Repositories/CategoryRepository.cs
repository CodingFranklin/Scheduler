using Microsoft.EntityFrameworkCore;
using Scheduler.App.Models;

namespace Scheduler.App.Data.Repositories;

/// <summary>
/// EF Core implementation of ICategoryRepository.
/// </summary>
public class CategoryRepository : ICategoryRepository
{
    private readonly SchedulerDbContext _db;

    public CategoryRepository(SchedulerDbContext db)
    {
        _db = db;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _db.Categories
            .OrderBy(c => c.SortOrder)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _db.Categories.FindAsync(id);
    }

    public async Task AddAsync(Category category)
    {
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        _db.Categories.Update(category);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _db.Categories.FindAsync(id);
        if (entity is not null)
        {
            _db.Categories.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
