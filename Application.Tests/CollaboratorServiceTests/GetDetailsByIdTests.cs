using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.CollaboratorServiceTests;

public class GetDetailsByIdTests
{
    [Fact]
    public async Task GetDetailsById_ShouldReturnCollabDetails_WhenCollabAndUserExist()
    {
        // arrange
        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        var userRepoDouble = new Mock<IUserRepository>();
        var collabFactoryDouble = new Mock<ICollaboratorFactory>();

        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var period = new PeriodDateTime(DateTime.Now.AddDays(1), DateTime.Now.AddDays(50));

        var collabDouble = new Mock<ICollaborator>();
        collabDouble.Setup(c => c.Id).Returns(id);
        collabDouble.Setup(c => c.UserId).Returns(userId);
        collabDouble.Setup(c => c.PeriodDateTime).Returns(period);

        var userDouble = new Mock<IUser>();
        userDouble.Setup(u => u.Id).Returns(userId);
        userDouble.Setup(u => u.Names).Returns("Ana");
        userDouble.Setup(u => u.Surnames).Returns("Silva");
        userDouble.Setup(u => u.Email).Returns("ana.silva@example.com");
        userDouble.Setup(u => u.PeriodDateTime).Returns(period);

        collabRepoDouble.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(collabDouble.Object);
        userRepoDouble.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(userDouble.Object);

        var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, userRepoDouble.Object);

        // act
        var result = await service.GetDetailsById(id);

        // assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(id, result.Value.CollabId);
        Assert.Equal(userId, result.Value.UserId);
        Assert.Equal("Ana", result.Value.Names);
        Assert.Equal("Silva", result.Value.Surnames);
        Assert.Equal("ana.silva@example.com", result.Value.Email);
        Assert.Equal(period, result.Value.UserPeriod);
        Assert.Equal(period, result.Value.CollaboratorPeriod);
    }

    [Fact]
    public async Task GetDetailsById_ShouldReturnFailure_WhenCollabNotFound()
    {
        // arrange
        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        var userRepoDouble = new Mock<IUserRepository>();
        var collabFactoryDouble = new Mock<ICollaboratorFactory>();

        var id = Guid.NewGuid();

        collabRepoDouble.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ICollaborator?)null);

        var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, userRepoDouble.Object);

        // act
        var result = await service.GetDetailsById(id);

        // assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Collab not found", result.Error.Message);
    }

    [Fact]
    public async Task GetDetailsById_ShouldReturnFailure_WhenUserNotFound()
    {
        // arrange
        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        var userRepoDouble = new Mock<IUserRepository>();
        var collabFactoryDouble = new Mock<ICollaboratorFactory>();

        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var collabDouble = new Mock<ICollaborator>();
        collabDouble.Setup(c => c.Id).Returns(id);
        collabDouble.Setup(c => c.UserId).Returns(userId);
        collabDouble.Setup(c => c.PeriodDateTime).Returns(new PeriodDateTime(DateTime.Now.AddDays(1), DateTime.Now.AddDays(50)));

        collabRepoDouble.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(collabDouble.Object);
        userRepoDouble.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((IUser?)null);

        var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, userRepoDouble.Object);

        // act
        var result = await service.GetDetailsById(id);

        // assert
        Assert.False(result.IsSuccess);
        Assert.Equal("User not found", result.Error.Message);
    }

    [Fact]
    public async Task GetDetailsById_ShouldReturnFailure_WhenExceptionIsThrown()
    {
        // arrange
        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        var userRepoDouble = new Mock<IUserRepository>();
        var collabFactoryDouble = new Mock<ICollaboratorFactory>();

        var id = Guid.NewGuid();

        collabRepoDouble.Setup(r => r.GetByIdAsync(id)).ThrowsAsync(new Exception("error"));

        var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, userRepoDouble.Object);

        // act
        var result = await service.GetDetailsById(id);

        // assert
        Assert.False(result.IsSuccess);
        Assert.Contains("error", result.Error.Message);
    }

}
