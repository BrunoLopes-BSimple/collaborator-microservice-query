using Application.Interfaces;
using Domain.Messages;
using Domain.Models;
using InterfaceAdapters.Consumers;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ConsumerTests;

public class UserUpdatedConsumerTests
{
    [Fact]
    public async Task Consume_ShouldUpdateUserReferenceAsync_WithCorrectData()
    {
        // arrange
        var userServiceDouble = new Mock<IUserService>();
        var consumer = new UserUpdatedConsumer(userServiceDouble.Object);

        var message = new UserUpdatedMessage(Guid.NewGuid(), "name", "surname", "email@gmail.com", It.IsAny<PeriodDateTime>());

        var context = Mock.Of<ConsumeContext<UserUpdatedMessage>>(c => c.Message == message);

        // act
        await consumer.Consume(context);

        // assert
        userServiceDouble.Verify(s => s.UpdateUserConsumed(message.Id, message.Names, message.Surnames, message.Email, message.PeriodDateTime), Times.Once);
    }
}
