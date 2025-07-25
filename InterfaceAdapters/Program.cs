using Application.DTO.Collaborators;
using Application.DTO.Users;
using Application.Interfaces;
using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Resolvers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using InterfaceAdapters.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AbsanteeContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

//Services
builder.Services.AddTransient<ICollaboratorService, CollaboratorService>();
builder.Services.AddTransient<IUserService, UserService>();


//Repositories
builder.Services.AddTransient<IUserRepository, UserRepositoryEF>();
builder.Services.AddTransient<ICollaboratorRepository, CollaboratorRepositoryEF>();


//Factories
builder.Services.AddTransient<ICollaboratorFactory, CollaboratorFactory>();
builder.Services.AddTransient<IUserFactory, UserFactory>();


//Mappers
builder.Services.AddTransient<UserDataModelConverter>();
builder.Services.AddTransient<CollaboratorDataModelConverter>();
builder.Services.AddAutoMapper(cfg =>
{
    //DataModels
    cfg.AddProfile<DataModelMappingProfile>();

    //DTO
    cfg.CreateMap<User, UserDTO>();
    cfg.CreateMap<Collaborator, CollaboratorDTO>();
});

// MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedConsumer>();
    x.AddConsumer<CollaboratorConsumer>();
    x.AddConsumer<CollaboratorUpdatedConsumer>();
    x.AddConsumer<UserUpdatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");
        var instance = InstanceInfo.InstanceId;
        cfg.ReceiveEndpoint($"collaborators-query-{instance}", e =>
        {
            e.ConfigureConsumer<CollaboratorConsumer>(context);
            e.ConfigureConsumer<CollaboratorUpdatedConsumer>(context);
            e.ConfigureConsumer<UserCreatedConsumer>(context);
            e.ConfigureConsumer<UserUpdatedConsumer>(context);
        });
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
