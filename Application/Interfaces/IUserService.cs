using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTO.Users;
using Domain.Interfaces;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<IUser?> AddUserReferenceAsync(ReceivedUserDTO dto);
    }
}