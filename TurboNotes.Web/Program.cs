using Microsoft.EntityFrameworkCore;
using TurboNotes.Core.Interfaces;
using TurboNotes.Core.Services;
using TurboNotes.Infrastructure.Data;
using TurboNotes.Infrastructure.Notifications;
using TurboNotes.Infrastructure.Repositories;
using TurboNotes.Web.Hubs;
using TurboNotes.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

builder.Services.AddDbContext<TurboNotesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TurboNotesDbConnection")));

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<INoteService, NoteService>();

builder.Services.AddSingleton<INotificationStrategy, ExpiredNotificationStrategy>();
builder.Services.AddSingleton<INotificationStrategy, OneMinuteNotificationStrategy>();
builder.Services.AddSingleton<INotificationStrategy, OneHourNotificationStrategy>();
builder.Services.AddSingleton<INotificationStrategy, OneDayNotificationStrategy>();
builder.Services.AddSingleton<INotificationStrategy, OneWeekNotificationStrategy>();
builder.Services.AddSingleton<INotificationStrategy, MoreThanWeekNotificationStrategy>();

builder.Services.AddSingleton<NotificationStrategyContext>();

builder.Services.AddSingleton<INotificationSender, SignalRNotificationSender>();

builder.Services.AddHostedService<DeadlineNotificationService>();


var app = builder.Build();

await SeedData.EnsurePopulatedAsync(app);

// Configure the HTTP request pipeline.
app.UseRouting();

app.UseStaticFiles();

app.MapDefaultControllerRoute();

app.MapHub<NotificationHub>("/notificationHub");

app.Run();