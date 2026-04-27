# Task-Management-API

Simple REST API for managing tasks. Built with ASP.NET Core, Entity Framework Core, SQLite, and JWT authentication.

## Features

- User register/login
- JWT auth
- Task CRUD
- Layered architecture
- Swagger for testing (disabled by default)

## Tech Stack

- ASP.NET Core 8
- Entity Framework Core
- SQLite
- JWT

## Project Structure

- `Controllers` handles HTTP requests
- `Services` holds business logic
- `Repositories` talks to the database
- `Models` stores entity classes
- `DTOs` stores request/response objects
- `Data` contains the EF Core context

## How to Run

Make sure .NET 8 SDK is installed, then run:

```bash
dotnet restore
dotnet run
```

Swagger should open at something like:

- `https://localhost:7168/swagger`
- `http://localhost:5168/swagger`

## Database

project uses SQLite with `Database.EnsureCreated()`, so the DB file automatically gets created when app starts.

DB file:

- `app.db`

## Main Endpoints

- `POST /api/auth/register`
- `POST /api/auth/login`
- `GET /api/tasks`
- `GET /api/tasks/{id}`
- `POST /api/tasks`
- `PUT /api/tasks/{id}`
- `DELETE /api/tasks/{id}`

## Notes

- Passwords hashed before being stored
- Users can only access their own tasks
- Task statuses are `Todo`, `In Progress`, and `Done`

```text
/api
```

### Auth Endpoints

#### Register

```http
POST /api/auth/register
```

Request body:

```json
{
  "name": "Jared",
  "email": "jared@example.com",
  "password": "password123"
}
```

Example success response:

```json
{
  "message": "User registered successfully.",
  "data": {
    "token": "your-jwt-token",
    "name": "Jared",
    "email": "jared@example.com"
  }
}
```

#### Login

```http
POST /api/auth/login
```

Request body:

```json
{
  "email": "jared@example.com",
  "password": "password123"
}
```

Example success response:

```json
{
  "message": "Login successful.",
  "data": {
    "token": "your-jwt-token",
    "name": "Jared",
    "email": "jared@example.com"
  }
}
```

### Task Endpoints

All task endpoints require authentication.

#### Get All Tasks

```http
GET /api/tasks
```

#### Get One Task

```http
GET /api/tasks/{id}
```

#### Create Task

```http
POST /api/tasks
```

Request body:

```json
{
  "title": "Finish homework",
  "description": "Do the API part first",
  "status": "Todo",
  "dueDate": "2026-05-01T23:59:00"
}
```

#### Update Task

```http
PUT /api/tasks/{id}
```

Request body:

```json
{
  "title": "Finish homework",
  "description": "API part is done, now do the report",
  "status": "In Progress",
  "dueDate": "2026-05-02T23:59:00"
}
```

#### Delete Task

```http
DELETE /api/tasks/{id}
```

## Example Testing Flow in Swagger

If you want to test everything in order:

1. Run project
2. Open Swagger
3. Call `POST /api/auth/register`
4. Copy the JWT token from the response
5. Click the `Authorize` button in Swagger
6. Paste `Bearer your-token`
7. Test all `/api/tasks` endpoints

Pretty standard workflow, plus Swagger makes it way less annoying XD

## Example cURL Commands

### Register

```bash
curl -X POST https://localhost:7168/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "name":"Jared",
    "email":"jared@example.com",
    "password":"password123"
  }'
```

### Login

```bash
curl -X POST https://localhost:7168/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email":"jared@example.com",
    "password":"password123"
  }'
```

### Create a Task

```bash
curl -X POST https://localhost:7168/api/tasks \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer your-token-here" \
  -d '{
    "title":"Study for exam",
    "description":"Need to review EF Core and JWT",
    "status":"Todo"
  }'
```

## Validation That Exists Right Now
- required fields
- email format
- password min length
- string length limits

Validation mainly comes from data annotations on DTOs and models...

## Security Notes

- passwords are hashed, not stored in plain text
- task routes are protected with `[Authorize]`
- users only access their own tasks

## What I want to do in the future (prob not)
- add task priority
- add task categories
- add search/filter endpoints
- add refresh tokens
- add unit tests (too boring)
- add EF Core migrations (too hard)
- deploy to Azure or Render (too tedious)
