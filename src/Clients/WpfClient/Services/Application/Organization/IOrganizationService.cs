using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;

namespace WpfClient.Services.Application.Organization
{
    public interface IOrganizationService
    {
        Task<bool> CreateOrganizationAsync(string name, string abbreviation);
        Task<List<EmployeeModel>> GetEmployeesForMyOrganizationAsync();
    }
}
