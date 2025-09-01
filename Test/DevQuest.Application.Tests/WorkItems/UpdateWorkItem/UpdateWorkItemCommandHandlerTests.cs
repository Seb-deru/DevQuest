using DevQuest.Application.WorkItems;
using DevQuest.Application.WorkItems.UpdateWorkItem;
using DevQuest.Domain.WorkItems;

namespace DevQuest.Application.Tests.WorkItems.UpdateWorkItem;

public class UpdateWorkItemCommandHandlerTests
{
    private readonly Mock<IWorkItemRepository> _workItemRepositoryMock;
    private readonly UpdateWorkItemCommandHandler _handler;

    public UpdateWorkItemCommandHandlerTests()
    {
        _workItemRepositoryMock = new Mock<IWorkItemRepository>();
        _handler = new UpdateWorkItemCommandHandler(_workItemRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Update_WorkItem_Successfully_When_WorkItem_Exists()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var originalWorkItem = new WorkItem("Original Name", "Original Description", DateTime.UtcNow.AddDays(1));
        var newScheduledDate = DateTime.UtcNow.AddDays(3);
        var request = new UpdateWorkItemRequest(workItemId, "Updated Name", "Updated Description", newScheduledDate);

        _workItemRepositoryMock.Setup(x => x.GetByIdAsync(workItemId))
            .ReturnsAsync(originalWorkItem);

        // Act
        var result = await _handler.Handle(request);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(originalWorkItem.Id);
        result.Name.Should().Be("Updated Name");
        result.Description.Should().Be("Updated Description");
        result.ScheduledDate.Should().Be(newScheduledDate);
        result.CreatedAt.Should().Be(originalWorkItem.CreatedAt);
        result.ModifiedAt.Should().BeAfter(originalWorkItem.CreatedAt);

        _workItemRepositoryMock.Verify(x => x.GetByIdAsync(workItemId), Times.Once);
        _workItemRepositoryMock.Verify(x => x.UpdateAsync(originalWorkItem), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_WorkItem_Does_Not_Exist()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var request = new UpdateWorkItemRequest(workItemId, "Updated Name", "Updated Description");

        _workItemRepositoryMock.Setup(x => x.GetByIdAsync(workItemId))
            .ReturnsAsync((WorkItem?)null);

        // Act
        var result = await _handler.Handle(request);

        // Assert
        result.Should().BeNull();

        _workItemRepositoryMock.Verify(x => x.GetByIdAsync(workItemId), Times.Once);
        _workItemRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<WorkItem>()), Times.Never);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Handle_Should_Throw_ArgumentException_When_Name_Is_Invalid(string name)
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var request = new UpdateWorkItemRequest(workItemId, name, "Description");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request));
        exception.ParamName.Should().Be("Name");
        exception.Message.Should().Contain("WorkItem name cannot be empty");
    }

    [Fact]
    public async Task Handle_Should_Update_WorkItem_With_Null_Optional_Parameters()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var originalWorkItem = new WorkItem("Original Name", "Original Description", DateTime.UtcNow.AddDays(1));
        var request = new UpdateWorkItemRequest(workItemId, "Updated Name"); // No description or scheduled date

        _workItemRepositoryMock.Setup(x => x.GetByIdAsync(workItemId))
            .ReturnsAsync(originalWorkItem);

        // Act
        var result = await _handler.Handle(request);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Updated Name");
        result.Description.Should().BeNull();
        result.ScheduledDate.Should().BeNull();

        _workItemRepositoryMock.Verify(x => x.UpdateAsync(originalWorkItem), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Call_WorkItem_Update_Method_With_Correct_Parameters()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var originalWorkItem = new WorkItem("Original Name");
        var newScheduledDate = DateTime.UtcNow.AddDays(2);
        var request = new UpdateWorkItemRequest(workItemId, "New Name", "New Description", newScheduledDate);

        _workItemRepositoryMock.Setup(x => x.GetByIdAsync(workItemId))
            .ReturnsAsync(originalWorkItem);

        // Act
        await _handler.Handle(request);

        // Assert
        // Verify that the work item properties were updated
        originalWorkItem.Name.Should().Be("New Name");
        originalWorkItem.Description.Should().Be("New Description");
        originalWorkItem.ScheduledDate.Should().Be(newScheduledDate);
    }

    [Fact]
    public async Task Handle_Should_Update_ModifiedAt_Property()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var originalWorkItem = new WorkItem("Original Name");
        var originalModifiedAt = originalWorkItem.ModifiedAt;
        var request = new UpdateWorkItemRequest(workItemId, "Updated Name");

        _workItemRepositoryMock.Setup(x => x.GetByIdAsync(workItemId))
            .ReturnsAsync(originalWorkItem);

        // Act
        await Task.Delay(10); // Ensure time difference
        var result = await _handler.Handle(request);

        // Assert
        result.Should().NotBeNull();
        result!.ModifiedAt.Should().BeAfter(originalModifiedAt);
    }

    [Fact]
    public async Task Handle_Should_Preserve_CreatedAt_Property()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var originalWorkItem = new WorkItem("Original Name");
        var originalCreatedAt = originalWorkItem.CreatedAt;
        var request = new UpdateWorkItemRequest(workItemId, "Updated Name");

        _workItemRepositoryMock.Setup(x => x.GetByIdAsync(workItemId))
            .ReturnsAsync(originalWorkItem);

        // Act
        var result = await _handler.Handle(request);

        // Assert
        result.Should().NotBeNull();
        result!.CreatedAt.Should().Be(originalCreatedAt);
    }

    [Fact]
    public async Task Handle_Should_Preserve_WorkItem_Id()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var originalWorkItem = new WorkItem("Original Name");
        var originalId = originalWorkItem.Id;
        var request = new UpdateWorkItemRequest(workItemId, "Updated Name");

        _workItemRepositoryMock.Setup(x => x.GetByIdAsync(workItemId))
            .ReturnsAsync(originalWorkItem);

        // Act
        var result = await _handler.Handle(request);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(originalId);
    }
}