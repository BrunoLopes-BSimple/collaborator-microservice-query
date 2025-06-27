using Domain.Interfaces;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace Domain.Models;

public class User : IUser
{
    public Guid Id { get; private set; }
    public string Names { get; private set; }
    public string Surnames { get; private set; }
    public string Email { get; private set; }
    public PeriodDateTime PeriodDateTime { get; private set; }

    public User(Guid id, string names, string surnames, string email, PeriodDateTime periodDateTime)
    {
        Id = id;
        Names = names;
        Surnames = surnames;
        Email = email;
        PeriodDateTime = periodDateTime;
    }
}