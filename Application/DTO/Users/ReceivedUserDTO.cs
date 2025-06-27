using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.DTO.Users
{

    public record ReceivedUserDTO
    {
        public Guid Id { get; set; }
        public string Names { get; set; }
        public string Surnames { get; set; }
        public string Email { get; set; }
        public PeriodDateTime PeriodDateTime { get; set; }

        public ReceivedUserDTO(Guid id, string names, string surnames, string email, PeriodDateTime periodDateTime)
        {
            Id = id;
            Names = names;
            Surnames = surnames;
            Email = email;
            PeriodDateTime = periodDateTime;
        }
    }
    
}