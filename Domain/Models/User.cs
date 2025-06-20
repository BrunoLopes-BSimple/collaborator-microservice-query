using Domain.Interfaces;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace Domain.Models;

public class User : IUser
{
    public Guid Id { get; private set; }

    public User(Guid id)
    {
        Id = id;
    }
}