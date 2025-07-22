using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTO.Users;
using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.Tests.UserServiceTests
{
    public class AddUserReferenceAsyncTests 
    {
        [Fact]
        public async Task AddUserReferenceAsync_ShouldReturnUser_WhenFactoryCreatesUserAndRepositoryAddsSuccessfully()
        {
            // arrange
            var userRepoDouble = new Mock<IUserRepository>();
            var userFactoryDouble = new Mock<IUserFactory>();

            var period = new PeriodDateTime(DateTime.Now.AddDays(1), DateTime.Now.AddDays(50));
            var dto = new ReceivedUserDTO(Guid.NewGuid(), "João", "Silva", "joao.silva@example.com", period);

            var userToCreate = new User(dto.Id, dto.Names, dto.Surnames, dto.Email, dto.PeriodDateTime);

            userFactoryDouble.Setup(f => f.Create(It.IsAny<UserDataModel>())).Returns(userToCreate);
            userRepoDouble.Setup(r => r.AddAsync(userToCreate)).ReturnsAsync(userToCreate);

            var service = new UserService(userRepoDouble.Object, userFactoryDouble.Object);

            // act
            var result = await service.AddUserReferenceAsync(dto);

            // assert
            Assert.NotNull(result);
            Assert.Equal(dto.Id, result!.Id);
            Assert.Equal(dto.Names, result.Names);
            Assert.Equal(dto.Surnames, result.Surnames);
            Assert.Equal(dto.Email, result.Email);
            Assert.Equal(dto.PeriodDateTime, result.PeriodDateTime);
        }

        [Fact]
        public async Task AddUserReferenceAsync_ShouldReturnNull_WhenFactoryReturnsNull()
        {
            // arrange
            var userRepoDouble = new Mock<IUserRepository>();
            var userFactoryDouble = new Mock<IUserFactory>();

            var period = new PeriodDateTime(DateTime.Now.AddDays(1), DateTime.Now.AddDays(50));
            var dto = new ReceivedUserDTO(Guid.NewGuid(), "João", "Silva", "joao.silva@example.com", period);

            userFactoryDouble.Setup(f => f.Create(It.IsAny<UserDataModel>())).Returns((User?)null);

            var service = new UserService(userRepoDouble.Object, userFactoryDouble.Object);

            // act
            var result = await service.AddUserReferenceAsync(dto);

            // assert
            Assert.Null(result);
            userRepoDouble.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }
    }
}