using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;

namespace Infrastructure.Tests.UserRepositoryTests;

public class UserRepositoryExistsTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenUserExists_ThenExistsReturnsTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var user = new UserDataModel
        {
            Id = userId,
            Names = "André",
            Surnames = "Martins",
            Email = "andre@email.com",
            PeriodDateTime = new PeriodDateTime()
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        var userRepo = new UserRepositoryEF(context, _mapper.Object);

        // Act
        var exists = await userRepo.Exists(userId);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task WhenUserDoesNotExist_ThenExistsReturnsFalse()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        var userRepo = new UserRepositoryEF(context, _mapper.Object);

        // Act
        var exists = await userRepo.Exists(nonExistingId);

        // Assert
        Assert.False(exists);
    }


}
