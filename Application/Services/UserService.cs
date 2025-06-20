using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;

namespace Application.Services
{
    public class UserService
    {
        private IUserRepository _userRepository;
        private IUserFactory _userFactory;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IUser?> AddUserReferenceAsync(Guid userId)
        {
            var newUser = await _userFactory.Create(userId);

            if (newUser == null) return null;

            return await _userRepository.AddAsync(newUser);
        }
    }
}