using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Messaging;
using Application.Services;
using MassTransit;

namespace WebApi.Consumers
{
    public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly UserService _userService;

        public UserCreatedConsumer(UserService userService)
        {
            _userService = userService;
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var userId = context.Message.Id;
            await _userService.AddUserReferenceAsync(userId);
        }
    }
}