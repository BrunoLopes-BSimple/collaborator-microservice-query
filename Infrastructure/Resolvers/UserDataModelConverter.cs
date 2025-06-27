using AutoMapper;
using Domain.Factory;
using Domain.Models;
using Infrastructure.DataModel;

namespace Infrastructure.Resolvers;

public class UserDataModelConverter : ITypeConverter<UserDataModel, User>
{
    private readonly IUserFactory _UserFactory;

    public UserDataModelConverter(IUserFactory UserFactory)
    {
        _UserFactory = UserFactory;
    }

    public User Convert(UserDataModel source, User destination, ResolutionContext context)
    {
        return _UserFactory.Create(source);
    }

    public bool UpdateDataModel(UserDataModel userDataModel, User userDomain)

    {
        userDataModel.Id = userDomain.Id;
        userDataModel.PeriodDateTime._initDate = userDomain.PeriodDateTime._initDate;
        userDataModel.PeriodDateTime._finalDate = userDomain.PeriodDateTime._finalDate;
        return true;
    }
}