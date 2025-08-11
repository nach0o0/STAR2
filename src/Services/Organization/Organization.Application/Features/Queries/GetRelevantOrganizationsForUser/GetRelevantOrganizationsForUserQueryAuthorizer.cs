using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetRelevantOrganizationsForUser
{
    public class GetRelevantOrganizationsForUserQueryAuthorizer : ICommandAuthorizer<GetRelevantOrganizationsForUserQuery>
    {
        public Task AuthorizeAsync(GetRelevantOrganizationsForUserQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // Jeder authentifizierte Benutzer darf diese Aktion ausführen.
            // Die Logik im Handler stellt sicher, dass er nur sieht, was er sehen darf.
            return Task.CompletedTask;
        }
    }
}
