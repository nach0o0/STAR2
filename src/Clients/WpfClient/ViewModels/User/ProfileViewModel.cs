using CommunityToolkit.Mvvm.Messaging;
using WpfClient.Messages;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.User
{
    public partial class ProfileViewModel : ViewModelBase, IRecipient<StatusUpdateMessage>
    {
        public ProfileInfoViewModel ProfileInfoViewModel { get; }
        public ChangePasswordViewModel ChangePasswordViewModel { get; }
        public ActiveSessionsViewModel ActiveSessionsViewModel { get; }
        public DeleteAccountViewModel DeleteAccountViewModel { get; }

        public ProfileViewModel(
            IMessenger messenger,
            ProfileInfoViewModel profileInfoViewModel,
            ChangePasswordViewModel changePasswordViewModel,
            ActiveSessionsViewModel activeSessionsViewModel,
            DeleteAccountViewModel deleteAccountViewModel)
        {
            ProfileInfoViewModel = profileInfoViewModel;
            ChangePasswordViewModel = changePasswordViewModel;
            ActiveSessionsViewModel = activeSessionsViewModel;
            DeleteAccountViewModel = deleteAccountViewModel;

            messenger.Register<StatusUpdateMessage>(this);
        }

        public void Receive(StatusUpdateMessage message)
        {
            // Setzt zuerst alle Nachrichten zurück, um alte Anzeigen zu löschen.
            SuccessMessage = null;
            ErrorMessage = null;

            if (message.MessageType == StatusMessageType.Success)
            {
                SuccessMessage = message.Message;
            }
            else
            {
                ErrorMessage = message.Message;
            }
        }
    }
}
