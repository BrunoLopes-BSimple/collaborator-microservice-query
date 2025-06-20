using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.Messaging
{
    public record CollaboratorCreatedEvent(Guid Id, Guid UserId, PeriodDateTime PeriodDateTime);
}