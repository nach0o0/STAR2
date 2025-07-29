using Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Interfaces.Infrastructure
{
    public interface IBasicTokenService
    {
        string GenerateToken(User user);
    }
}
