using DevQuest.Application.WorkItems;
using DevQuest.Application.WorkItems.GetAllWorkItems;
using DevQuest.Domain.WorkItems;

namespace DevQuest.Application.Tests.WorkItems.GetAllWorkItems;

public class GetAllWorkItemsQueryHandlerTests
{
    private readonly Mock<IWorkItemRepository> _workItemRepositoryMock;
    private readonly GetAllWorkItemsQueryHandler _handler;

    public GetAllWorkItemsQueryHandlerTests()
    {
        _workItemRepositoryMock = new Mock<IWorkItemRepository>();
        _handler = new GetAllWorkItemsQueryHandler(_workItemRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_All_WorkItems_Successfully()
    {
        // Arrange
        var scheduledDate = DateTime.UtcNow.AddDays(1);
        var workItem1 = new WorkItem("WorkItem 1", "Description 1", scheduledDate);
        var workItem2 = new WorkItem("WorkItem 2", "Description 2");
        var workItem3 = new WorkItem("WorkItem 3");

        var workItems = new List<WorkItem> { workItem1, workItem2, workItem3 };

        _workItemRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(workItems);

        // Act
        var result = await _handler.Handle();

        // Assert
        result.Should().NotBeNull();
        result.WorkItems.Should().HaveCount(3);

        var workItemArray = result.WorkItems.ToArray();
        
        // Check first work item
        workItemArray[0].Id.Should().Be(workItem1.Id);
        workItemArray[0].Name.Should().Be("WorkItem 1");
        workItemArray[0].Description.Should().Be("Description 1");
        workItemArray[0].ScheduledDate.Should().Be(scheduledDate);

        // Check second work item
        workItemArray[1].Id.Should().Be(workItem2.Id);
        workItemArray[1].Name.Should().Be("WorkItem 2");
        workItemArray[1].Description.Should().Be("Description 2");
        workItemArray[1].ScheduledDate.Should().BeNull();

        // Check third work item
        workItemArray[2].Id.Should().Be(workItem3.Id);
        workItemArray[2].Name.Should().Be("WorkItem 3");
        workItemArray[2].Description.Should().BeNull();
        workItemArray[2].ScheduledDate.Should().BeNull();

        _workItemRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_Collection_When_No_WorkItems_Exist()
    {
        // Arrange
        var workItems = new List<WorkItem>();

        _workItemRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(workItems);

        // Act
        var result = await _handler.Handle();

        // Assert
        result.Should().NotBeNull();
        result.WorkItems.Should().BeEmpty();

        _workItemRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Map_WorkItem_Properties_Correctly()
    {
        // Arrange
        var scheduledDate = DateTime.UtcNow.AddDays(2);
        var workItem = new WorkItem("Test WorkItem", "Test Description", scheduledDate);
        var workItems = new List<WorkItem> { workItem };

        _workItemRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(workItems);

        // Act
        var result = await _handler.Handle();

        // Assert
        var summary = result.WorkItems.First();
        summary.Id.Should().Be(workItem.Id);
        summary.Name.Should().Be(workItem.Name);
        summary.Description.Should().Be(workItem.Description);
        summary.CreatedAt.Should().Be(workItem.CreatedAt);
        summary.ModifiedAt.Should().Be(workItem.ModifiedAt);
        summary.ScheduledDate.Should().Be(workItem.ScheduledDate);
    }

    [Fact]
    public async Task Handle_Should_Preserve_Order_Of_WorkItems()
    {
        // Arrange
        var workItem1 = new WorkItem("First");
        var workItem2 = new WorkItem("Second");
        var workItem3 = new WorkItem("Third");

        var workItems = new List<WorkItem> { workItem1, workItem2, workItem3 };

        _workItemRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(workItems);

        // Act
        var result = await _handler.Handle();

        // Assert
        var summaries = result.WorkItems.ToArray();
        summaries[0].Name.Should().Be("First");
        summaries[1].Name.Should().Be("Second");
        summaries[2].Name.Should().Be("Third");
    }
}