using Domain.Models;

namespace Application.DTO.Users;

public record ReceivedUserDTO(Guid Id, string Names, string Surnames, string Email, PeriodDateTime PeriodDateTime);
