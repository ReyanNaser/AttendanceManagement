using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Messages
{
    public record UserCreatedEvent
    (
        string FirstName,
        string LastName,
        string Email,
        string Role
    );
}
