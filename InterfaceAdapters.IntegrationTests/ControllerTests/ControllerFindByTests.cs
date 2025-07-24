using System.Net;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure;
using Infrastructure.DataModel;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ControllerTests;

public class ControllerFindByTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;
    public ControllerFindByTests(IntegrationTestsWebApplicationFactory<Program> factory) : base(factory.CreateClient())
    { _factory = factory; }

    [Fact]
    public async Task FindBy_ShouldReturnIdsWhenNameMatches()
    {
        // arrange
        var collabId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var collabPeriod = new PeriodDateTime(DateTime.UtcNow.AddDays(2), DateTime.UtcNow.AddDays(11));

        var collabDouble = new Mock<ICollaborator>();
        collabDouble.Setup(c => c.Id).Returns(collabId);
        collabDouble.Setup(c => c.UserId).Returns(userId);
        collabDouble.Setup(c => c.PeriodDateTime).Returns(collabPeriod);

        var collabDM = new CollaboratorDataModel(collabDouble.Object);

        var names = "Joao";
        var surname = "surname";
        var email = "email@gmail.com";
        var userPeriod = new PeriodDateTime(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(10));
        var userDouble = new Mock<IUser>();
        userDouble.Setup(u => u.Id).Returns(userId);
        userDouble.Setup(u => u.Names).Returns(names);
        userDouble.Setup(u => u.Surnames).Returns(surname);
        userDouble.Setup(u => u.Email).Returns(email);
        userDouble.Setup(u => u.PeriodDateTime).Returns(userPeriod);

        var userDM = new UserDataModel(userDouble.Object);


        await using (var scope = _factory.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AbsanteeContext>();
            context.Collaborators.Add(collabDM);
            context.Users.Add(userDM);

            await context.SaveChangesAsync();
        }

        // act
        var response = await GetAndDeserializeAsync<List<Guid>>($"api/collaborators/search?name=Joao");


        // assert
        Assert.NotNull(response);
        Assert.Equal(collabId, response[0]);
    }

    [Fact]
    public async Task FindBy_ShouldReturnIdsWhenSurnameMatches()
    {
        // arrange
        var collabId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var collabPeriod = new PeriodDateTime(DateTime.UtcNow.AddDays(2), DateTime.UtcNow.AddDays(11));

        var collabDouble = new Mock<ICollaborator>();
        collabDouble.Setup(c => c.Id).Returns(collabId);
        collabDouble.Setup(c => c.UserId).Returns(userId);
        collabDouble.Setup(c => c.PeriodDateTime).Returns(collabPeriod);

        var collabDM = new CollaboratorDataModel(collabDouble.Object);

        var names = "Luis";
        var surname = "Miguel";
        var email = "email@gmail.com";
        var userPeriod = new PeriodDateTime(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(10));
        var userDouble = new Mock<IUser>();
        userDouble.Setup(u => u.Id).Returns(userId);
        userDouble.Setup(u => u.Names).Returns(names);
        userDouble.Setup(u => u.Surnames).Returns(surname);
        userDouble.Setup(u => u.Email).Returns(email);
        userDouble.Setup(u => u.PeriodDateTime).Returns(userPeriod);

        var userDM = new UserDataModel(userDouble.Object);


        await using (var scope = _factory.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AbsanteeContext>();
            context.Collaborators.Add(collabDM);
            context.Users.Add(userDM);

            await context.SaveChangesAsync();
        }

        // act
        var response = await GetAndDeserializeAsync<List<Guid>>($"api/collaborators/search?surname=Miguel");


        // assert
        Assert.NotNull(response);
        Assert.Equal(collabId, response[0]);
    }

    [Fact]
    public async Task FindBy_ShouldReturnIdsWhenNameAndSurnameMatches()
    {
        // arrange
        var collabId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var collabPeriod = new PeriodDateTime(DateTime.UtcNow.AddDays(2), DateTime.UtcNow.AddDays(11));

        var collabDouble = new Mock<ICollaborator>();
        collabDouble.Setup(c => c.Id).Returns(collabId);
        collabDouble.Setup(c => c.UserId).Returns(userId);
        collabDouble.Setup(c => c.PeriodDateTime).Returns(collabPeriod);

        var collabDM = new CollaboratorDataModel(collabDouble.Object);

        var names = "Alberto";
        var surname = "Dias";
        var email = "email@gmail.com";
        var userPeriod = new PeriodDateTime(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(10));
        var userDouble = new Mock<IUser>();
        userDouble.Setup(u => u.Id).Returns(userId);
        userDouble.Setup(u => u.Names).Returns(names);
        userDouble.Setup(u => u.Surnames).Returns(surname);
        userDouble.Setup(u => u.Email).Returns(email);
        userDouble.Setup(u => u.PeriodDateTime).Returns(userPeriod);

        var userDM = new UserDataModel(userDouble.Object);


        await using (var scope = _factory.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AbsanteeContext>();
            context.Collaborators.Add(collabDM);
            context.Users.Add(userDM);

            await context.SaveChangesAsync();
        }

        // act
        var response = await GetAndDeserializeAsync<List<Guid>>($"api/collaborators/search?name=Alberto&surname=Dias");


        // assert
        Assert.NotNull(response);
        Assert.Equal(collabId, response[0]);
    }

    [Fact]
    public async Task FindBy_ShouldReturnBadRequestWhenNoParamsProvided()
    {
        // act
        var response = await GetAsync("api/collaborators/search");

        // assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task FindBy_ShouldReturnNotFoundWhenNoMatches()
    {
        // act
        var response = await GetAsync("api/collaborators/search?name=DoesNotExist");

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }



}
