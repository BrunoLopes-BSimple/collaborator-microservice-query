using Domain.IRepository;
using Domain.Interfaces;
using Domain.Factory;
using Domain.Models;
using Application.DTO;
using AutoMapper;
using Infrastructure;
using Application.Messaging;
using Application.DTO.Collaborators;
using Application.Interfaces;
namespace Application.Services;

public class CollaboratorService : ICollaboratorService
{
    private ICollaboratorRepository _collaboratorRepository;
    private IUserRepository _userRepository;
    private ICollaboratorFactory _collaboratorFactory;

    public CollaboratorService(ICollaboratorRepository collaboratorRepository, ICollaboratorFactory collaboratorFactory, IUserRepository userRepository)
    {
        _collaboratorRepository = collaboratorRepository;
        _collaboratorFactory = collaboratorFactory;
        _userRepository = userRepository;
    }

    public async Task<ICollaborator?> AddCollaboratorReferenceAsync(Guid collabId, Guid userId, PeriodDateTime period)
    {
        var collabAlreadyExists = await _collaboratorRepository.AlreadyExistsAsync(collabId);

        if (collabAlreadyExists) return null;

        var newCollab = _collaboratorFactory.Create(collabId, userId, period);

        return await _collaboratorRepository.AddAsync(newCollab);
    }

    public async Task<ICollaborator?> UpdateCollaboratorReferenceAsync(Guid collabId, Guid userId, PeriodDateTime period)
    {
        var existingCollab = await _collaboratorRepository.GetByIdAsync(collabId);

        if (existingCollab == null)
        {
            return null;
        }

        existingCollab.UpdatePeriod(period);

        return await _collaboratorRepository.UpdateCollaborator(existingCollab);
    }

    public async Task<Result<IEnumerable<Guid>>> GetAll()
    {
        try
        {
            var collabs = await _collaboratorRepository.GetAllAsync();
            var collabIds = collabs.Select(U => U.Id);

            return Result<IEnumerable<Guid>>.Success(collabIds);
        }
        catch (Exception e)
        {
            return Result<IEnumerable<Guid>>.Failure(Error.InternalServerError(e.Message));
        }
    }

    public async Task<Result<CollaboratorDTO>> GetById(Guid id)
    {
        try
        {
            var collab = await _collaboratorRepository.GetByIdAsync(id);
            if (collab == null)
                return Result<CollaboratorDTO>.Failure(Error.NotFound("User not found"));

            var result = new CollaboratorDTO(collab.Id, collab.UserId, collab.PeriodDateTime);

            return Result<CollaboratorDTO>.Success(result);
        }
        catch (Exception e)
        {
            return Result<CollaboratorDTO>.Failure(Error.InternalServerError(e.Message));
        }
    }

    public async Task<Result<IEnumerable<CollabDetailsDTO>>> GetAllInfo()
    {
        try
        {
            var collabs = await _collaboratorRepository.GetAllAsync();
            var userIds = collabs.Select(c => c.UserId).ToList();
            var users = await _userRepository.GetByIdsAsync(userIds);

            var resultList = new List<CollabDetailsDTO>();

            foreach (var collab in collabs)
            {
                var user = users.FirstOrDefault(u => u.Id == collab.UserId);

                if (user != null)
                {
                    resultList.Add(new CollabDetailsDTO
                    {
                        CollabId = collab.Id,
                        UserId = user.Id,
                        Names = user.Names,
                        Surnames = user.Surnames,
                        Email = user.Email,
                        UserPeriod = user.PeriodDateTime,
                        CollaboratorPeriod = collab.PeriodDateTime
                    });
                }
            }

            return Result<IEnumerable<CollabDetailsDTO>>.Success(resultList);
        }
        catch (Exception e)
        {
            return Result<IEnumerable<CollabDetailsDTO>>.Failure(Error.InternalServerError(e.Message));
        }
    }

    public async Task<Result<CollabDetailsDTO>> GetDetailsById(Guid id)
    {
        try
        {

            var collab = await _collaboratorRepository.GetByIdAsync(id);
            if (collab == null)
                return Result<CollabDetailsDTO>.Failure(Error.NotFound("Collab not found"));

            var user = await _userRepository.GetByIdAsync(collab.UserId);
            if (user == null)
                return Result<CollabDetailsDTO>.Failure(Error.NotFound("User not found"));

            var result = new CollabDetailsDTO()
            {
                CollabId = collab.Id,
                UserId = user.Id,
                Names = user.Names,
                Surnames = user.Surnames,
                Email = user.Email,
                UserPeriod = user.PeriodDateTime,
                CollaboratorPeriod = collab.PeriodDateTime
            };

            return Result<CollabDetailsDTO>.Success(result);
        }
        catch (Exception e)
        {
            return Result<CollabDetailsDTO>.Failure(Error.InternalServerError(e.Message));
        }
    }

    public async Task<IEnumerable<Guid>> GetByNames(string names)
    {
        var users = await _userRepository.GetByNamesAsync(names);
        var userIds = users.Select(u => u.Id);
        var collabs = await _collaboratorRepository.GetByUsersIdsAsync(userIds);
        var collabIds = collabs.Select(c => c.Id);
        return collabIds;
    }

    public async Task<IEnumerable<Guid>> GetBySurnames(string surnames)
    {
        var users = await _userRepository.GetBySurnamesAsync(surnames);
        var userIds = users.Select(u => u.Id);
        var collabs = await _collaboratorRepository.GetByUsersIdsAsync(userIds);
        return collabs.Select(c => c.Id);
    }

    public async Task<IEnumerable<Guid>> GetByNamesAndSurnames(string names, string surnames)
    {
        var users = await _userRepository.GetByNamesAndSurnamesAsync(names, surnames);
        var userIds = users.Select(u => u.Id);
        var collabs = await _collaboratorRepository.GetByUsersIdsAsync(userIds);
        return collabs.Select(c => c.Id);
    }

    /*  // UC9 - Como gestor de RH, quero listar todos os colaboradores
     

     

     public async Task<CollabUpdatedDTO?> EditCollaborator(CollabData dto)
     {
         var collab = await _collaboratorRepository.GetByIdAsync(dto.CollabId);
         if (collab == null) return null;

         var user = await _userRepository.GetByIdAsync(dto.UserId);
         if (user == null) return null;

         user.UpdateEmail(dto.Email);
         user.UpdateName(dto.Names);
         user.UpdateSurname(dto.Surnames);
         user.UpdatePeriod(dto.UserPeriod);
         collab.UpdatePeriod(dto.CollaboratorPeriod);

         var updateCollabDetails = await _collaboratorRepository.UpdateCollaborator(collab);
         var updatedUserDetails = await _userRepository.UpdateUser(user);

         if (updateCollabDetails == null || updatedUserDetails == null) return null;

         await _context.SaveChangesAsync();

         return new CollabUpdatedDTO(dto.CollabId, dto.UserId, dto.Names, dto.Surnames, dto.Email, dto.UserPeriod, dto.CollaboratorPeriod);
     }

     

     

     public async Task<long> GetCount()
     {
         return await _collaboratorRepository.GetCount();
     }

     // uc10 - Como gestor de RG, quero listar todos os colaboradores que têm determinado nome ou sobrenome
    


     //UC15: Como gestor de RH, quero listar os colaboradores que já registaram períodos de férias superiores a x dias 
     public async Task<Result<IEnumerable<CollaboratorDTO>>> FindAllWithHolidayPeriodsLongerThan(int days)
     {
         try
         {
             var holidayPlans = await _holidayPlanRepository.FindAllWithHolidayPeriodsLongerThanAsync(days);
             var collabIds = holidayPlans.Select(hp => hp.CollaboratorId);
             var collabs = await _collaboratorRepository.GetByIdsAsync(collabIds);

             return Result<IEnumerable<CollaboratorDTO>>.Success(collabs.Select(_mapper.Map<CollaboratorDTO>));

         }
         catch (Exception e)
         {
             return Result<IEnumerable<CollaboratorDTO>>.Failure(Error.InternalServerError(e.Message));
         }
     }

     // US14 - Como gestor de RH, quero listar os collaboradores que têm de férias num período
     public async Task<Result<IEnumerable<CollaboratorDTO>>> FindAllWithHolidayPeriodsBetweenDates(PeriodDate periodDate)
     {
         try
         {
             var holidayPlans = await _holidayPlanRepository.FindHolidayPlansWithinPeriodAsync(periodDate);

             var collabIds = holidayPlans.Select(holidayPlans => holidayPlans.CollaboratorId);
             var collabs = await _collaboratorRepository.GetByIdsAsync(collabIds);

             return Result<IEnumerable<CollaboratorDTO>>.Success(collabs.Select(_mapper.Map<CollaboratorDTO>));
         }
         catch (Exception e)
         {
             return Result<IEnumerable<CollaboratorDTO>>.Failure(Error.InternalServerError(e.Message));
         }
     }

     public async Task<Result<IEnumerable<CollaboratorDTO>>> FindAllByProject(Guid projectId)
     {
         try
         {
             var assocs = await _associationProjectCollaboratorRepository.FindAllByProjectAsync(projectId);
             var collabsIds = assocs.Select(c => c.CollaboratorId);
             var collabs = await _collaboratorRepository.GetByIdsAsync(collabsIds);
             var result = collabs.Select(_mapper.Map<CollaboratorDTO>);
             return Result<IEnumerable<CollaboratorDTO>>.Success(result);
         }
         catch (Exception ex)
         {
             return Result<IEnumerable<CollaboratorDTO>>.Failure(Error.InternalServerError(ex.Message));
         }
     }

     public async Task<Result<IEnumerable<AssociationProjectCollaboratorDTO>>> FindAllAssociationsByProject(Guid projectId)
     {
         try
         {
             var assocs = await _associationProjectCollaboratorRepository.FindAllByProjectAsync(projectId);
             var collabsIds = assocs.Select(c => c.CollaboratorId);
             var collabs = await _collaboratorRepository.GetByIdsAsync(collabsIds);
             var users = await _userRepository.GetByIdsAsync(collabs.Select(c => c.UserId).ToList());

             var result = assocs.Select(assoc =>
             {
                 var collab = collabs.First(c => c.Id == assoc.CollaboratorId);
                 var user = users.First(u => u.Id == collab.UserId);

                 return new AssociationProjectCollaboratorDTO
                 {
                     Id = assoc.Id,
                     CollaboratorId = assoc.CollaboratorId,
                     CollaboratorEmail = user.Email,
                     ProjectId = assoc.ProjectId,
                     ProjectAcronym = null!,
                     PeriodDate = assoc.PeriodDate
                 };
             });

             return Result<IEnumerable<AssociationProjectCollaboratorDTO>>.Success(result);
         }
         catch (Exception ex)
         {
             return Result<IEnumerable<AssociationProjectCollaboratorDTO>>.Failure(Error.InternalServerError(ex.Message));
         }
     }

     public async Task<Result<IEnumerable<CollaboratorDTO>>> FindAllByProjectAndBetweenPeriod(Guid projectId, PeriodDate periodDate)
     {
         try
         {
             var assocs = await _associationProjectCollaboratorRepository.FindAllByProjectAndIntersectingPeriodAsync(projectId, periodDate);
             var collabsIds = assocs.Select(c => c.CollaboratorId);
             var collabs = await _collaboratorRepository.GetByIdsAsync(collabsIds);
             var result = collabs.Select(_mapper.Map<CollaboratorDTO>);
             return Result<IEnumerable<CollaboratorDTO>>.Success(result);
         }
         catch (Exception ex)
         {
             return Result<IEnumerable<CollaboratorDTO>>.Failure(Error.InternalServerError(ex.Message));
         }

     }

     public async Task<Result<ICollection<Guid>>> GetActiveCollaboratorsWithNoTrainingModuleFinishedInSubject(Guid subjectId)
     {
         IEnumerable<Collaborator> activeCollaborators;
         IEnumerable<AssociationTrainingModuleCollaborator> finishedCollaborators;
         try
         {
             // Step 1: Get all active collaborators
             activeCollaborators = await _collaboratorRepository.GetActiveCollaborators();

             // Step 2: Get training modules that are finished for the subject
             var finishedTrainingModules = await _trainingModuleRepository
                 .GetBySubjectIdAndFinished(subjectId, DateTime.UtcNow);

             var finishedTrainingModuleIds = finishedTrainingModules.Select(m => m.Id).ToList();

             // Step 3: Get collaborator-module links for those finished modules
             finishedCollaborators = await _assocTMCRepository
                 .GetByTrainingModuleIds(finishedTrainingModuleIds);

         }
         catch (Exception e)
         {
             return Result<ICollection<Guid>>.Failure(Error.InternalServerError(e.Message));
         }

         var collaboratorsWithFinishedModules = finishedCollaborators
             .Select(c => c.CollaboratorId)
             .Distinct()
             .ToHashSet();

         // Step 4: Filter active collaborators that are NOT in the above list
         var result = activeCollaborators
             .Where(c => !collaboratorsWithFinishedModules.Contains(c.Id))
             .Select(c => c.Id)
             .ToList();

         return Result<ICollection<Guid>>.Success(result);
     }

     public async Task<Result<ICollection<Guid>>> GetCompletedTrainingsAsync(Guid subjectId, DateTime fromDate)
     {
         // Garantir que a data de entrada também seja UTC
         var fromDateUtc = DateTime.SpecifyKind(fromDate, DateTimeKind.Utc);

         IEnumerable<Collaborator> activeCollaborators;
         IEnumerable<AssociationTrainingModuleCollaborator> trainingModuleCollaborators;
         try
         {
             // Passo 1 - Confirmar a existencia do subjectId
             activeCollaborators = await _collaboratorRepository.GetActiveCollaborators();

             // Passo 2 - Procurar os Training Modules que foram terminados antes da fromDate de um dado SubjectId
             var finishedTrainingModules = await _trainingModuleRepository
                 .GetBySubjectAndAfterDateFinished(subjectId, fromDateUtc);

             var finishedTrainingModuleIds = finishedTrainingModules.Select(m => m.Id).ToList();

             // Passo 3: Obter as ligações de colaboradores que completaram esses módulos
             trainingModuleCollaborators = await _assocTMCRepository
                 .GetByTrainingModuleIds(finishedTrainingModuleIds);
         }
         catch (Exception e)
         {
             return Result<ICollection<Guid>>.Failure(Error.InternalServerError(e.Message));
         }

         var activeCollaboratorIds = activeCollaborators.Select(c => c.Id).ToList();

         var collaboratorsWithFinishedModules = trainingModuleCollaborators
             .Select(c => c.CollaboratorId)
             .Distinct()
             .ToHashSet();

         // Passo 4: Filtrar os colaboradores ativos que participaram e terminaram esses módulos
         var result = activeCollaborators
             .Where(c => collaboratorsWithFinishedModules.Contains(c.Id))
             .Select(c => c.Id)  // Alteração aqui, estamos agora retornando apenas o ID
             .ToList();

         return Result<ICollection<Guid>>.Success(result);
     }

     //UC13
     public async Task<IEnumerable<HolidayPeriodDTO>> FindHolidayPeriodsByCollaboratorBetweenDatesAsync(Guid collaboratorId, PeriodDate periodDate)
     {
         var result = await _holidayPlanRepository.FindHolidayPeriodsByCollaboratorBetweenDatesAsync(collaboratorId, periodDate);

         return result.Select(_mapper.Map<HolidayPeriod, HolidayPeriodDTO>);
     }

     public async Task<IEnumerable<ICollaborator>> GetAllByFinishedTrainingModuleInSubjectAfterPeriod(Guid subjectId, DateTime date)
     {
         // Step 1: Get training modules that are finished for the subject
         var finishedTrainingModules = await _trainingModuleRepository
             .GetBySubjectIdAndFinished(subjectId, date);

         var trainingModulesIds = finishedTrainingModules.Select(t => t.Id).ToList();

         // Step 2: Get collaborator-module links for those finished modules
         var trainingModuleCollaborators = await _assocTMCRepository.GetByTrainingModuleIds(trainingModulesIds);

         var collabsIds = trainingModuleCollaborators.Select(t => t.CollaboratorId).ToList();

         // Step 3: Get collaborators
         var collabs = await _collaboratorRepository.GetByIdsAsync(collabsIds);

         return collabs;
     }

     public async Task<IEnumerable<Guid>> GetAllIds()
     {
         var collabs = await _collaboratorRepository.GetAllAsync();
         return collabs.Select(c => c.Id);
     }

     public async Task<IEnumerable<AssociationProjectCollaboratorDTO>> GetCollaboratorProjects(Guid CollabId)
     {
         // Get all associations connected to collaborator ID
         var assocs = await _associationProjectCollaboratorRepository.FindAllByCollaboratorAsync(CollabId);

         // Find and get all projects associated with collaborator
         var projectIds = assocs.Select(a => a.ProjectId);
         var projects = await _projectRepository.GetByIdAsync(projectIds);

         // Converto into AssociationProjectCollaboratorDTO
         var result = assocs.Select(a =>
         {
             var currAssoc = new AssociationProjectCollaboratorDTO();
             currAssoc.Id = a.Id;
             currAssoc.CollaboratorId = a.CollaboratorId;
             currAssoc.ProjectId = a.ProjectId;
             currAssoc.ProjectAcronym = projects.First(p => p.Id == a.ProjectId).Acronym;
             currAssoc.PeriodDate = a.PeriodDate;

             return currAssoc;
         });

         return result;
     } */
}
