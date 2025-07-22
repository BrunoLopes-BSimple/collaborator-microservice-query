using Domain.Interfaces;

namespace Application.Messaging;

public interface IMessagePublisher
{
    Task PublishCollaboratorCreatedAsync(ICollaborator collaborator);

}
