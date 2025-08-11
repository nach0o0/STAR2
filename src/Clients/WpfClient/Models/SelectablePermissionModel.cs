using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Models
{
    public partial class SelectablePermissionModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isSelected;

        public PermissionModel Permission { get; }

        public SelectablePermissionModel(PermissionModel permission, bool isSelected)
        {
            Permission = permission;
            _isSelected = isSelected;
        }
    }
}
