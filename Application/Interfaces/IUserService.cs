using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTO.Users;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<IUser?> AddUserReferenceAsync(ReceivedUserDTO dto);
        Task UpdateUserConsumed(Guid id, string names, string surnames, string email, PeriodDateTime periodDateTime);
    }
}