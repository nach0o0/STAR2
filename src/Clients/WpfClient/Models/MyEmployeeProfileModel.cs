using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Models
{
    public partial class MyEmployeeProfileModel : ObservableObject
    {
        [ObservableProperty]
        private Guid _employeeId;

        [ObservableProperty]
        private string _firstName;

        [ObservableProperty]
        private string _lastName;
    }
}
