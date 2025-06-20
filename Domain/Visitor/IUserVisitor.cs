using Domain.Models;

namespace Domain.Visitor;

public interface IUserVisitor
{
    Guid Id { get; }
}
