using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TurboNotes.Core.Models;
using TurboNotes.Core.Services;

namespace TurboNotes.Infrastructure.Data;

public static class SeedData
{
    public static async Task EnsurePopulatedAsync(IApplicationBuilder app)
    {
        var context = app.ApplicationServices
            .CreateScope().ServiceProvider
            .GetRequiredService<TurboNotesDbContext>();
        
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Id = 1, Name = "No category" },
                new Category { Id = 2, Name = "Home" },
                new Category { Id = 3, Name = "Work" },
                new Category { Id = 4, Name = "Education" }
            );
            await context.SaveChangesAsync();
        }
        
        if (!context.Notes.Any())
        {
            var random = new Random();
            var yesterday = DateTime.Now.AddDays(-1);

            for (var i = 1; i <= 13; i++)
            {
                var localDeadline = yesterday.AddDays(random.Next(0, 10)).AddHours(random.Next(0, 24));
                
                var note = new Note
                {
                    Title = $"Test note {i}",
                    Content = $"Test note content {i}",
                    CreatedAt = TimeService.GetCurrentUtcTime(),
                    Deadline = TimeService.ToUtc(localDeadline),
                    CategoryId = random.Next(1, 5)
                };

                context.Notes.Add(note);
            }
            await context.SaveChangesAsync();
        }
    }
}