using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Moq;

namespace Application.Tests.CollaboratorServiceTests;

public class GetAllTests
{
    [Fact]
    public async Task GetAll_ShouldGetAll()
    {
        // arrange
        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        var collabFactoryDouble = new Mock<ICollaboratorFactory>();
        var userRepo = new Mock<IUserRepository>();

        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();

        var collabDouble1 = new Mock<ICollaborator>();
        var collabDouble2 = new Mock<ICollaborator>();

        collabDouble1.Setup(c => c.Id).Returns(id1);
        collabDouble2.Setup(c => c.Id).Returns(id2);

        var collabs = new List<ICollaborator> { collabDouble1.Object, collabDouble2.Object };

        var expected = new List<Guid> { id1, id2 };

        collabRepoDouble.Setup(cr => cr.GetAllAsync()).ReturnsAsync(collabs);

        var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, userRepo.Object);

        // act
        var result = await service.GetAll();

        // assert
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public async Task GetAll_ShouldReturnFailureResult_WhenExceptionIsThrown()
    {
        // arrange
        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        var collabFactoryDouble = new Mock<ICollaboratorFactory>();
        var userRepo = new Mock<IUserRepository>();

        var exceptionMessage = "error";

        collabRepoDouble.Setup(cr => cr.GetAllAsync()).ThrowsAsync(new Exception(exceptionMessage));

        var service = new CollaboratorService(
            collabRepoDouble.Object,
            collabFactoryDouble.Object,
            userRepo.Object
        );

        // act
        var result = await service.GetAll();

        // assert
        Assert.False(result.IsSuccess); 
        Assert.NotNull(result.Error);
        Assert.Contains(exceptionMessage, result.Error.Message);
    }

}
