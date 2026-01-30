📈 ASP.NetCore_StockMarketApp

A full-stack ASP.NET Core MVC application that displays live stock prices using data from Finnhub.io.
The project demonstrates Clean Architecture, real-time updates, and test-driven development using Entity Framework Core, xUnit, and FluentValidation.


Deployed on Azure App Service

📁 Project Structure
ASP.NetCore_StockMarketApp/
│
├── StockMarketApp.Core/           # Domain models, DTOs, and interfaces
├── StockMarketApp.Infrastructure/ # EF Core DbContext + repository implementations
├── StockMarketApp.UI/             # ASP.NET Core MVC Web UI with live stock updates
├── StockMarketApp.ServiceTest/    # xUnit tests for services
├── StockMarketAppTests/           # Unit & integration tests
├── StockMarketApp.sln             # Solution file

🛠 Tech Stack

.NET 8

ASP.NET Core MVC

Entity Framework Core

SQL Server (LocalDB or full)

FluentValidation

xUnit (Unit & Integration Tests)

Clean Architecture

JavaScript / SignalR (real-time updates)

Azure App Service (Deployment)

📦 Features

📊 Display live stock prices from Finnhub.io

🧱 Clean Architecture (Core / Infrastructure / UI)

🔄 Real-time updates via SignalR / WebSockets (if implemented)

✅ Data validation with FluentValidation

🧪 Full test suite:

Service tests

Controller tests

Integration tests

🔌 Dependency Injection for repositories and services

🔄 Use of DTOs for clean controller models

🚀 Getting Started
Prerequisites

.NET 8 SDK

SQL Server (LocalDB or full)

Finnhub API key (sign up at https://finnhub.io
)

Setup Steps
# Clone the repository
git clone https://github.com/00r3e/ASP.NetCore_StockMarketApp.git
cd ASP.NetCore_StockMarketApp

# Navigate to the UI project
cd StockMarketApp.UI

# Apply migrations and create the database
dotnet ef database update

# Run the application
dotnet run
