using Microsoft.EntityFrameworkCore;
using TurboNotes.Core.Interfaces;
using TurboNotes.Core.Models;
using TurboNotes.Infrastructure.Data;

namespace TurboNotes.Infrastructure.Repositories;

public  class CategoryRepository(TurboNotesDbContext context) : ICategoryRepository
{
    public async Task<IEnumerable<Category>> GetAllAsync() => await context.Categories.ToListAsync();
    
    public async Task<Category> GetByIdAsync(int id) =>
        await context.Categories.FirstOrDefaultAsync(c => c.Id == id) 
        ?? throw new InvalidOperationException($"Category with id {id} not found");
    
    public async Task AddAsync(Category category)
    {
        context.Categories.Add(category);
        await context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Category category)
    {
        context.Categories.Update(category);
        await context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        if (id == 1)
        {
            throw new InvalidOperationException("Cannot delete the default 'No Category'");
        }

        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var vacancies = await context.Notes
                .Where(v => v.CategoryId == id)
                .ToListAsync();

            foreach (var vacancy in vacancies)
            {
                vacancy.CategoryId = 1;
            }
            
            var category = await context.Categories.FindAsync(id);
            if (category != null)
            {
                context.Categories.Remove(category);
            }

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}