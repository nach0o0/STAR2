using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WpfClient.Services.Api.Extensions;
using WpfClient.Services.Api.Handlers;
using WpfClient.Services.Api.Interfaces;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.MyEmployeeProfile;
using WpfClient.Services.Application.Navigation;
using WpfClient.Services.Application.UserState;
using WpfClient.ViewModels;
using WpfClient.ViewModels.Authentication;
using WpfClient.ViewModels.Shell;
using WpfClient.ViewModels.User;
using WpfClient.Views.Shell;

namespace WpfClient
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }

        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
            Services = _serviceProvider;
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<TimeTrackingViewModel>();
            services.AddTransient<CostObjectViewModel>();
            services.AddTransient<PlanningViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<ProfileViewModel>();
            services.AddTransient<ProfileInfoViewModel>();
            services.AddTransient<ChangePasswordViewModel>();
            services.AddTransient<ActiveSessionsViewModel>();
            services.AddTransient<DeleteAccountViewModel>();

            // Views
            services.AddSingleton<MainWindow>();

            // --- Application Services ---

            // Zentrales State Management (Singleton)
            services.AddSingleton<IUserStateService, UserStateService>();

            // Reine "Action" Services (Singleton, da sie selbst keinen Zustand halten)
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IMyEmployeeProfileService, MyEmployeeProfileService>();

            // Hilfsdienste
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

            // --- API Clients & Infrastruktur ---

            // HTTP Middleware für Authentifizierung
            services.AddTransient<AuthDelegatingHandler>();

            // Refit API Clients
            string baseAddress = "http://localhost:5058"; // API Gateway URL
            services.AddStarApiClient<IAuthApiClient>(baseAddress);
            services.AddStarApiClient<ISessionApiClient>(baseAddress);
            services.AddStarApiClient<IPermissionApiClient>(baseAddress);
            services.AddStarApiClient<IOrganizationApiClient>(baseAddress);
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = _serviceProvider.GetRequiredService<MainViewModel>();

            var authService = _serviceProvider.GetRequiredService<IAuthService>();
            await authService.TryInitializeSessionAsync();

            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
