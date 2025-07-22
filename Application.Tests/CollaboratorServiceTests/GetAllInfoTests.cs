using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.CollaboratorServiceTests;

public class GetAllInfoTests
{
    [Fact]
    public async Task GetAllInfo_ShouldReturnSuccessResult_WhenCollaboratorsAndUsersExist()
    {
        // arrange
        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        var collabFactoryDouble = new Mock<ICollaboratorFactory>();
        var userRepoDouble = new Mock<IUserRepository>();

        var collabId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var period = new PeriodDateTime(DateTime.Now.AddDays(1), DateTime.Now.AddDays(50));

        var collabDouble = new Mock<ICollaborator>();
        collabDouble.Setup(c => c.Id).Returns(collabId);
        collabDouble.Setup(c => c.UserId).Returns(userId);
        collabDouble.Setup(c => c.PeriodDateTime).Returns(period);

        var userDouble = new Mock<IUser>();
        userDouble.Setup(u => u.Id).Returns(userId);
        userDouble.Setup(u => u.Names).Returns("Ana");
        userDouble.Setup(u => u.Surnames).Returns("Silva");
        userDouble.Setup(u => u.Email).Returns("ana@example.com");
        userDouble.Setup(u => u.PeriodDateTime).Returns(period);

        collabRepoDouble.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<ICollaborator> { collabDouble.Object });

        userRepoDouble.Setup(repo => repo.GetByIdsAsync(It.IsAny<List<Guid>>())).ReturnsAsync(new List<IUser> { userDouble.Object });

        var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, userRepoDouble.Object);

        // act
        var result = await service.GetAllInfo();

        // assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
        Assert.NotNull(result.Value);
        var dto = result.Value.First();
        Assert.Equal(collabId, dto.CollabId);
        Assert.Equal(userId, dto.UserId);
        Assert.Equal("Ana", dto.Names);
        Assert.Equal("Silva", dto.Surnames);
        Assert.Equal("ana@example.com", dto.Email);
        Assert.Equal(period, dto.UserPeriod);
        Assert.Equal(period, dto.CollaboratorPeriod);
    }

    [Fact]
    public async Task GetAllInfo_ShouldReturnFailureResult_WhenExceptionIsThrownGettingCollaborators()
    {
        // arrange
        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        var collabFactoryDouble = new Mock<ICollaboratorFactory>();
        var userRepoDouble = new Mock<IUserRepository>();

        collabRepoDouble.Setup(repo => repo.GetAllAsync()).ThrowsAsync(new Exception("Failed to fetch collaborators"));

        var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, userRepoDouble.Object);

        // act
        var result = await service.GetAllInfo();

        // assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Contains("Failed to fetch collaborators", result.Error.Message);
    }



}
