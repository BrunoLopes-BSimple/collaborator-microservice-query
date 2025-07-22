using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.CollaboratorServiceTests;

public class GetByIdTests
{
    [Fact]
    public async Task GetById_ShouldReturnSuccessResult_WhenCollaboratorExists()
    {
        // arrange
        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        var collabFactoryDouble = new Mock<ICollaboratorFactory>();
        var userRepo = new Mock<IUserRepository>();

        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var periodDateTime = new PeriodDateTime(DateTime.Now.AddDays(1), DateTime.Now.AddDays(50));

        var collabDouble = new Mock<ICollaborator>();
        collabDouble.Setup(c => c.Id).Returns(id);
        collabDouble.Setup(c => c.UserId).Returns(userId);
        collabDouble.Setup(c => c.PeriodDateTime).Returns(periodDateTime);

        collabRepoDouble.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(collabDouble.Object);

        var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, userRepo.Object);

        // act
        var result = await service.GetById(id);

        // assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
        Assert.NotNull(result.Value);
        Assert.Equal(id, result.Value.Id);
        Assert.Equal(userId, result.Value.UserId);
        Assert.Equal(periodDateTime, result.Value.PeriodDateTime);
    }


    [Fact]
    public async Task GetById_ShouldReturnFailureResult_WhenExceptionIsThrown()
    {
        // arrange
        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        var collabFactoryDouble = new Mock<ICollaboratorFactory>();
        var userRepo = new Mock<IUserRepository>();

        var exceptionMessage = "error";
        var fakeId = Guid.NewGuid();

        collabRepoDouble
            .Setup(repo => repo.GetByIdAsync(fakeId))
            .ThrowsAsync(new Exception(exceptionMessage));

        var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, userRepo.Object);

        // act
        var result = await service.GetById(fakeId);

        // assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Contains(exceptionMessage, result.Error.Message);
    }

    [Fact]
    public async Task GetById_ShouldReturnFailureResult_WhenCollaboratorNotFound()
    {
        // arrange
        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        var collabFactoryDouble = new Mock<ICollaboratorFactory>();
        var userRepo = new Mock<IUserRepository>();

        var fakeId = Guid.NewGuid();

        collabRepoDouble
            .Setup(repo => repo.GetByIdAsync(fakeId))
            .ReturnsAsync((ICollaborator)null!); 

        var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, userRepo.Object);

        // act
        var result = await service.GetById(fakeId);

        // assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal("User not found", result.Error.Message);
    }


}
