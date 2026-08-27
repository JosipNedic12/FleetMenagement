# Fleet Management System 🚛

A full-stack **Fleet Management System** designed to help manage vehicles, fleet-related information, and operational data through a modern web application.

The project consists of a **.NET backend** following a layered architecture and an **Angular frontend** that provides the user interface.

## 🛠️ Technologies

### Backend

* **C#**
* **.NET**
* **ASP.NET Core Web API**
* **Entity Framework Core**
* REST API
* Dependency Injection
* Middleware
* Localization

### Frontend

* **Angular**
* **TypeScript**
* HTML
* CSS
* Angular CLI

### Development Tools

* Visual Studio
* Visual Studio Code
* Git
* Docker

## 🏗️ Architecture

The backend follows a layered architecture separating business logic, domain models, infrastructure, and API concerns.

```text
                         ┌─────────────────────┐
                         │   Angular Frontend  │
                         │   TypeScript        │
                         └──────────┬──────────┘
                                    │
                                  HTTP
                                    │
                                    ▼
                         ┌─────────────────────┐
                         │    ASP.NET Core     │
                         │        API          │
                         └──────────┬──────────┘
                                    │
                    ┌───────────────┼───────────────┐
                    ▼               ▼               ▼
             ┌────────────┐ ┌────────────┐ ┌──────────────┐
             │ Application│ │   Domain   │ │Infrastructure│
             │   Layer    │ │   Layer    │ │    Layer     │
             └────────────┘ └────────────┘ └──────────────┘
```

## 📁 Project Structure

```text
FleetMenagement/
│
├── FleetManagement.API/
│   ├── Controllers/
│   ├── Localization/
│   ├── Middleware/
│   ├── Properties/
│   ├── Dockerfile
│   ├── Program.cs
│   └── FleetManagement.API.csproj
│
├── FleetManagement.Application/
│
├── FleetManagement.Domain/
│
├── FleetManagement.Infrastructure/
│
├── fleet-frontend/
│   ├── public/
│   ├── src/
│   ├── angular.json
│   ├── package.json
│   ├── Dockerfile
│   └── nginx.conf
│
├── elk/
│
├── FleetManagement.sln
└── .gitignore
```

The repository currently contains separate projects for the API, application logic, domain model, infrastructure, and Angular frontend.

## 🚀 Getting Started

### Prerequisites

Make sure the following are installed:

* .NET SDK
* Node.js
* npm
* Angular CLI
* Visual Studio or Visual Studio Code

## ⚙️ Backend Setup

Clone the repository:

```bash
git clone https://github.com/JosipNedic12/FleetMenagement.git
```

Navigate into the project:

```bash
cd FleetMenagement
```

Restore the .NET dependencies:

```bash
dotnet restore
```

Build the solution:

```bash
dotnet build
```

The solution contains the following .NET projects:

* `FleetManagement.API`
* `FleetManagement.Application`
* `FleetManagement.Domain`
* `FleetManagement.Infrastructure`

These projects are included in the `FleetManagement.sln` solution.

### Run the API

```bash
dotnet run --project FleetManagement.API
```

The API can also be started directly from Visual Studio by opening:

```text
FleetManagement.sln
```

## 💻 Frontend Setup

Navigate to the Angular application:

```bash
cd fleet-frontend
```

Install dependencies:

```bash
npm install
```

Start the development server:

```bash
ng serve
```

The Angular application is available at:

```text
http://localhost:4200/
```

The frontend was generated using Angular CLI and the repository currently uses Angular CLI **21.1.1**.

## 🏭 Production Build

To create a production build of the Angular application:

```bash
ng build
```

The generated files are placed in:

```text
dist/
```

Angular's production build performs optimization for performance and deployment.

## 🧪 Testing

Run the Angular unit tests with:

```bash
ng test
```

End-to-end tests can be run with:

```bash
ng e2e
```

The frontend project uses **Vitest** for its unit-test runner.

For the .NET solution, tests can be executed with:

```bash
dotnet test
```

## 🐳 Docker

The project includes Docker configuration for the API and Angular frontend.

Backend:

```text
FleetManagement.API/Dockerfile
```

Frontend:

```text
fleet-frontend/Dockerfile
```

The frontend also contains an `nginx.conf` configuration for serving the production Angular application.

## 🔐 Configuration

Application-specific configuration should be stored in environment-specific configuration files rather than committed secrets.

Before running the application, make sure the required:

* Database connection
* API configuration
* Authentication settings
* Environment variables

are configured appropriately for your environment.

**Do not commit passwords, API keys, connection strings containing credentials, or other secrets to the repository.**

## 🌐 Application Overview

The system is designed around a web-based fleet management workflow.

The backend provides REST API endpoints through ASP.NET Core, while the Angular frontend consumes those endpoints and provides the user interface.

The architecture separates responsibilities into:

### API Layer

Responsible for:

* HTTP requests
* Controllers
* API endpoints
* Middleware
* Application startup
* Localization

### Application Layer

Responsible for:

* Application logic
* Use cases
* Services
* Validation
* Coordination between the API and domain layers

### Domain Layer

Contains:

* Domain entities
* Business rules
* Core models
* Domain-specific logic

### Infrastructure Layer

Responsible for:

* Database access
* External services
* Persistence
* Infrastructure-specific implementations

## 📊 Logging and Monitoring

The repository also contains an `elk` directory, intended for the **ELK stack (Elasticsearch, Logstash, and Kibana)** and application logging/monitoring infrastructure.

## 🔄 Development Workflow

A typical development workflow is:

```text
       Developer
           │
           ▼
    Angular Frontend
           │
           │ HTTP / REST
           ▼
    ASP.NET Core API
           │
           ▼
    Application Layer
           │
           ▼
      Domain Layer
           │
           ▼
 Infrastructure Layer
           │
           ▼
       Database
```

## 📌 Future Improvements

Potential improvements include:

* Advanced fleet analytics
* More detailed vehicle tracking
* Maintenance scheduling
* Improved reporting
* Advanced dashboard visualizations
* Role-based permissions
* Additional localization
* Automated deployment
* Expanded automated testing
* Improved monitoring and logging

## 👨‍💻 Author

**Josip Nedić**

Computer Science / Software Engineering

## 📄 License

This project is currently intended as a personal/educational software project.

See the repository for the current licensing and usage terms.

## 🔗 Repository

[FleetMenagement on GitHub](https://github.com/JosipNedic12/FleetMenagement?utm_source=chatgpt.com)
