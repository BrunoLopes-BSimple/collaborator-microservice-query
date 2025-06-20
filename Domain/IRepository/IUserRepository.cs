using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Domain.IRepository
{
    public interface IUserRepository : IGenericRepositoryEF<IUser, User, IUserVisitor>
    {
        Task<bool> Exists(Guid ID);
    }
}