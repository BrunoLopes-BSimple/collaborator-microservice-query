using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.DTO.Collaborators
{
    public class CreateCollabDTO
    {
        public Guid UserId { get; set; }
        public PeriodDateTime PeriodDateTime { get; set; }
    }
}