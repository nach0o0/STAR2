using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Contracts.Responses
{
    public record SessionTokensResponse(
        string AccessToken,
        string RefreshToken
    );
}
