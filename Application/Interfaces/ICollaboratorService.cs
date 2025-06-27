
using Application.DTO.Collaborators;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Interfaces
{
    public interface ICollaboratorService
    {
        Task<Result<CollaboratorDTO>> GetById(Guid id);
        Task<Result<IEnumerable<Guid>>> GetAll();
        Task<ICollaborator?> UpdateCollaboratorReferenceAsync(Guid collabId, Guid userId, PeriodDateTime period);
        Task<ICollaborator?> AddCollaboratorReferenceAsync(Guid collabId, Guid userId, PeriodDateTime period);
    }
}