using DevQuest.Application.WorkItems;
using DevQuest.Application.WorkItems.CreateWorkItem;
using DevQuest.Domain.WorkItems;

namespace DevQuest.Application.Tests.WorkItems.CreateWorkItem;

public class CreateWorkItemCommandHandlerTests
{
    private readonly Mock<IWorkItemRepository> _workItemRepositoryMock;
    private readonly CreateWorkItemCommandHandler _handler;

    public CreateWorkItemCommandHandlerTests()
    {
        _workItemRepositoryMock = new Mock<IWorkItemRepository>();
        _handler = new CreateWorkItemCommandHandler(_workItemRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_WorkItem_Successfully_When_Request_Is_Valid()
    {
        // Arrange
        var request = new CreateWorkItemRequest("Test WorkItem", "Test description", DateTime.UtcNow.AddDays(1));

        // Act
        var result = await _handler.Handle(request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Description.Should().Be(request.Description);
        result.ScheduledDate.Should().Be(request.ScheduledDate);
        result.Id.Should().NotBeEmpty();
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        result.ModifiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        _workItemRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<WorkItem>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Create_WorkItem_Successfully_With_Only_Name()
    {
        // Arrange
        var request = new CreateWorkItemRequest("Test WorkItem");

        // Act
        var result = await _handler.Handle(request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Description.Should().BeNull();
        result.ScheduledDate.Should().BeNull();
        result.Id.Should().NotBeEmpty();

        _workItemRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<WorkItem>()), Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Handle_Should_Throw_ArgumentException_When_Name_Is_Invalid(string name)
    {
        // Arrange
        var request = new CreateWorkItemRequest(name, "Test description");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request));
        exception.ParamName.Should().Be("Name");
        exception.Message.Should().Contain("WorkItem name cannot be empty");
    }

    [Fact]
    public async Task Handle_Should_Pass_Correct_WorkItem_To_Repository()
    {
        // Arrange
        var request = new CreateWorkItemRequest("Test WorkItem", "Test description", DateTime.UtcNow.AddDays(1));
        WorkItem? capturedWorkItem = null;

        _workItemRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<WorkItem>()))
            .Callback<WorkItem>(workItem => capturedWorkItem = workItem);

        // Act
        await _handler.Handle(request);

        // Assert
        capturedWorkItem.Should().NotBeNull();
        capturedWorkItem!.Name.Should().Be(request.Name);
        capturedWorkItem.Description.Should().Be(request.Description);
        capturedWorkItem.ScheduledDate.Should().Be(request.ScheduledDate);
    }

    [Fact]
    public async Task Handle_Should_Return_Response_With_Correct_Properties()
    {
        // Arrange
        var scheduledDate = DateTime.UtcNow.AddDays(2);
        var request = new CreateWorkItemRequest("My WorkItem", "My description", scheduledDate);

        // Act
        var result = await _handler.Handle(request);

        // Assert
        result.Name.Should().Be("My WorkItem");
        result.Description.Should().Be("My description");
        result.ScheduledDate.Should().Be(scheduledDate);
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        result.ModifiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}