using Domain.IRepository;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DataModel;
using Domain.Visitor;
using AutoMapper;

namespace Infrastructure.Repositories;

public class CollaboratorRepositoryEF : GenericRepositoryEF<ICollaborator, Collaborator, CollaboratorDataModel>, ICollaboratorRepository
{
    private readonly IMapper _mapper;
    public CollaboratorRepositoryEF(AbsanteeContext context, IMapper mapper) : base(context, mapper)
    {
        _mapper = mapper;
    }

    public async Task<bool> ExistsByUserIdAsync(Guid userId)
    {
        return await _context.Set<CollaboratorDataModel>()
                             .AnyAsync(c => c.UserId == userId);
    }

    public async Task<bool> AlreadyExistsAsync(Guid collbId)
    {
        return await _context.Set<CollaboratorDataModel>().AnyAsync(c => c.Id == collbId);
    }

    public async Task<bool> IsRepeated(ICollaborator collaborator)
    {
        return await this._context.Set<CollaboratorDataModel>()
                .AnyAsync(c => c.UserId == collaborator.UserId
                    && c.PeriodDateTime._initDate <= collaborator.PeriodDateTime._finalDate
                    && collaborator.PeriodDateTime._initDate <= c.PeriodDateTime._finalDate);
    }

    public override ICollaborator? GetById(Guid id)
    {
        var collabDM = this._context.Set<CollaboratorDataModel>()
                            .FirstOrDefault(c => c.Id == id);

        if (collabDM == null)
            return null;

        var collab = _mapper.Map<CollaboratorDataModel, Collaborator>(collabDM);
        return collab;
    }

    public override async Task<ICollaborator?> GetByIdAsync(Guid id)
    {
        var collabDM = await this._context.Set<CollaboratorDataModel>()
                            .FirstOrDefaultAsync(c => c.Id == id);

        if (collabDM == null)
            return null;

        var collab = _mapper.Map<CollaboratorDataModel, Collaborator>(collabDM);
        return collab;
    }

    public async Task<IEnumerable<ICollaborator>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        var collabsDm = await this._context.Set<CollaboratorDataModel>()
                    .Where(c => ids.Contains(c.Id))
                    .ToListAsync();

        var collabs = collabsDm.Select(c => _mapper.Map<CollaboratorDataModel, Collaborator>(c));

        return collabs;
    }

    public async Task<IEnumerable<ICollaborator>> GetByUsersIdsAsync(IEnumerable<Guid> ids)
    {
        var collabsDm = await this._context.Set<CollaboratorDataModel>()
                    .Where(c => ids.Contains(c.UserId))
                    .ToListAsync();

        var collabs = collabsDm.Select(c => _mapper.Map<CollaboratorDataModel, Collaborator>(c));

        return collabs;
    }

    public async Task<Collaborator?> UpdateCollaborator(ICollaborator collab)
    {
        var collaboratorDM = await _context.Set<CollaboratorDataModel>()
            .FirstOrDefaultAsync(c => c.Id == collab.Id);

        if (collaboratorDM == null) return null;

        collaboratorDM.PeriodDateTime = collab.PeriodDateTime;
        
        _context.Set<CollaboratorDataModel>().Update(collaboratorDM);
        await _context.SaveChangesAsync();
        return _mapper.Map<CollaboratorDataModel, Collaborator>(collaboratorDM);
    }
}