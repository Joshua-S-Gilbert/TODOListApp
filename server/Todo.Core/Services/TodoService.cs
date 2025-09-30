using System.ComponentModel.DataAnnotations;
using Todo.Core.Abstractions;
using Todo.Core.Domain;

namespace Todo.Core.Services;

 public sealed class TodoService(ITodoRepository repository) : ITodoService{
    public Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken ct = default) =>
        repository.GetAllAsync(ct);

    public Task<TodoItem> AddAsync(string title, CancellationToken ct = default){
        if(string.IsNullOrWhiteSpace(title))
            throw new ValidationException("Title empty");
        if (title.Length > 200)    // arbitrary
            throw new ValidationException("Title too long");
        return repository.AddAsync(title, ct);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default) =>
        repository.DeleteAsync(id, ct);
 }