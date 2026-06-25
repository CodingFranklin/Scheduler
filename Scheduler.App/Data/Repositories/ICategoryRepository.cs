using Scheduler.App.Models;

namespace Scheduler.App.Data.Repositories;

/// <summary>
/// Repository interface for Category CRUD operations.
/// </summary>
public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(int id);
}
