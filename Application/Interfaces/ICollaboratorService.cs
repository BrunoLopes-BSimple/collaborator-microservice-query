
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
        Task<Result<IEnumerable<CollabDetailsDTO>>> GetAllInfo();
        Task<Result<CollabDetailsDTO>> GetDetailsById(Guid id);
        Task<IEnumerable<Guid>> GetByNames(string names);
        Task<IEnumerable<Guid>> GetBySurnames(string surnames);
        Task<IEnumerable<Guid>> GetByNamesAndSurnames(string names, string surnames);
    }
}