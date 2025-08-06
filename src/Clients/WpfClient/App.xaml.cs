using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Windows;
using WpfClient.Services.Api;
using WpfClient.Services.Api.AuthApi;
using WpfClient.Services.Api.Interfaces;
using WpfClient.Services.Api.OrganizationApi;
using WpfClient.Services.Api.PermissionApi;
using WpfClient.Services.Api.SessionApi;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.MyEmployeeProfile;
using WpfClient.Services.Application.Navigation;
using WpfClient.Services.Application.Notification;
using WpfClient.ViewModels;

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

            // Views
            services.AddSingleton<MainWindow>();

            // Application Services
            services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IMyEmployeeProfileService, MyEmployeeProfileService>();

            services.AddSingleton<IAccessTokenProvider, AccessTokenProvider>();
            services.AddTransient<AuthDelegatingHandler>();

            // API Clients
            services.AddRefitClient<IAuthApiClient>()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri("http://localhost:5058"))
                .AddHttpMessageHandler<AuthDelegatingHandler>();

            services.AddRefitClient<ISessionApiClient>()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri("http://localhost:5058"))
                .AddHttpMessageHandler<AuthDelegatingHandler>();

            services.AddRefitClient<IPermissionApiClient>()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri("http://localhost:5058"))
                .AddHttpMessageHandler<AuthDelegatingHandler>();

            services.AddRefitClient<IOrganizationApiClient>()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri("http://localhost:5058"))
                .AddHttpMessageHandler<AuthDelegatingHandler>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var authService = _serviceProvider.GetRequiredService<IAuthService>();
            await authService.InitializeAsync();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = _serviceProvider.GetRequiredService<MainViewModel>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
