# TurboNotes

TurboNotes is a web application based on ASP.NET Core MVC 9.0 for creating, managing, and tracking notes with deadlines. Users can create notes, assign categories, set deadlines, and receive notifications about approaching or passed deadlines via SignalR. The project is built using modern design patterns (MVC, MVVM, Repository, Strategy) and SOLID principles, ensuring modularity, testability, and ease of extension.

## Features

- **Note Management**: Create, edit, delete notes with support for categories and deadlines.
- **Real-time Notifications**: Background service sends notifications via SignalR for key deadline thresholds (≤60 minutes, ≤1 minute, ≤0, >7 days, etc.).
- **Modular Architecture**:
  - MVC for web application organization (controllers, models, views)
  - MVVM for client-side logic (JavaScript with SignalR)
  - Repository Pattern for data access abstraction
  - Strategy Pattern for notification threshold handling
- **Thread Safety**: Using lock and Dictionary for one-time notification sending when threshold changes
- **Asynchronous Processing**: Background service with delay for proper SignalR operation
- **Flexible Data Handling**: Separate transmission of NoteTitle in notifications for client-side processing

## Technology Stack

- **Backend**: ASP.NET Core 9.0, C#
- **Frontend**: JavaScript, SignalR for real-time notifications
- **Database**: Entity Framework Core (SQL Server)
- **Dependencies**:
  - Microsoft.AspNetCore.SignalR
  - Microsoft.EntityFrameworkCore.Design
  - Microsoft.EntityFrameworkCore.SqlServer
  - Microsoft.EntityFrameworkCore.Tools
- **DI**: Built-in ASP.NET Core dependency system

## Architecture

The project is structured according to SOLID principles and design patterns:

- **Core**: Models and Interfaces (INoteRepository, INotificationSender, INotificationStrategy) and business logic (NotificationStrategyContext, TimeService)
- **Infrastructure**: Implementations Repository, Notifications of notification strategies (ExpiredNotificationStrategy, MoreThanWeekNotificationStrategy, etc.) and repositories (NoteRepository)
- **Web**: Web application including controllers (MVC), SignalR hub (NotificationHub), background service (DeadlineNotificationService), and client JavaScript (MVVM)

## Applied Design Patterns

### MVC (Model-View-Controller)
- **Role**: Organization of server-side logic for the web application
- **Implementation**:
  - Models: Note, Category for data representation
  - Controllers: NotesController for request handling (note operations) and CategoriesController (category operations)
  - Views: Razor pages for data display
- **Effect**: Clear separation of processing logic, data, and display

### MVVM (Model-View-ViewModel)
- **Role**: Organization of client-side logic for notes, categories, and navigation menu

### Repository Pattern
- **Role**: Abstraction of data access for notes and categories
- **Implementation**: NoteRepository with Entity Framework Core for database operations

### Strategy Pattern
- **Role**: Processing different deadline thresholds
- **Implementation**: INotificationStrategy classes (ExpiredNotificationStrategy, MoreThanWeekNotificationStrategy) and NotificationStrategyContext for strategy selection
- **Effect**: Elimination of hard coding, compliance with OCP

### Factory-like Behavior
- **Role**: Selection of notification strategy
- **Implementation**: NotificationStrategyContext selects strategy based on timeUntilDeadline
- **Effect**: Centralization of selection logic

### Dependency Inversion Principle (DIP)
- **Role**: Decoupling notification sending
- **Implementation**: INotificationSender interface with SignalRNotificationSender implementation
- **Effect**: Improved testing and flexibility

### Synchronization Pattern
- **Role**: Thread safety in BackgroundService
- **Implementation**: Using lock for Dictionary access
- **Effect**: Elimination of minute-by-minute notifications

### Deferred Execution
- **Role**: SignalR connection coordination
- **Implementation**: Delay using Task.Delay(TimeSpan.FromSeconds(5))
- **Effect**: Guaranteed notification delivery

## Key Components

### DeadlineNotificationService
- Background service for checking deadlines every minute
- Uses Dictionary<int, (DateTime Deadline, string LastNotificationType)> and lock for one-time notification sending
- 5-second delay ensures SignalR client connections
- Passes NoteTitle separately for flexible processing

### NotificationStrategyContext
- Selects notification strategy through INotificationStrategy

### INotificationSender
- Abstracts notification sending (SignalR)

### INoteRepository
- Provides access to notes through Repository Pattern

## Installation and Running

### Requirements
- .NET SDK 9.0
- SQL Server (configure in appsettings.json)
- Browser with WebSocket support (for SignalR)

### Instructions

1. **Clone the repository**:
```bash
git clone https://github.com/AcceleratorMX/TurboNotes
cd TurboNotes
```

2. **Configure the database**:
Update the connection string in appsettings.json:
```json
{
  "ConnectionStrings": {
    "TurboNotesDbConnection": "Server=localhost;Database=TurboNotesDB;MultipleActiveResultSets=true;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

3. **Run migrations**:
```bash
dotnet ef database update --startup-project ../TurboNotes.Web
```

4. **Run the application**:
```bash
dotnet run --project TurboNotes.Web
```

The application is available at http://localhost:5000.

5. **Testing notifications**:
Create a note with a deadline (e.g., 3 minutes from now).

## Refactoring and Improvements

The project underwent refactoring to eliminate code smells and improve architecture.

### Resolved Issues

#### Minute-by-minute notifications
- **Problem**: Intermediate implementation with ConcurrentDictionary sent notifications every minute
- **Solution**: Return to Dictionary with lock and LastNotificationType verification

#### Error for deadlines >7 days
- **Problem**: Missing strategy caused an error
- **Solution**: Added MoreThanWeekNotificationStrategy

#### Notification failure
- **Problem**: Notifications were sent before SignalR connection
- **Solution**: 5-second delay

#### Tight coupling to SignalR
- **Problem**: Direct use of IHubContext
- **Solution**: Introduced INotificationSender

### Eliminated Code Smells
- **Large Method**: Threshold logic moved to strategies
- **Hardcoding**: Replaced with Strategy Pattern
- **Tight Coupling**: Added INotificationSender
- **DI Lifecycle Issues**: Changed to AddSingleton

### Applied SOLID Principles
- **SRP**: Each component has a single responsibility
- **OCP**: New thresholds are added through strategies
- **DIP**: Dependency on abstractions

## Screenshots
![Main page](screenshots/Screenshot%202025-05-17%20171403.png)