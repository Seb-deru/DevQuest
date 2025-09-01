using DevQuest.Application.WorkItems;
using DevQuest.Application.WorkItems.GetWorkItemById;
using DevQuest.Domain.WorkItems;

namespace DevQuest.Application.Tests.WorkItems.GetWorkItemById;

public class GetWorkItemByIdQueryHandlerTests
{
    private readonly Mock<IWorkItemRepository> _workItemRepositoryMock;
    private readonly GetWorkItemByIdQueryHandler _handler;

    public GetWorkItemByIdQueryHandlerTests()
    {
        _workItemRepositoryMock = new Mock<IWorkItemRepository>();
        _handler = new GetWorkItemByIdQueryHandler(_workItemRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_WorkItem_When_WorkItem_Exists()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var scheduledDate = DateTime.UtcNow.AddDays(1);
        var workItem = new WorkItem("Test WorkItem", "Test description", scheduledDate);

        _workItemRepositoryMock.Setup(x => x.GetByIdAsync(workItemId))
            .ReturnsAsync(workItem);

        // Act
        var result = await _handler.Handle(workItemId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(workItem.Id);
        result.Name.Should().Be(workItem.Name);
        result.Description.Should().Be(workItem.Description);
        result.ScheduledDate.Should().Be(workItem.ScheduledDate);
        result.CreatedAt.Should().Be(workItem.CreatedAt);
        result.ModifiedAt.Should().Be(workItem.ModifiedAt);

        _workItemRepositoryMock.Verify(x => x.GetByIdAsync(workItemId), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_WorkItem_Does_Not_Exist()
    {
        // Arrange
        var workItemId = Guid.NewGuid();

        _workItemRepositoryMock.Setup(x => x.GetByIdAsync(workItemId))
            .ReturnsAsync((WorkItem?)null);

        // Act
        var result = await _handler.Handle(workItemId);

        // Assert
        result.Should().BeNull();

        _workItemRepositoryMock.Verify(x => x.GetByIdAsync(workItemId), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_WorkItem_With_Null_Optional_Properties()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var workItem = new WorkItem("Test WorkItem"); // No description or scheduled date

        _workItemRepositoryMock.Setup(x => x.GetByIdAsync(workItemId))
            .ReturnsAsync(workItem);

        // Act
        var result = await _handler.Handle(workItemId);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Test WorkItem");
        result.Description.Should().BeNull();
        result.ScheduledDate.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_Call_Repository_With_Correct_Id()
    {
        // Arrange
        var workItemId = Guid.NewGuid();

        _workItemRepositoryMock.Setup(x => x.GetByIdAsync(workItemId))
            .ReturnsAsync((WorkItem?)null);

        // Act
        await _handler.Handle(workItemId);

        // Assert
        _workItemRepositoryMock.Verify(x => x.GetByIdAsync(workItemId), Times.Once);
    }
}