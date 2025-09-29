using Todo.Core.Domain;

namespace Todo.Core.Abstractions;

public interface ITodoRepository{
    Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken ct = default);
    Task<TodoItem> AddAsync(string item, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}