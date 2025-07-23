using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.UserRepositoryTests;

public class UserRepositoryGetBySurnamesAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenSearchingBySurnames_ThenReturnsMatchingUsers()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var surnames = "Silva";

        var userDouble = new Mock<IUser>();
        userDouble.Setup(u => u.Id).Returns(userId);
        userDouble.Setup(u => u.Email).Returns("joao@email.com");
        userDouble.Setup(u => u.Names).Returns("João");
        userDouble.Setup(u => u.Surnames).Returns(surnames);
        userDouble.Setup(u => u.PeriodDateTime).Returns(new PeriodDateTime());

        var userDM = new UserDataModel(userDouble.Object);
        context.Users.Add(userDM);
        await context.SaveChangesAsync();

        _mapper.Setup(m => m.Map<User>(It.Is<UserDataModel>(u => u.Id == userId)))
               .Returns(new User(userDM.Id, userDM.Names, userDM.Surnames, userDM.Email, userDM.PeriodDateTime));

        var userRepo = new UserRepositoryEF(context, _mapper.Object);

        // Act
        var result = await userRepo.GetBySurnamesAsync("Silva");

        // Assert
        Assert.Single(result);
        Assert.Equal(userId, result.First().Id);
    }
}
