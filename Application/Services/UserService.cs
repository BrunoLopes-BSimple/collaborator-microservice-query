using Application.DTO.Collaborators;
using Application.DTO.Users;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;

namespace Application.Services
{
    public class UserService
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
            var visitor = new UserDataModel() { Id=dto.Id, Names = dto.Names, Surnames = dto.Surnames, Email = dto.Email, PeriodDateTime = dto.PeriodDateTime };
            var newUser = _userFactory.Create(visitor);

            if (newUser == null) return null;

            return await _userRepository.AddAsync(newUser);
        }
    }
}