using Domain.Models;

namespace Application.DTO.Collaborators;

public record CollabDetailsDTO(Guid CollabId, Guid UserId, string Names, string Surnames, string Email, PeriodDateTime UserPeriod, PeriodDateTime CollaboratorPeriod);
