using CommunityToolkit.Mvvm.Messaging;
using WpfClient.Messages;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.User
{
    public partial class ProfileViewModel : ViewModelBase
    {
        public ProfileInfoViewModel ProfileInfoViewModel { get; }
        public ChangePasswordViewModel ChangePasswordViewModel { get; }
        public ActiveSessionsViewModel ActiveSessionsViewModel { get; }
        public DeleteAccountViewModel DeleteAccountViewModel { get; }

        public ProfileViewModel(
            ProfileInfoViewModel profileInfoViewModel,
            ChangePasswordViewModel changePasswordViewModel,
            ActiveSessionsViewModel activeSessionsViewModel,
            DeleteAccountViewModel deleteAccountViewModel)
        {
            ProfileInfoViewModel = profileInfoViewModel;
            ChangePasswordViewModel = changePasswordViewModel;
            ActiveSessionsViewModel = activeSessionsViewModel;
            DeleteAccountViewModel = deleteAccountViewModel;
        }
    }
}
