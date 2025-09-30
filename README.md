# TODO list app

simple TODO list app with Angular frontend and .NET 9 backend. shows CRUD operations, angular framework, API

## versions
Frontend: Angular 20.3.3  
Backend: .NET 9 Minimal API

## requirements

Node.js 20+  
Angular CLI (npm install -g @angular/cli)  
.NET 9 SDK (LTS version)  

## How to run

1. run backend
``` bash
  cd server/Todo.Api
  dotnet restore
  dotnet run
```
2. run frontend
``` bash
  cd client
  npm install
  ng serve
```

double check your ports. the backend should run on http://localhost:5287  
if it doesn't, change the port under client/src/app/environments/environments.development.ts

by default angular is hosted on http://localhost:4200

## run unit tests
``` bash
cd server/Todo.Tests
dotnet restore
dotnet test
```

## API Endpoints
| Method | Endpoint           | Description              |
|--------|--------------------|--------------------------|
| GET    | /api/todos         | returns list of all tasks |
| POST   | /api/todos         | creates a task. Body: { "title": "string" } |
| DELETE | /api/todos/{id}    | Delete task by task ID   |
| PUT    | /api/todos/{id}/finished | set finished state. Body: { "finished": true } |


## notes/enhancements
Originally this app was made with only add task, delete task and a get for getting the tasks.  
I added a put for updating the finished state.   
Normally i use angular material and tailwindcss for styling, but this is a simple app  
adding further features would be fairly simple. an example is if i wanted to add a due date, there is a datepicker built into angular material. this would hit POST /api/todos, and if a task is updated another PUT /api/todos/{id}/dueDate is added to the backend.