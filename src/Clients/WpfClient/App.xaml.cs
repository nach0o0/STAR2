using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Net.Http;
using System.Windows;
using WpfClient.Services.Api.Handlers;
using WpfClient.Services.Api.Interfaces;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.MyEmployeeProfile;
using WpfClient.Services.Application.Navigation;
using WpfClient.Services.Application.Notification;
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
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
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
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

            // --- API Clients & Infrastruktur ---

            // HTTP Middleware für Authentifizierung
            services.AddTransient<AuthDelegatingHandler>();

            // Refit API Clients
            string baseAddress = "http://localhost:5058"; // API Gateway URL
            Action<HttpClient> configureClient = client =>
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.UserAgent.ParseAdd("STAR-WpfClient");
            };

            services.AddRefitClient<IAuthApiClient>()
                .ConfigureHttpClient(configureClient)
                .AddHttpMessageHandler<AuthDelegatingHandler>();

            services.AddRefitClient<ISessionApiClient>()
                .ConfigureHttpClient(configureClient)
                .AddHttpMessageHandler<AuthDelegatingHandler>();

            services.AddRefitClient<IPermissionApiClient>()
                .ConfigureHttpClient(configureClient)
                .AddHttpMessageHandler<AuthDelegatingHandler>();

            services.AddRefitClient<IOrganizationApiClient>()
                .ConfigureHttpClient(configureClient)
                .AddHttpMessageHandler<AuthDelegatingHandler>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var authService = _serviceProvider.GetRequiredService<IAuthService>();
            await authService.TryInitializeSessionAsync();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = _serviceProvider.GetRequiredService<MainViewModel>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
