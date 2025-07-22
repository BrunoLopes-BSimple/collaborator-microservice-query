using Domain.Models;
using Moq;

namespace Domain.Tests.UserTests;

public class UserConstructorTests
{
    [Fact]
    public void Constructor_ShouldCreateNewUser()
    {
        new User(Guid.NewGuid(), "name", "surname", "email@gmail.com", It.IsAny<PeriodDateTime>());
    }
}
