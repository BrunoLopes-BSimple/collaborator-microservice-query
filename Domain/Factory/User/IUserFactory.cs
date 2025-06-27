using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Domain.Factory;

public interface IUserFactory
{
    public User Create(IUserVisitor userVisitor);
}