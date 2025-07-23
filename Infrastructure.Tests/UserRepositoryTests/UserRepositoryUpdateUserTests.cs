using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.UserRepositoryTests;

public class UserRepositoryUpdateUserTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenUpdatingExistingUser_ThenUpdatesAndReturnsUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userDM = new UserDataModel
        {
            Id = userId,
            Names = "Pedro",
            Surnames = "Vieira",
            Email = "pedro@example.com",
            PeriodDateTime = new PeriodDateTime(DateTime.UtcNow, DateTime.UtcNow.AddDays(5))
        };

        context.Users.Add(userDM);
        await context.SaveChangesAsync();

        var updatedUser = new User(userId, "Pedro Atualizado", "Vieira Atualizado", "novo@example.com", new PeriodDateTime(DateTime.UtcNow, DateTime.UtcNow.AddDays(10)));

        _mapper.Setup(m => m.Map<UserDataModel, User>(It.Is<UserDataModel>(u => u.Id == userId))).Returns(updatedUser);

        var repo = new UserRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.UpdateUser(updatedUser);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Pedro Atualizado", result.Names);
        Assert.Equal("Vieira Atualizado", result.Surnames);
        Assert.Equal("novo@example.com", result.Email);
    }

    [Fact]
    public async Task WhenUpdatingNonExistingUser_ThenReturnsNull()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "Fake", "User", "fake@example.com", new PeriodDateTime(DateTime.UtcNow, DateTime.UtcNow.AddDays(1)));
        var repo = new UserRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.UpdateUser(user);

        // Assert
        Assert.Null(result);
    }
}
