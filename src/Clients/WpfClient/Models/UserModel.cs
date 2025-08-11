using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Models
{
    public partial class UserModel : ObservableObject
    {
        [ObservableProperty]
        private Guid _id;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string? _firstName;

        [ObservableProperty]
        private string? _lastName;

        /// <summary>
        /// Eine berechnete Eigenschaft, die den vollständigen Namen für die Anzeige bereitstellt.
        /// </summary>
        public string DisplayName =>
            !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName)
                ? $"{FirstName} {LastName}"
                : Email;
    }
}
