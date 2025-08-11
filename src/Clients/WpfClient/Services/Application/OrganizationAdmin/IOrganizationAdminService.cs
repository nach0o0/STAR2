using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;

namespace WpfClient.Services.Application.OrganizationAdmin
{
    public interface IOrganizationAdminService
    {
        Task<OrganizationModel?> CreateOrganizationAsync(string name, string abbreviation, Guid? parentId);
    }
}
