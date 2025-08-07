using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;

namespace WpfClient.Services.Application.UserState
{
    public interface IUserStateService : INotifyPropertyChanged
    {
        bool IsLoggedIn { get; }
        bool HasProfile { get; }
        CurrentUser? CurrentUser { get; }
        MyEmployeeProfileModel? Profile { get; }

        Task RefreshProfileAsync();
    }
}
