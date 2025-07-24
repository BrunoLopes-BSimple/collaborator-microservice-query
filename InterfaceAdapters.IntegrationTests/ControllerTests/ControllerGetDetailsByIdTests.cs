using System.Net;
using Application.DTO.Collaborators;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure;
using Infrastructure.DataModel;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ControllerTests;

public class ControllerGetDetailsByIdTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;
    public ControllerGetDetailsByIdTests(IntegrationTestsWebApplicationFactory<Program> factory) : base(factory.CreateClient())
    { _factory = factory; }

    [Fact]
    public async Task GetDetailsById_ShouldReturnCorrectCollaborator()
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

        var names = "name";
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
        var response = await GetAndDeserializeAsync<CollabDetailsDTO>($"api/collaborators/{collabId}/details");

        // assert
        Assert.NotNull(response);
        Assert.Equal(collabId, response.CollabId);
        Assert.Equal(userId, response.UserId);
        Assert.Equal(names, response.Names);
        Assert.Equal(surname, response.Surnames);
        Assert.Equal(email, response.Email);
    }

    [Fact]
    public async Task GetDetailsById_ShouldReturnNotFoundWhenIdDoesNotExist()
    {
        // arrange
        var id = Guid.NewGuid();

        // act
        var response = await GetAsync($"api/collaborators/{id}/details");

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }


}
