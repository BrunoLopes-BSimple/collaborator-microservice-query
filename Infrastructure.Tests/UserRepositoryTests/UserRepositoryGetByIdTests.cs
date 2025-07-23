using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.UserRepositoryTests;

public class UserRepositoryGetByIdTests : RepositoryTestBase
{
    [Fact]
    public void WhenSearchingById_ThenReturnsUser()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var userDouble = new Mock<IUser>();
        userDouble.Setup(u => u.Id).Returns(userId);
        userDouble.Setup(u => u.Email).Returns("email@email.com");
        userDouble.Setup(u => u.Names).Returns("name");
        userDouble.Setup(u => u.Surnames).Returns("surnames");

        var userDM = new UserDataModel(userDouble.Object);
        context.Users.Add(userDM);

        context.SaveChangesAsync();

        var expected = new Mock<IUser>().Object;

        _mapper.Setup(m => m.Map<UserDataModel, User>(It.Is<UserDataModel>(t => t.Id == userDM.Id))).Returns(new User(userId, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PeriodDateTime>()));

        var userRepo = new UserRepositoryEF(context, _mapper.Object);

        //Act 
        var result = userRepo.GetById(userId);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(userDM.Id, result.Id);
    }

    [Fact]
    public void WhenSearchingByIdWithNoUsers_ThenReturnsNull()
    {
        // Arrange
        var userRepo = new UserRepositoryEF(context, _mapper.Object);

        //Act 
        var result = userRepo.GetById(Guid.NewGuid());

        //Assert
        Assert.Null(result);
    }
}
