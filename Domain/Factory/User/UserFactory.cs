using System.Text.RegularExpressions;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Domain.Visitor;

namespace Domain.Factory;

public class UserFactory : IUserFactory
{
    public UserFactory()
    {}

    public User Create(IUserVisitor userVisitor)
    {
        return new User(userVisitor.Id, userVisitor.Names, userVisitor.Surnames, userVisitor.Email, userVisitor.PeriodDateTime);
    }
}