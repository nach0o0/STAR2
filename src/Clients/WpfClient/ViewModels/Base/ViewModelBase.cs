using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Net.Http;
using WpfClient.Messages;
using WpfClient.Models;
using WpfClient.Services.Application.Notification;

namespace WpfClient.ViewModels.Base
{
    public abstract partial class ViewModelBase : ObservableObject
    {
        [ObservableProperty]
        private bool _isLoading;

        protected ViewModelBase() 
        {
        }

        protected async Task ExecuteCommandAsync(Func<Task> action)
        {
            IsLoading = true;
            try
            {
                await action();
            }
            catch (ApiException ex)
            {
                var notificationService = App.Services.GetRequiredService<INotificationService>();
                string errorMessage = await GetErrorMessageFromApiException(ex);
                notificationService.ShowMessage(errorMessage, StatusMessageType.Error);
            }
            catch (HttpRequestException)
            {
                var notificationService = App.Services.GetRequiredService<INotificationService>();
                notificationService.ShowMessage("Could not connect to the server.", StatusMessageType.Error);
            }
            catch (InvalidOperationException ex)
            {
                var notificationService = App.Services.GetRequiredService<INotificationService>();
                notificationService.ShowMessage(ex.Message, StatusMessageType.Error);
            }
            catch (Exception ex)
            {
                var notificationService = App.Services.GetRequiredService<INotificationService>();
                notificationService.ShowMessage($"An unexpected error occurred: {ex.Message}", StatusMessageType.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task<string> GetErrorMessageFromApiException(ApiException ex)
        {
            // Spezieller Fall für Login: 404 bedeutet "nicht gefunden"
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return "Invalid email or password.";
            }

            // Versuche, eine detailliertere Validierungsfehlermeldung vom Server zu lesen
            var validationErrors = await ex.GetContentAsAsync<ValidationProblemDetails>();
            if (validationErrors?.Errors != null && validationErrors.Errors.Any())
            {
                // Gib die erste konkrete Fehlermeldung zurück
                return validationErrors.Errors.First().Value.First();
            }

            // Fallback für alle anderen API-Fehler
            return $"An API error occurred: {ex.StatusCode}";
        }
    }
}
