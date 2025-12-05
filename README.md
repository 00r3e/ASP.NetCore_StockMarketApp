# 📈 ASP.NetCore_StockMarketApp

A full-stack C# web application that displays live stock prices using data from [Finnhub.io](https://finnhub.io/).  
The application demonstrates clean architecture, real-time updates, and test-driven development with Entity Framework Core, xUnit, and FluentValidation.

## Project Structure

ASP.NetCore_StockMarketApp/

├── StockMarketApp.Core/          # Domain models, DTOs, and interfaces

├── StockMarketApp.Infrastructure/ # EF Core DbContext + repository implementations

├── StockMarketApp.UI/             # ASP.NET Core MVC Web UI with live stock updates

├── StockMarketApp.ServiceTest/    # xUnit tests for services

├── StockMarketAppTests/           # Unit & integration tests for the application

├── StockMarketApp.sln             # Solution file

---

## 🛠 Tech Stack

- **.NET 8**
- **ASP.NET Core MVC**
- **Entity Framework Core**
- **SQL Server (LocalDB or full)**
- **FluentValidation**
- **xUnit** (unit + integration tests)
- **Clean Architecture** pattern
- **JavaScript / SignalR** for live updates

---

## 📦 Features

- Display live stock prices from Finnhub.io
- CRUD-like management for stock watchlists (if implemented)
- Domain-driven design with `Core`, `Infrastructure`, and `UI` layers
- Data validation with FluentValidation
- Full test suite:
  - Service tests
  - Controller tests
  - Integration tests
- Dependency Injection for repositories and services
- Use of DTOs for cleaner controller models
- Real-time updates using SignalR / WebSockets (if implemented)

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- SQL Server (LocalDB or full)
- Finnhub API key ([sign up here](https://finnhub.io/))

### Setup Steps

```bash
# Clone the repository
git clone https://github.com/00r3e/ASP.NetCore_StockMarketApp.git
cd ASP.NetCore_StockMarketApp

# Navigate to the UI project
cd StockMarketApp.UI

# Apply migrations and create the database
dotnet ef database update

# Run the application
dotnet run
