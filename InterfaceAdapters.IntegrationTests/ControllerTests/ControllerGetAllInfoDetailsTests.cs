using System.Configuration;
using Application.DTO.Collaborators;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure;
using Infrastructure.DataModel;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ControllerTests;

public class ControllerGetAllInfoDetailsTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;
    public ControllerGetAllInfoDetailsTests(IntegrationTestsWebApplicationFactory<Program> factory) : base(factory.CreateClient())
    { _factory = factory; }


    [Fact]
    public async Task GetAllInfo_ShouldReturnListOfCollaborators()
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
        var response = await GetAndDeserializeAsync<List<CollabDetailsDTO>>("api/collaborators/details");

        // assert
        Assert.NotNull(response);
        Assert.Equal(collabId, response[0].CollabId);
        Assert.Equal(userId, response[0].UserId);
        Assert.Equal(names, response[0].Names);
        Assert.Equal(surname, response[0].Surnames);
        Assert.Equal(email, response[0].Email);
    }

}
