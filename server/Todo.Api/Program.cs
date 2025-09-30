using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Todo.Core.Abstractions;
using Todo.Core.Infrastructure;
using Todo.Core.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer(); // needed for swagger, exposes endpoint metadata
builder.Services.AddSwaggerGen();   // API documentation

builder.Services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();   // create once, used everywhere until app shutdown. related to stateless service. careful of race conditions on shared memory
builder.Services.AddScoped<ITodoService, TodoService>();    // 1 instance per request. after request ends, dispose instance

// CORS
var corsPolicy = "dev";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy => {
        policy.AllowAnyOrigin()     // any website can access this API
                .AllowAnyHeader()   // custom headers allowed
                .AllowAnyMethod();  // GET, POST, DELETE, etc
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(corsPolicy);

app.MapGet("/api/todos", async ([FromServices] ITodoService service, CancellationToken ct) =>{
    return Results.Ok(await service.GetAllAsync(ct));
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
    return deleted ? Results.NoContent() : Results.NotFound();  // 204 vs 404
});


app.Run();

public record CreateTodo(string Title);