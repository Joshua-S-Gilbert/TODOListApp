using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Todo.Core.Abstractions;
using Todo.Core.Infrastructure;
using Todo.Core.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();
builder.services.AddScoped<ITodoService, TodoService>();

// CORS
var coresPolicy = "dev";
builder.Services.AddCors(options =>
{
    options.AddPolicy(coresPolicy, policy => {
        policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
    })
});

var app = builder.Build();

app.MapGet("/api/todos", async ([FromServices] ITodoService service, CancellationToken ct) =>{
    Results.Ok(await service.GetAllAsync(ct));
});

app.MapPost("/api/todos", async ([FromServices] ITodoService service, [FromBody] CreateTodo req, CancellationToken ct) => {
    try {
        var created = await service.AddAsync(req.Title, ct);
        return Results.Ok(created);
    }
    catch (ValidationException ex){
        return Results.ValidationProblem(new Dictionary<string, string[]>{
            ["Title"] = new[] {ex.Message}
        });
    }
});

app.MapDelete("/api/todos/{id:guid}", async ([FromServices] ITodoService service, Guid id, CancellationToken ct) => {
    var deleted = await service.DeleteAsync(id, ct);
    return deleted ? Results.NoContent() : Results.NotFound();
});




app.Run();

public record CreateTodo(string title);