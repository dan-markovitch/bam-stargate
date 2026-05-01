# Stargate — Astronaut Career Tracking System

A full-stack application for tracking astronaut careers, duty assignments, and career history.

## Tech Stack

### Backend
- .NET 8 / C#
- ASP.NET Core Web API
- MediatR (CQRS pattern)
- Entity Framework Core + Dapper
- SQLite database
- xUnit + Moq (unit testing)

### Frontend
- Angular 21
- TypeScript
- HttpClient for API communication

---

## Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js 18+
- Angular CLI (`npm install -g @angular/cli`)

### Run the Backend
```bash
cd api
dotnet ef database update
dotnet run
```
API will be available at `http://localhost:5204`
Swagger UI at `http://localhost:5204/swagger`

### Run the Frontend
```bash
cd stargate-ui
npm install
ng serve
```
UI will be available at `http://localhost:4200`

---

## What I Did

### Bug Fixes
1. **AstronautDutyController** — `GetAstronautDutiesByName` was calling the wrong MediatR query (`GetPersonByName` instead of `GetAstronautDutiesByName`)
2. **Duplicate duty check** — The pre-processor was checking for duplicate duties globally across all people instead of scoping the check to the specific person
3. **Missing try/catch** — `CreateAstronautDuty` POST endpoint had no exception handling unlike every other controller action
4. **RETIRED CareerEndDate** — When creating a brand new `AstronautDetail` for a RETIRED astronaut, `CareerEndDate` was being set to `DutyStartDate` instead of `DutyStartDate.AddDays(-1)`

### Defensive Coding
- Fixed all null reference warnings across data models (`AstronautDetail`, `AstronautDuty`)
- Added null checks in query handlers before dereferencing results
- Made navigation properties nullable where appropriate

### Process Logging
- Added `AstronautLog` table to the database via EF Core migration
- All controller actions log both successes and exceptions to the database with timestamps
- Log entries include the operation name, person name, and result message

### Unit Tests
- Created `StargateAPI.Tests` xUnit test project
- 7 tests covering `CreatePerson` and `CreateAstronautDuty` business logic
- Tests use an in-memory database to avoid external dependencies
- Coverage includes: success paths, duplicate detection, retirement logic, and duty end date handling

### Angular Frontend
- Search for astronauts by name
- Display person card with current rank, duty title, career start and end dates
- Display full duty history in a sortable table
- Error handling for person not found
- Responsive design

### CORS
- Enabled CORS in the API to allow the Angular frontend to communicate with the backend

---

## Design Decisions

- **Kept MediatR pattern** — The starter code uses MediatR for CQRS. I kept this pattern and worked within it rather than replacing it, as it's clearly an intentional architectural choice.
- **Logging at controller level** — I chose to log at the controller level rather than inside handlers so that all entry/exit points are captured regardless of which handler is invoked.
- **In-memory database for tests** — Rather than mocking the entire DbContext, I used EF Core's InMemory provider to keep tests readable and close to real behavior.
- **Angular standalone components** — Used Angular's modern standalone component architecture rather than NgModules.