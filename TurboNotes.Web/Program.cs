using Microsoft.EntityFrameworkCore;
using TurboNotes.Core.Interfaces;
using TurboNotes.Infrastructure.Data;
using TurboNotes.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TurboNotesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TurboNotesDbConnection")));

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

var app = builder.Build();

await SeedData.EnsurePopulatedAsync(app);

// Configure the HTTP request pipeline.
app.UseRouting();

app.UseStaticFiles();

app.MapDefaultControllerRoute();


app.Run();