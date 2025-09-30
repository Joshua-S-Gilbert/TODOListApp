using System.ComponentModel.DataAnnotations;
using Todo.Core.Abstractions;
using Todo.Core.Services;
using Xunit;

namespace Todo.Tests;

public class TodoServiceTests
{
    // valid task
    [Fact]
    public async Task AddValidCreateItem()
    {
        var repo = new InMemoryTodoRepository();
        var svc = new TodoService(repo);
        var item = await svc.AddAsync("Test Task");
        Assert.NotEqual(Guid.Empty, item.Id);
        Assert.Equal("Test Task", item.Title);
        Assert.False(item.IsCompleted);
    }

    // test for empty title
    [Theory]    
    [InlineData("")]
    [InlineData("     ")]
    public async Task AddBlankTitleThrows(string title){
        var svc = new TodoService(new InMemoryTodoRepository());
        await Assert.ThrowsAsync<ValidationException>(async () => await svc.AddAsync(title));
    }

    // test for too long title is basically same as above. Time constraints means I won't write it out.
}
