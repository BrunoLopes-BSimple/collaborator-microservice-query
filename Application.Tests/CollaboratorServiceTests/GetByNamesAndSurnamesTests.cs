using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Moq;

namespace Application.Tests.CollaboratorServiceTests;

public class GetByNamesAndSurnamesTests
{
    [Fact]
    public async Task GetByNamesAndSurnames_ShouldReturnCollabIds_WhenUsersAndCollabsExist()
    {
        // arrange
        var userRepoDouble = new Mock<IUserRepository>();
        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        var collabFactoryDouble = new Mock<ICollaboratorFactory>();

        var userId = Guid.NewGuid();
        var collabId = Guid.NewGuid();

        var userDouble = new Mock<IUser>();
        userDouble.Setup(u => u.Id).Returns(userId);

        var collabDouble = new Mock<ICollaborator>();
        collabDouble.Setup(c => c.Id).Returns(collabId);

        userRepoDouble.Setup(r => r.GetByNamesAndSurnamesAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<IUser> { userDouble.Object });

        collabRepoDouble.Setup(r => r.GetByUsersIdsAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(new List<ICollaborator> { collabDouble.Object });

        var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, userRepoDouble.Object);

        // act
        var result = await service.GetByNamesAndSurnames("Ana", "Silva");

        // assert
        Assert.Single(result);
        Assert.Contains(collabId, result);
    }

}
