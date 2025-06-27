using Domain.Models;

namespace Domain.Interfaces;

public interface IUser
{
    public Guid Id { get; }
    public string Names { get; }
    public string Surnames { get; }
    public string Email { get; }
    public PeriodDateTime PeriodDateTime { get; }
}