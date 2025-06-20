using Application.Messaging;
using MassTransit;
using Moq;
using WebApi.Publishers;
using Xunit;

namespace WebApi.IntegrationTests.PublisherTests
{
    public class PublishCollaboratorTests
    {
        [Fact]
        public async Task Should_publish_collaborator_created_event()
        {
            // Arrange
            var mock = new Mock<IPublishEndpoint>();
            var publisher = new MassTransitPublisher(mock.Object);
            var collaboratorId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var period = new Domain.Models.PeriodDateTime(DateTime.UtcNow, DateTime.UtcNow.AddYears(1));

            // Act
            var collaborator = Mock.Of<Domain.Interfaces.ICollaborator>(c =>
                c.Id == collaboratorId &&
                c.UserId == userId &&
                c.PeriodDateTime == period);

            await publisher.PublishCollaboratorCreatedAsync(collaborator);

            // Assert
            mock.Verify(p => p.Publish(
                It.Is<CollaboratorCreatedEvent>(m =>
                    m.Id == collaboratorId &&
                    m.UserId == userId &&
                    m.PeriodDateTime == period
                ),
                It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
    }
}