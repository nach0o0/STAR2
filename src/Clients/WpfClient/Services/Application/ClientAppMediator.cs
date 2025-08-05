using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Services.Application
{
    public static class ClientAppMediator
    {
        public static event Func<Task>? ProfileCreated;

        public static async Task NotifyProfileCreatedAsync()
        {
            if (ProfileCreated is not null)
            {
                await ProfileCreated.Invoke();
            }
        }

        public static event Func<Task>? UserLoggedIn;
        public static async Task NotifyUserLoggedInAsync()
        {
            if (UserLoggedIn is not null) await UserLoggedIn.Invoke();
        }

        public static event Action? UserLoggedOut;
        public static void NotifyUserLoggedOut()
        {
            UserLoggedOut?.Invoke();
        }
    }
}
