using Microsoft.Extensions.DependencyInjection;
using WpfClient.ViewModels.Base;
using WpfClient.ViewModels.Shell;

namespace WpfClient.Services.Application.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<TViewModel>();
        }
    }
}
