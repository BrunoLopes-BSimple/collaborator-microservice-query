using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Messages;
using MassTransit;

namespace WebApi.Consumers
{
    public class CollaboratorUpdatedConsumer : IConsumer<CollaboratorUpdatedMessage>
    {
        private readonly ICollaboratorService _collabService;

        public CollaboratorUpdatedConsumer(ICollaboratorService collabService)
        {
            _collabService = collabService;
        }

        public async Task Consume(ConsumeContext<CollaboratorUpdatedMessage> context)
        {
            await _collabService.UpdateCollaboratorReferenceAsync(context.Message.Id, context.Message.UserId, context.Message.PeriodDateTime);
        }
    }
}