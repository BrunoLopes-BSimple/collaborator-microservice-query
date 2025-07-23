using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.UserRepositoryTests;

public class UserRepositoryGetByNamesAndSurnamesAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenSearchingByNamesAndSurnames_ThenReturnsMatchingUsers()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var names = "João";
        var surnames = "Silva";

        var userDouble = new Mock<IUser>();
        userDouble.Setup(u => u.Id).Returns(userId);
        userDouble.Setup(u => u.Email).Returns("joao@email.com");
        userDouble.Setup(u => u.Names).Returns(names);
        userDouble.Setup(u => u.Surnames).Returns(surnames);
        userDouble.Setup(u => u.PeriodDateTime).Returns(new PeriodDateTime());

        var userDM = new UserDataModel(userDouble.Object);
        context.Users.Add(userDM);
        await context.SaveChangesAsync();

        _mapper.Setup(m => m.Map<User>(It.Is<UserDataModel>(u => u.Id == userId))).Returns(new User(userDM.Id, userDM.Names, userDM.Surnames, userDM.Email, userDM.PeriodDateTime));

        var userRepo = new UserRepositoryEF(context, _mapper.Object);

        // Act
        var result = await userRepo.GetByNamesAndSurnamesAsync("João", "Silva");

        // Assert
        Assert.Single(result);
        Assert.Equal(userId, result.First().Id);
    }

    [Fact]
    public async Task WhenNamesAndSurnamesAreEmpty_ThenReturnsEmptyList()
    {
        // Arrange
        var userRepo = new UserRepositoryEF(context, _mapper.Object);

        // Act
        var result = await userRepo.GetByNamesAndSurnamesAsync("", "");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task WhenNamesIsNullOrEmpty_ThenSearchesOnlyBySurnames()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var user = new UserDataModel
        {
            Id = userId,
            Names = "Carlos",
            Surnames = "Silva",
            Email = "carlos@email.com",
            PeriodDateTime = new PeriodDateTime()
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        _mapper.Setup(m => m.Map<User>(It.Is<UserDataModel>(u => u.Id == userId))).Returns(new User(user.Id, user.Names, user.Surnames, user.Email, user.PeriodDateTime));

        var userRepo = new UserRepositoryEF(context, _mapper.Object);

        // Act
        var result = await userRepo.GetByNamesAndSurnamesAsync(null, "Silva");

        // Assert
        Assert.Single(result);
        Assert.Equal(userId, result.First().Id);
    }

    [Fact]
    public async Task WhenSurnamesIsEmpty_ThenSearchesOnlyByNames()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var user = new UserDataModel
        {
            Id = userId,
            Names = "Ana Maria",
            Surnames = "Ferreira",
            Email = "ana@email.com",
            PeriodDateTime = new PeriodDateTime()
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        _mapper.Setup(m => m.Map<User>(It.Is<UserDataModel>(u => u.Id == userId)))
               .Returns(new User(user.Id, user.Names, user.Surnames, user.Email, user.PeriodDateTime));

        var userRepo = new UserRepositoryEF(context, _mapper.Object);

        // Act
        var result = await userRepo.GetByNamesAndSurnamesAsync("Ana", "");

        // Assert
        Assert.Single(result);
        Assert.Equal(userId, result.First().Id);
    }



}
