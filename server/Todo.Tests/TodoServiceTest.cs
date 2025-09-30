using System.ComponentModel.DataAnnotations;
using Todo.Core.Abstractions;
using Todo.Core.Services;
using Todo.Core.Infrastructure;
using Xunit;

namespace Todo.Tests;

public class TodoServiceTests
{
    // valid task
    [Fact]
    public async Task AddValidCreateItem()
    {
        var repo = new InMemoryTodoRepository();
        var service = new TodoService(repo);
        var item = await service.AddAsync("Test Task");
        Assert.NotEqual(Guid.Empty, item.Id);
        Assert.Equal("Test Task", item.Title);
        Assert.False(item.finished);
    }

    [Fact]
    public async Task AddValidTitle()
    {
        var repo = new InMemoryTodoRepository();
        var service = new TodoService(repo);
        var item = await service.AddAsync("     isTrimmed      ");
        Assert.Equal("isTrimmed", item.Title);
    }

    // test for empty title
    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    public async Task AddBlankTitleThrows(string title)
    {
        var service = new TodoService(new InMemoryTodoRepository());
        await Assert.ThrowsAsync<ValidationException>(() => service.AddAsync(title));
    }

    [Fact]
    public async Task AddTooLongTitleThrows()
    {
        var service = new TodoService(new InMemoryTodoRepository());
        string longTitle = new string('*', 201);
        await Assert.ThrowsAsync<ValidationException>(() => service.AddAsync(longTitle));
    }

    [Fact]
    public async Task SetFinished()
    {
        var repo = new InMemoryTodoRepository();
        var service = new TodoService(repo);
        var item = await service.AddAsync("Task");
        var updated = await service.SetFinishedAsync(item.Id, true);
        Assert.True(updated!.finished);
        Assert.Equal(item.Id, updated.Id);
    }

    
}
