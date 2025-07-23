using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.UserRepositoryTests;

public class UserRepositoryGetByIdsAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenGettingUsersByIds_ThenReturnsCorrectUsers()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();

        var user1 = new UserDataModel
        {
            Id = userId1,
            Names = "Miguel",
            Surnames = "Pereira",
            Email = "miguel@email.com",
            PeriodDateTime = new PeriodDateTime()
        };

        var user2 = new UserDataModel
        {
            Id = userId2,
            Names = "Laura",
            Surnames = "Costa",
            Email = "laura@email.com",
            PeriodDateTime = new PeriodDateTime()
        };

        context.Users.AddRange(user1, user2);
        await context.SaveChangesAsync();

        _mapper.Setup(m => m.Map<User>(It.Is<UserDataModel>(u => u.Id == userId1)))
               .Returns(new User(user1.Id, user1.Names, user1.Surnames, user1.Email, user1.PeriodDateTime));
        _mapper.Setup(m => m.Map<User>(It.Is<UserDataModel>(u => u.Id == userId2)))
               .Returns(new User(user2.Id, user2.Names, user2.Surnames, user2.Email, user2.PeriodDateTime));

        var userRepo = new UserRepositoryEF(context, _mapper.Object);

        // Act
        var result = await userRepo.GetByIdsAsync(new List<Guid> { userId1, userId2 });

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.Contains(resultList, u => u.Id == userId1);
        Assert.Contains(resultList, u => u.Id == userId2);
    }

}
