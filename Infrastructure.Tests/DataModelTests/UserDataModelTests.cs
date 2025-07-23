using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Moq;

namespace Infrastructure.Tests.DataModelTests;

public class UserDataModelTests
{
    [Fact]
    public void Constructor_WithIUser_ShouldCopyAllFields()
    {
        // Arrange
        var id = Guid.NewGuid();
        var period = new PeriodDateTime(DateTime.UtcNow, DateTime.UtcNow.AddDays(30));

        var userDouble = new Mock<IUser>();
        userDouble.Setup(u => u.Id).Returns(id);
        userDouble.Setup(u => u.Names).Returns("John");
        userDouble.Setup(u => u.Surnames).Returns("Doe");
        userDouble.Setup(u => u.Email).Returns("john.doe@example.com");
        userDouble.Setup(u => u.PeriodDateTime).Returns(period);

        // Act
        var model = new UserDataModel(userDouble.Object);

        // Assert
        Assert.Equal(id, model.Id);
        Assert.Equal("John", model.Names);
        Assert.Equal("Doe", model.Surnames);
        Assert.Equal("john.doe@example.com", model.Email);
        Assert.Equal(period, model.PeriodDateTime);
    }

    [Fact]
    public void Constructor_WithEmptyGuid_ShouldNotAssignId()
    {
        // Arrange
        var userDouble = new Mock<IUser>();
        userDouble.Setup(u => u.Id).Returns(Guid.Empty);
        userDouble.Setup(u => u.Names).Returns("Test");
        userDouble.Setup(u => u.Surnames).Returns("User");
        userDouble.Setup(u => u.Email).Returns("test@example.com");
        userDouble.Setup(u => u.PeriodDateTime).Returns(new PeriodDateTime(DateTime.UtcNow, DateTime.UtcNow.AddDays(1)));

        // Act
        var model = new UserDataModel(userDouble.Object);

        // Assert
        Assert.Equal(Guid.Empty, model.Id);
    }

    [Fact]
    public void ParameterlessConstructor_ShouldInitializeDefaults()
    {
        // Act
        var model = new UserDataModel();

        // Assert
        Assert.Equal(Guid.Empty, model.Id);
        Assert.Null(model.Names);
        Assert.Null(model.Surnames);
        Assert.Null(model.Email);
        Assert.Null(model.PeriodDateTime);
    }
}
