using Application.Interfaces;
using Application.Services;
using Domain.Messages;
using MassTransit;

namespace WebApi.Consumers
{
    public class CollaboratorConsumer : IConsumer<CollaboratorCreatedMessage>
    {
        private readonly ICollaboratorService _collabService;

        public CollaboratorConsumer(ICollaboratorService collabService)
        {
            _collabService = collabService;
        }

        public async Task Consume(ConsumeContext<CollaboratorCreatedMessage> context)
        {
            await _collabService.AddCollaboratorReferenceAsync(context.Message.Id, context.Message.UserId, context.Message.PeriodDateTime);
        }
    }
}