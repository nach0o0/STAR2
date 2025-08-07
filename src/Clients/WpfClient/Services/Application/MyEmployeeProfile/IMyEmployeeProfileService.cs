using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;

namespace WpfClient.Services.Application.MyEmployeeProfile
{
    public interface IMyEmployeeProfileService
    {
        Task CreateMyProfileAsync(string firstName, string lastName);
        Task UpdateMyProfileAsync(string firstName, string lastName);
    }
}
