using Application.Interfaces;
using Domain.Models;
using MassTransit;
using Moq;
using InterfaceAdapters.Consumers;
using Xunit;
using Domain.Messages;

namespace InterfaceAdapters.IntegrationTests.ConsumerTests;

public class CollaboratorConsumerTests
{
    [Fact]
    public async Task Consume_ShouldCallAddCollaboratorReferenceAsync_WithCorrectData()
    {
        // arrange
        var serviceDouble = new Mock<ICollaboratorService>();
        var consumer = new CollaboratorConsumer(serviceDouble.Object);

        var message = new CollaboratorCreatedMessage(Guid.NewGuid(), Guid.NewGuid(), new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(1)));

        var context = Mock.Of<ConsumeContext<CollaboratorCreatedMessage>>(c => c.Message == message);

        // act
        await consumer.Consume(context);

        // asset
        serviceDouble.Verify(s => s.AddCollaboratorReferenceAsync(message.Id, message.UserId, message.PeriodDateTime), Times.Once);
    }

}
