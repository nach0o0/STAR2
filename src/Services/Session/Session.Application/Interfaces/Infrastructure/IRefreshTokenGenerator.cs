using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Application.Interfaces.Infrastructure
{
    public interface IRefreshTokenGenerator
    {
        string GenerateRefreshToken();
    }
}
