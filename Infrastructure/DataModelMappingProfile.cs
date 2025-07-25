﻿using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Resolvers;

namespace Infrastructure;

public class DataModelMappingProfile : Profile
{
    public DataModelMappingProfile()
    {
        CreateMap<User, UserDataModel>();
        CreateMap<UserDataModel, User>().ConvertUsing<UserDataModelConverter>();

        CreateMap<Collaborator, CollaboratorDataModel>();
        CreateMap<CollaboratorDataModel, Collaborator>().ConvertUsing<CollaboratorDataModelConverter>();
    }

}