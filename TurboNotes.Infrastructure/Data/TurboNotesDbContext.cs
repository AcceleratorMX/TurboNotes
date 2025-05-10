using Microsoft.EntityFrameworkCore;
using TurboNotes.Core.Models;

namespace TurboNotes.Infrastructure.Data;

public class TurboNotesDbContext(DbContextOptions<TurboNotesDbContext> options) : DbContext(options)
{
    public DbSet<Note> Notes { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "No category" },
            new Category { Id = 2, Name = "Home" },
            new Category { Id = 3, Name = "Work" },
            new Category { Id = 4, Name = "Education" }
        );
    }
}