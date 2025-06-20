using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTO;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.Tests.CollaboratorServiceTests
{
    public class CreateCollaboratorTests : CollaboratorServiceTestBase
    {
        [Fact]
        public async Task Create_Should_SaveCollaboratorAndPublishEvent_WhenSuccessful()
        {
            // Arrange 
            var createDto = new CreateCollaboratorDTO
            {
                UserId = Guid.NewGuid(),
                PeriodDateTime = new PeriodDateTime(DateTime.UtcNow, DateTime.UtcNow.AddYears(1))
            };

            var expectedCollaborator = new Collaborator(createDto.UserId, createDto.PeriodDateTime);

            CollaboratorFactoryMock.Setup(f => f.Create(createDto.UserId, createDto.PeriodDateTime)).ReturnsAsync(expectedCollaborator);

            // Act 
            var result = await Service.Create(createDto);

            // Assert 
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(expectedCollaborator.Id, result.Value.CollaboratorId);

            var collabInDb = await Context.Collaborators.FirstOrDefaultAsync(c => c.Id == expectedCollaborator.Id);
            Assert.NotNull(collabInDb);
            Assert.Equal(createDto.UserId, collabInDb.UserId);

            MessagePublisherMock.Verify(
                p => p.PublishCollaboratorCreatedAsync(
                    It.Is<ICollaborator>(c => c.Id == expectedCollaborator.Id)
                ),
                Times.Once);
        }
    }
}