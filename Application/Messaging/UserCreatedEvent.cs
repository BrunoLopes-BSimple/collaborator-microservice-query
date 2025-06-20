using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Messaging
{
    public record UserCreatedEvent
    {
        public Guid Id { get; init; }

        public UserCreatedEvent(Guid id)
        {
            Id = id;
        }
    }
}