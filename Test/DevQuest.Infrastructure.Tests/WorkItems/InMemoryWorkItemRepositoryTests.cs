using DevQuest.Domain.WorkItems;
using DevQuest.Infrastructure.WorkItems;

namespace DevQuest.Infrastructure.Tests.WorkItems;

public class InMemoryWorkItemRepositoryTests
{
    private readonly InMemoryWorkItemRepository _repository;

    public InMemoryWorkItemRepositoryTests()
    {
        _repository = new InMemoryWorkItemRepository();
    }

    [Fact]
    public async Task InsertAsync_Should_Add_WorkItem_To_Repository()
    {
        // Arrange
        var workItem = new WorkItem("Test WorkItem", "Test description");

        // Act
        await _repository.InsertAsync(workItem);

        // Assert
        var result = await _repository.GetByIdAsync(workItem.Id);
        result.Should().NotBeNull();
        result!.Should().Be(workItem);
    }

    [Fact]
    public async Task InsertAsync_Should_Throw_ArgumentNullException_When_WorkItem_Is_Null()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.InsertAsync(null!));
    }

    [Fact]
    public async Task InsertAsync_Should_Throw_InvalidOperationException_When_WorkItem_Already_Exists()
    {
        // Arrange
        var workItem = new WorkItem("Test WorkItem");
        await _repository.InsertAsync(workItem);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.InsertAsync(workItem));
        exception.Message.Should().Contain($"WorkItem with ID {workItem.Id} already exists.");
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_WorkItem_When_WorkItem_Exists()
    {
        // Arrange
        var workItem = new WorkItem("Test WorkItem", "Test description");
        await _repository.InsertAsync(workItem);

        // Act
        var result = await _repository.GetByIdAsync(workItem.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Should().Be(workItem);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_WorkItem_Does_Not_Exist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Empty_When_No_WorkItems_Exist()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_WorkItems()
    {
        // Arrange
        var workItem1 = new WorkItem("WorkItem 1");
        var workItem2 = new WorkItem("WorkItem 2");
        var workItem3 = new WorkItem("WorkItem 3");

        await _repository.InsertAsync(workItem1);
        await _repository.InsertAsync(workItem2);
        await _repository.InsertAsync(workItem3);

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(workItem1);
        result.Should().Contain(workItem2);
        result.Should().Contain(workItem3);
    }

    [Fact]
    public async Task GetByNameAsync_Should_Return_WorkItems_Containing_Name()
    {
        // Arrange
        var workItem1 = new WorkItem("Complete project task");
        var workItem2 = new WorkItem("Review code task");
        var workItem3 = new WorkItem("Meeting with client");

        await _repository.InsertAsync(workItem1);
        await _repository.InsertAsync(workItem2);
        await _repository.InsertAsync(workItem3);

        // Act
        var result = await _repository.GetByNameAsync("task");

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(workItem1);
        result.Should().Contain(workItem2);
        result.Should().NotContain(workItem3);
    }

    [Fact]
    public async Task GetByNameAsync_Should_Be_Case_Insensitive()
    {
        // Arrange
        var workItem = new WorkItem("Complete Project Task");
        await _repository.InsertAsync(workItem);

        // Act
        var result = await _repository.GetByNameAsync("project");

        // Assert
        result.Should().HaveCount(1);
        result.Should().Contain(workItem);
    }

    [Fact]
    public async Task GetByNameAsync_Should_Return_Empty_When_No_Match()
    {
        // Arrange
        var workItem = new WorkItem("Complete project");
        await _repository.InsertAsync(workItem);

        // Act
        var result = await _repository.GetByNameAsync("nonexistent");

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetScheduledForDateAsync_Should_Return_WorkItems_Scheduled_For_Date()
    {
        // Arrange
        var targetDate = DateTime.UtcNow.Date.AddDays(1);
        var workItem1 = new WorkItem("Task 1", null, targetDate);
        var workItem2 = new WorkItem("Task 2", null, targetDate.AddHours(5)); // Same date, different time
        var workItem3 = new WorkItem("Task 3", null, targetDate.AddDays(1)); // Different date
        var workItem4 = new WorkItem("Task 4"); // No scheduled date

        await _repository.InsertAsync(workItem1);
        await _repository.InsertAsync(workItem2);
        await _repository.InsertAsync(workItem3);
        await _repository.InsertAsync(workItem4);

        // Act
        var result = await _repository.GetScheduledForDateAsync(targetDate);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(workItem1);
        result.Should().Contain(workItem2);
        result.Should().NotContain(workItem3);
        result.Should().NotContain(workItem4);
    }

    [Fact]
    public async Task GetScheduledForDateAsync_Should_Return_Empty_When_No_WorkItems_Scheduled()
    {
        // Arrange
        var workItem = new WorkItem("Task without schedule");
        await _repository.InsertAsync(workItem);

        // Act
        var result = await _repository.GetScheduledForDateAsync(DateTime.UtcNow.Date);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Existing_WorkItem()
    {
        // Arrange
        var workItem = new WorkItem("Original name", "Original description");
        await _repository.InsertAsync(workItem);
        
        workItem.Update("Updated name", "Updated description");

        // Act
        await _repository.UpdateAsync(workItem);

        // Assert
        var result = await _repository.GetByIdAsync(workItem.Id);
        result.Should().NotBeNull();
        result!.Name.Should().Be("Updated name");
        result.Description.Should().Be("Updated description");
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_ArgumentNullException_When_WorkItem_Is_Null()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.UpdateAsync(null!));
    }

    [Fact]
    public async Task UpdateAsync_Should_Add_WorkItem_If_Not_Exists()
    {
        // Arrange
        var workItem = new WorkItem("New WorkItem");

        // Act
        await _repository.UpdateAsync(workItem);

        // Assert
        var result = await _repository.GetByIdAsync(workItem.Id);
        result.Should().NotBeNull();
        result!.Should().Be(workItem);
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_WorkItem_From_Repository()
    {
        // Arrange
        var workItem = new WorkItem("Test WorkItem");
        await _repository.InsertAsync(workItem);

        // Act
        await _repository.DeleteAsync(workItem.Id);

        // Assert
        var result = await _repository.GetByIdAsync(workItem.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Not_Throw_When_WorkItem_Does_Not_Exist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert (should not throw)
        await _repository.DeleteAsync(nonExistentId);
    }
}