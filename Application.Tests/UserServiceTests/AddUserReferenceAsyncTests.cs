/* using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.IRepository;
using Infrastructure;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.Tests.UserServiceTests
{
    public class AddUserReferenceAsyncTests : UserServiceTestBase
    {
        [Fact]
        public async Task AddUserReferenceAsync_ShouldAddUser_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var result = await UserService.AddUserReferenceAsync(userId);

            // Assert
            // 1. Verifica se o método retornou um objeto User
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);

            // 2. Verifica DIRETAMENTE na base de dados em memória se o registo foi criado
            var userInDb = await Context.ValidUserIds.FirstOrDefaultAsync(u => u.Id == userId);
            Assert.NotNull(userInDb);
            Assert.Equal(userId, userInDb.Id);
        }

        [Fact]
        public async Task AddUserReferenceAsync_ShouldReturnNull_WhenUserAlreadyExists()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Adiciona previamente o utilizador à base de dados para simular a sua existência
            Context.ValidUserIds.Add(new UserDataModel { Id = userId });
            await Context.SaveChangesAsync();

            // Act
            var result = await UserService.AddUserReferenceAsync(userId);

            // Assert
            // 1. O serviço deve retornar null, pois o utilizador já existia
            Assert.Null(result);

            // 2. Garante que não há duplicados na base de dados
            var count = await Context.ValidUserIds.CountAsync(u => u.Id == userId);
            Assert.Equal(1, count);
        }
    }
} */