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
        Task<IEnumerable<IUser>> GetByIdsAsync(List<Guid> userIdsOfCollab);
        Task<IEnumerable<IUser>> GetByNamesAsync(string names);
        Task<IEnumerable<IUser>> GetBySurnamesAsync(string surnames);
        Task<IEnumerable<IUser>> GetByNamesAndSurnamesAsync(string names, string surnames);
        Task<IUser?> UpdateUser(IUser user_);
    }
}