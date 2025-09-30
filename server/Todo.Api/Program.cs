using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Todo.Core.Abstractions;
using Todo.Core.Infrastructure;
using Todo.Core.Services;
using System;   // debugging purposes

var builder = WebApplication.CreateBuilder(args);



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

app.UseCors(corsPolicy);

app.MapGet("/api/todos", async ([FromServices] ITodoService service, CancellationToken ct) =>{
    return Results.Ok(await service.GetAllAsync(ct));
});

app.MapPost("/api/todos", async ([FromServices] ITodoService service, [FromBody] CreateTodo req, CancellationToken ct) =>
{
    // Console.WriteLine("start post");    // debugging purposes, left for posterity
    try
    {
        var created = await service.AddAsync(req.Title, ct);
        return Results.Ok(created);
    }
    catch (ValidationException ex)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["Title"] = new[] { ex.Message }
        });
    };
});

app.MapDelete("/api/todos/{id:guid}", async ([FromServices] ITodoService service, Guid id, CancellationToken ct) => {
    // Console.WriteLine("start delete");   // debugging purposes, left for posterity
    var deleted = await service.DeleteAsync(id, ct);
    return deleted ? Results.NoContent() : Results.NotFound();  // 204 vs 404
});

app.MapPut("/api/todos/{id:guid}/finished",
    async (ITodoService service, Guid id, SetFinished body, CancellationToken ct) =>
{
    var updated = await service.SetFinishedAsync(id, body.Finished, ct);
    return updated is null ? Results.NotFound() : Results.Ok(updated);
});

app.Run();

public record CreateTodo(string Title);
public record SetFinished(bool Finished);