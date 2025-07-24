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

        var result = await _collaboratorRepository.UpdateCollaborator(existingCollab);
        return result;
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
                    resultList.Add(new CollabDetailsDTO(collab.Id, user.Id, user.Names, user.Surnames, user.Email, user.PeriodDateTime, collab.PeriodDateTime));
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

            var result = new CollabDetailsDTO(collab.Id, user.Id, user.Names, user.Surnames, user.Email, user.PeriodDateTime, collab.PeriodDateTime);

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
}
