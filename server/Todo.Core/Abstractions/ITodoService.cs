using Todo.Core.Domain;

namespace Todo.Core.Abstractions;

public interface ITodoService
{
    Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken ct = default);
    Task<TodoItem> AddAsync(string item, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    Task<TodoItem?> SetFinishedAsync(Guid id, bool finished, CancellationToken ct = default);
}