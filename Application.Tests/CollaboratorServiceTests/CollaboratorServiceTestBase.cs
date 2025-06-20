using Application.Messaging;
using Application.Services;
using AutoMapper;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Infrastructure;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.Tests.CollaboratorServiceTests
{
    public abstract class CollaboratorServiceTestBase : IDisposable
    {
        protected AbsanteeContext Context;
        protected ICollaboratorRepository CollaboratorRepository;
        protected Mock<ICollaboratorFactory> CollaboratorFactoryMock;
        protected Mock<IMessagePublisher> MessagePublisherMock;
        protected CollaboratorService Service;
        protected Mock<IMapper> MapperMock; // Adicione esta linha para que o mock seja acessível

        protected CollaboratorServiceTestBase()
        {
            var options = new DbContextOptionsBuilder<AbsanteeContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            Context = new AbsanteeContext(options);

            MapperMock = new Mock<IMapper>();

            // Adicione a configuração de mapeamento para Collaborator.
            // Isto diz ao mock o que fazer quando o repositório pedir para mapear um Collaborator.
            MapperMock.Setup(m => m.Map<Collaborator, CollaboratorDataModel>(It.IsAny<Collaborator>()))
                      .Returns((Collaborator c) => new CollaboratorDataModel
                      {
                          Id = c.Id,
                          UserId = c.UserId,
                          PeriodDateTime = c.PeriodDateTime
                      });

            // É também uma boa prática configurar o mapeamento inverso.
            MapperMock.Setup(m => m.Map<CollaboratorDataModel, Collaborator>(It.IsAny<CollaboratorDataModel>()))
                      .Returns((CollaboratorDataModel dm) => new Collaborator(dm.Id, dm.UserId, dm.PeriodDateTime));


            // O repositório real agora recebe o mock do mapper configurado
            CollaboratorRepository = new CollaboratorRepositoryEF(Context, MapperMock.Object);

            CollaboratorFactoryMock = new Mock<ICollaboratorFactory>();
            MessagePublisherMock = new Mock<IMessagePublisher>();

            Service = new CollaboratorService(
                CollaboratorRepository,
                CollaboratorFactoryMock.Object,
                MessagePublisherMock.Object
            );
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}