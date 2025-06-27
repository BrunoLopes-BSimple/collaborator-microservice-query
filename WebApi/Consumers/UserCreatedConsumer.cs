using Application.DTO.Users;
using Application.Services;
using Domain.Messages;
using MassTransit;

namespace WebApi.Consumers
{
    public class UserCreatedConsumer : IConsumer<UserCreatedMessage>
    {
        private readonly UserService _userService;

        public UserCreatedConsumer(UserService userService)
        {
            _userService = userService;
        }

        public async Task Consume(ConsumeContext<UserCreatedMessage> context)
        {

            Console.WriteLine($"XXX {context.Message.Names}");
            var receivedCollab = new ReceivedUserDTO(context.Message.Id, context.Message.Names, context.Message.Surnames, context.Message.Email, context.Message.PeriodDateTime);

            await _userService.AddUserReferenceAsync(receivedCollab);
        }
    }
}