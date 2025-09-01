using DevQuest.Domain.WorkItems;

namespace DevQuest.Domain.Tests.WorkItems;

public class WorkItemTests
{
    [Fact]
    public void WorkItem_Constructor_Should_Create_WorkItem_With_Valid_Properties()
    {
        // Arrange
        const string name = "Test WorkItem";
        const string description = "Test description";
        var scheduledDate = DateTime.UtcNow.AddDays(1);

        // Act
        var workItem = new WorkItem(name, description, scheduledDate);

        // Assert
        workItem.Should().NotBeNull();
        workItem.Id.Should().NotBeEmpty();
        workItem.Name.Should().Be(name);
        workItem.Description.Should().Be(description);
        workItem.ScheduledDate.Should().Be(scheduledDate);
        workItem.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        workItem.ModifiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void WorkItem_Constructor_Should_Create_WorkItem_With_Only_Name()
    {
        // Arrange
        const string name = "Test WorkItem";

        // Act
        var workItem = new WorkItem(name);

        // Assert
        workItem.Should().NotBeNull();
        workItem.Id.Should().NotBeEmpty();
        workItem.Name.Should().Be(name);
        workItem.Description.Should().BeNull();
        workItem.ScheduledDate.Should().BeNull();
        workItem.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        workItem.ModifiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void WorkItem_Should_Generate_Unique_Ids()
    {
        // Arrange & Act
        var workItem1 = new WorkItem("WorkItem 1");
        var workItem2 = new WorkItem("WorkItem 2");

        // Assert
        workItem1.Id.Should().NotBe(workItem2.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("   ")]
    public void WorkItem_Constructor_Should_Throw_ArgumentException_When_Name_Is_Invalid(string name)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new WorkItem(name));
        exception.ParamName.Should().Be("name");
        exception.Message.Should().Contain("WorkItem name cannot be null, empty, or whitespace.");
    }

    [Fact]
    public void UpdateName_Should_Update_Name_And_ModifiedAt()
    {
        // Arrange
        var workItem = new WorkItem("Original Name");
        var originalModifiedAt = workItem.ModifiedAt;
        const string newName = "Updated Name";

        // Act
        Thread.Sleep(10); // Ensure time difference
        workItem.UpdateName(newName);

        // Assert
        workItem.Name.Should().Be(newName);
        workItem.ModifiedAt.Should().BeAfter(originalModifiedAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("   ")]
    public void UpdateName_Should_Throw_ArgumentException_When_Name_Is_Invalid(string name)
    {
        // Arrange
        var workItem = new WorkItem("Original Name");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => workItem.UpdateName(name));
        exception.ParamName.Should().Be("name");
        exception.Message.Should().Contain("WorkItem name cannot be null, empty, or whitespace.");
    }

    [Fact]
    public void UpdateDescription_Should_Update_Description_And_ModifiedAt()
    {
        // Arrange
        var workItem = new WorkItem("Test WorkItem");
        var originalModifiedAt = workItem.ModifiedAt;
        const string newDescription = "Updated description";

        // Act
        Thread.Sleep(10); // Ensure time difference
        workItem.UpdateDescription(newDescription);

        // Assert
        workItem.Description.Should().Be(newDescription);
        workItem.ModifiedAt.Should().BeAfter(originalModifiedAt);
    }

    [Fact]
    public void UpdateDescription_Should_Allow_Null_Description()
    {
        // Arrange
        var workItem = new WorkItem("Test WorkItem", "Original description");
        var originalModifiedAt = workItem.ModifiedAt;

        // Act
        Thread.Sleep(10); // Ensure time difference
        workItem.UpdateDescription(null);

        // Assert
        workItem.Description.Should().BeNull();
        workItem.ModifiedAt.Should().BeAfter(originalModifiedAt);
    }

    [Fact]
    public void UpdateScheduledDate_Should_Update_ScheduledDate_And_ModifiedAt()
    {
        // Arrange
        var workItem = new WorkItem("Test WorkItem");
        var originalModifiedAt = workItem.ModifiedAt;
        var newScheduledDate = DateTime.UtcNow.AddDays(2);

        // Act
        Thread.Sleep(10); // Ensure time difference
        workItem.UpdateScheduledDate(newScheduledDate);

        // Assert
        workItem.ScheduledDate.Should().Be(newScheduledDate);
        workItem.ModifiedAt.Should().BeAfter(originalModifiedAt);
    }

    [Fact]
    public void UpdateScheduledDate_Should_Allow_Null_ScheduledDate()
    {
        // Arrange
        var originalScheduledDate = DateTime.UtcNow.AddDays(1);
        var workItem = new WorkItem("Test WorkItem", null, originalScheduledDate);
        var originalModifiedAt = workItem.ModifiedAt;

        // Act
        Thread.Sleep(10); // Ensure time difference
        workItem.UpdateScheduledDate(null);

        // Assert
        workItem.ScheduledDate.Should().BeNull();
        workItem.ModifiedAt.Should().BeAfter(originalModifiedAt);
    }

    [Fact]
    public void Update_Should_Update_All_Properties_And_ModifiedAt()
    {
        // Arrange
        var workItem = new WorkItem("Original Name", "Original description", DateTime.UtcNow.AddDays(1));
        var originalModifiedAt = workItem.ModifiedAt;
        const string newName = "Updated Name";
        const string newDescription = "Updated description";
        var newScheduledDate = DateTime.UtcNow.AddDays(3);

        // Act
        Thread.Sleep(10); // Ensure time difference
        workItem.Update(newName, newDescription, newScheduledDate);

        // Assert
        workItem.Name.Should().Be(newName);
        workItem.Description.Should().Be(newDescription);
        workItem.ScheduledDate.Should().Be(newScheduledDate);
        workItem.ModifiedAt.Should().BeAfter(originalModifiedAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Update_Should_Throw_ArgumentException_When_Name_Is_Invalid(string name)
    {
        // Arrange
        var workItem = new WorkItem("Original Name");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => workItem.Update(name));
        exception.ParamName.Should().Be("name");
        exception.Message.Should().Contain("WorkItem name cannot be null, empty, or whitespace.");
    }

    [Fact]
    public void Update_Should_Handle_Null_Optional_Parameters()
    {
        // Arrange
        var workItem = new WorkItem("Original Name", "Original description", DateTime.UtcNow.AddDays(1));
        var originalModifiedAt = workItem.ModifiedAt;
        const string newName = "Updated Name";

        // Act
        Thread.Sleep(10); // Ensure time difference
        workItem.Update(newName);

        // Assert
        workItem.Name.Should().Be(newName);
        workItem.Description.Should().BeNull();
        workItem.ScheduledDate.Should().BeNull();
        workItem.ModifiedAt.Should().BeAfter(originalModifiedAt);
    }
}