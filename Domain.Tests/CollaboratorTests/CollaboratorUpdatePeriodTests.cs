using Domain.Models;

namespace Domain.Tests.CollaboratorTests;

public class CollaboratorUpdatePeriodTests
{
    [Fact]
    public void UpdatePeriod_ShouldUpdatePeriodOfCollaborator()
    {
        // arrange
        var collabId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var period = new PeriodDateTime(DateTime.Now.AddDays(1), DateTime.Now.AddDays(50));

        var collab = new Collaborator(collabId, userId, period);

        var newPeriod = new PeriodDateTime(DateTime.Now.AddDays(2), DateTime.Now.AddDays(52));

        // act
        collab.UpdatePeriod(newPeriod);

        // assert
        Assert.Equal(newPeriod, collab.PeriodDateTime);
    }
}
