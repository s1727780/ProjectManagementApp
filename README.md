# Project Management App

A simple project and task tracking app built with Python. <br> 
Designed as a project to develop skills in backend.

## Features (MVP)
- Create, read, update, and delete **projects**
- Each project contains multiple **tasks**
- Each task has:
	- Title
	- Description
	- Due Date
	- Status
- Filter tasks by status

---

## Tech Stack

| Layer       | Tech                     |
|------------|---------------------------|
| Language    | Python |
| Backend     | FastAPI    |
| Database    | SQLite |
| Frontend    | **TBC** Blazor / HTML + JS / WPF (optional) |
| Deployment  | Localhost / Docker (optional) |

---

## Data Model

### Project

| Parameter		| Type                     |
|---------------|---------------------------|
| id (primary)	| int |
| name			| string |
| description   | string |
| created_at    | datetime |

### Task

| Parameter		| Type                     |
|---------------|---------------------------|
| id (primary)	| int |
| project_id	| Foreign Key to Project id |
| name			| string |
| description   | string |
| due_date		| datetime |
| status		| enum (Todo / In Progress / Done |

---

## API Endpoints

| Method | Endpoint             | Description             |
|--------|----------------------|-------------------------|
| GET    | `/projects`          | List all projects       |
| POST   | `/projects`          | Create new project      |
| GET    | `/projects/{id}`     | Get specific project    |
| PUT    | `/projects/{id}`     | Update project          |
| DELETE | `/projects/{id}`     | Delete project          |
| GET    | `/projects/{id}/tasks` | List all tasks in project |
| POST   | `/projects/{id}/tasks` | Add new task to project |
| PUT    | `/tasks/{id}`        | Update task             |
| DELETE | `/tasks/{id}`        | Delete task             |

---

## Project Structure

project-management-app/ <br>
├── Controllers/ <br>
├── Models/ <br>
├── Services/ <br>
├── Data/ <br>
├── DTOs/ <br>
├── Program.cs / main.py <br>
└── tests/ <br>

---

## Milestones

- [ ] Define MVP features
- [ ] Set up project and initialize Git
- [ ] Set up database and migrations
- [ ] Implement Project CRUD
- [ ] Implement Task CRUD
- [ ] Add basic unit tests
- [ ] (Optional) Build frontend
- [ ] (Optional) Dockerize and deploy

---

## Testing

- Unit tests for task logic and validation
- Integration tests for API endpoints
- (Planned): CI with GitHub Actions

---

## Running the Project

### Prerequisites

- Python 3.13
- SQLite

### Instructions

```bash
# Clone the repo
git clone https://github.com/yourusername/project-management-app.git
cd project-management-app

# Setup and run
uvicorn main:app   # for FastAPI













