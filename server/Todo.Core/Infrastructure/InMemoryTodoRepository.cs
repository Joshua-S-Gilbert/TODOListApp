using System.Collections.Concurrent;
using Todo.Core.Abstractions;
using Todo.Core.Domain;

namespace Todo.Core.Infrastructure;

public sealed class InMemoryTodoRepository : ITodoRepository
{
    private readonly ConcurrentDictionary<Guid, TodoItem> _store = new();

    public Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken ct = default) =>
        Task.FromResult((IReadOnlyList<TodoItem>)_store.Values.OrderByDescending(x => x.CreatedAt).ToList());

    public Task<TodoItem> AddAsync(string TaskTitle, CancellationToken ct = default)
    {
        var item = TodoItem.Create(TaskTitle);
        _store[item.Id] = item;
        return Task.FromResult(item);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default) =>
        Task.FromResult(_store.TryRemove(id, out _));   // TRY remove important, avoid not found exceptions. IMPORTANT!!!!

    public Task<TodoItem?> SetFinishedAsync(Guid id, bool finished, CancellationToken ct = default)
    {
        if (!_store.TryGetValue(id, out var old)) return Task.FromResult<TodoItem?>(null);
        var updated = old with { finished = finished };
        _store[id] = updated;
        return Task.FromResult<TodoItem?>(updated);
    }
    
}