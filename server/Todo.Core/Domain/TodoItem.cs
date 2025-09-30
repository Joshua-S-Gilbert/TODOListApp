namespace Todo.Core.Domain;

public sealed record TodoItem(Guid Id, string Title, DateTimeOffset CreatedAt, bool finished){
    public static TodoItem Create(string title) => new(
        Guid.NewGuid(),
        title.Trim(),
        DateTimeOffset.UtcNow,
        false);
}