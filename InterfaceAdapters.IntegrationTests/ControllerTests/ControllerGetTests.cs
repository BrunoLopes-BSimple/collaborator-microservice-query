using Domain.Interfaces;
using Domain.Models;
using Infrastructure;
using Infrastructure.DataModel;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ControllerTests;

public class ControllerGetTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;
    public ControllerGetTests(IntegrationTestsWebApplicationFactory<Program> factory) : base(factory.CreateClient())
    { _factory = factory; }

    [Fact]
    public async Task Get_ShouldReturnListOfGuid()
    {
        // arrange
        var collabId1 = Guid.NewGuid();
        var userId1 = Guid.NewGuid();
        var period1 = new PeriodDateTime(DateTime.Now.AddDays(5).ToUniversalTime(), DateTime.Now.AddDays(100).ToUniversalTime());
        var collab1Double1 = new Mock<ICollaborator>();
        collab1Double1.Setup(c => c.Id).Returns(collabId1);
        collab1Double1.Setup(c => c.UserId).Returns(userId1);
        collab1Double1.Setup(c => c.PeriodDateTime).Returns(period1);
        var collabDM1 = new CollaboratorDataModel(collab1Double1.Object);

        var collabId2 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var period2 = new PeriodDateTime(DateTime.Now.AddDays(5).ToUniversalTime(), DateTime.Now.AddDays(100).ToUniversalTime());
        var collab1Double2 = new Mock<ICollaborator>();
        collab1Double2.Setup(c => c.Id).Returns(collabId2);
        collab1Double2.Setup(c => c.UserId).Returns(userId2);
        collab1Double2.Setup(c => c.PeriodDateTime).Returns(period2);
        var collabDM2 = new CollaboratorDataModel(collab1Double2.Object);


        await using (var scope = _factory.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AbsanteeContext>();

            context.Collaborators.Add(collabDM1);
            context.Collaborators.Add(collabDM2);
            await context.SaveChangesAsync();
        }

        // act
        var response = await GetAndDeserializeAsync<List<Guid>>("api/collaborators");

        // assert
        Assert.NotNull(response);
        Assert.Equal(collabId1, response[0]);
        Assert.Equal(collabId2, response[1]);
    }
}
