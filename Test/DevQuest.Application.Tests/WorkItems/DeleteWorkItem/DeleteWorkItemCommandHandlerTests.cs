using DevQuest.Application.WorkItems;
using DevQuest.Application.WorkItems.DeleteWorkItem;
using DevQuest.Domain.WorkItems;

namespace DevQuest.Application.Tests.WorkItems.DeleteWorkItem;

public class DeleteWorkItemCommandHandlerTests
{
    private readonly Mock<IWorkItemRepository> _workItemRepositoryMock;
    private readonly DeleteWorkItemCommandHandler _handler;

    public DeleteWorkItemCommandHandlerTests()
    {
        _workItemRepositoryMock = new Mock<IWorkItemRepository>();
        _handler = new DeleteWorkItemCommandHandler(_workItemRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Delete_WorkItem_Successfully_When_WorkItem_Exists()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var existingWorkItem = new WorkItem("Test WorkItem", "Test Description");

        _workItemRepositoryMock.Setup(x => x.GetByIdAsync(workItemId))
            .ReturnsAsync(existingWorkItem);

        // Act
        var result = await _handler.Handle(workItemId);

        // Assert
        result.Should().BeTrue();

        _workItemRepositoryMock.Verify(x => x.GetByIdAsync(workItemId), Times.Once);
        _workItemRepositoryMock.Verify(x => x.DeleteAsync(workItemId), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_False_When_WorkItem_Does_Not_Exist()
    {
        // Arrange
        var workItemId = Guid.NewGuid();

        _workItemRepositoryMock.Setup(x => x.GetByIdAsync(workItemId))
            .ReturnsAsync((WorkItem?)null);

        // Act
        var result = await _handler.Handle(workItemId);

        // Assert
        result.Should().BeFalse();

        _workItemRepositoryMock.Verify(x => x.GetByIdAsync(workItemId), Times.Once);
        _workItemRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Call_Repository_Methods_In_Correct_Order()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var existingWorkItem = new WorkItem("Test WorkItem");
        var methodCalls = new List<string>();

        _workItemRepositoryMock.Setup(x => x.GetByIdAsync(workItemId))
            .ReturnsAsync(existingWorkItem)
            .Callback(() => methodCalls.Add("GetByIdAsync"));

        _workItemRepositoryMock.Setup(x => x.DeleteAsync(workItemId))
            .Returns(Task.CompletedTask)
            .Callback(() => methodCalls.Add("DeleteAsync"));

        // Act
        await _handler.Handle(workItemId);

        // Assert
        methodCalls.Should().HaveCount(2);
        methodCalls[0].Should().Be("GetByIdAsync");
        methodCalls[1].Should().Be("DeleteAsync");
    }

    [Fact]
    public async Task Handle_Should_Pass_Correct_Id_To_Repository_Methods()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var existingWorkItem = new WorkItem("Test WorkItem");

        _workItemRepositoryMock.Setup(x => x.GetByIdAsync(workItemId))
            .ReturnsAsync(existingWorkItem);

        // Act
        await _handler.Handle(workItemId);

        // Assert
        _workItemRepositoryMock.Verify(x => x.GetByIdAsync(workItemId), Times.Once);
        _workItemRepositoryMock.Verify(x => x.DeleteAsync(workItemId), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Not_Call_DeleteAsync_When_WorkItem_Does_Not_Exist()
    {
        // Arrange
        var workItemId = Guid.NewGuid();

        _workItemRepositoryMock.Setup(x => x.GetByIdAsync(workItemId))
            .ReturnsAsync((WorkItem?)null);

        // Act
        await _handler.Handle(workItemId);

        // Assert
        _workItemRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Return_True_For_Valid_WorkItem_Deletion()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var workItem = new WorkItem("WorkItem to Delete", "Description", DateTime.UtcNow.AddDays(1));

        _workItemRepositoryMock.Setup(x => x.GetByIdAsync(workItemId))
            .ReturnsAsync(workItem);

        // Act
        var result = await _handler.Handle(workItemId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Handle_Empty_Guid()
    {
        // Arrange
        var emptyGuid = Guid.Empty;

        _workItemRepositoryMock.Setup(x => x.GetByIdAsync(emptyGuid))
            .ReturnsAsync((WorkItem?)null);

        // Act
        var result = await _handler.Handle(emptyGuid);

        // Assert
        result.Should().BeFalse();
        _workItemRepositoryMock.Verify(x => x.GetByIdAsync(emptyGuid), Times.Once);
    }
}