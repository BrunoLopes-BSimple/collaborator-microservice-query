using Application.DTO.Collaborators;
using Application.DTO.Users;
using Application.Interfaces;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserFactory _userFactory;

        public UserService(IUserRepository userRepository, IUserFactory userFactory)
        {
            _userRepository = userRepository;
            _userFactory = userFactory;
        }

        public async Task<IUser?> AddUserReferenceAsync(ReceivedUserDTO dto)
        {
            var visitor = new UserDataModel() { Id = dto.Id, Names = dto.Names, Surnames = dto.Surnames, Email = dto.Email, PeriodDateTime = dto.PeriodDateTime };
            var newUser = _userFactory.Create(visitor);

            if (newUser == null) return null;

            return await _userRepository.AddAsync(newUser);
        }

        public async Task UpdateUserConsumed(Guid id, string names, string surnames, string email, PeriodDateTime periodDateTime)
        {
            var visitor = new UserDataModel()
            {
                Id = id,
                Names = names,
                Surnames = surnames,
                Email = email,
                PeriodDateTime = periodDateTime
            };

            var User = _userFactory.Create(visitor);

            await _userRepository.UpdateUser(User);
        }
    }
}