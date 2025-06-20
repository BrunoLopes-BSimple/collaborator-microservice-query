using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Messaging;
using Application.Services;
using MassTransit;

namespace WebApi.Consumers
{
    public class CollaboratorConsumer : IConsumer<CollaboratorCreatedEvent>
    {
        private readonly CollaboratorService _collabService;

        public CollaboratorConsumer(CollaboratorService collabService)
        {
            _collabService = collabService;
        }

        public async Task Consume(ConsumeContext<CollaboratorCreatedEvent> context)
        {
            await _collabService.AddCollaboratorReferenceAsync(context.Message.Id, context.Message.UserId, context.Message.PeriodDateTime);
        }
    }
}