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
            var notificationService = App.Services.GetRequiredService<INotificationService>();

            IsLoading = true;
            try
            {
                await action();
            }
            catch (ApiException ex)
            {
                string errorMessage = await GetErrorMessageFromApiException(ex);
                notificationService.ShowMessage(errorMessage, StatusMessageType.Error);
            }
            catch (HttpRequestException)
            {
                notificationService.ShowMessage("Could not connect to the server.", StatusMessageType.Error);
            }
            catch (InvalidOperationException ex)
            {
                notificationService.ShowMessage(ex.Message, StatusMessageType.Error);
            }
            catch (Exception ex)
            {
                notificationService.ShowMessage($"An unexpected error occurred: {ex.Message}", StatusMessageType.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task<string> GetErrorMessageFromApiException(ApiException ex)
        {
            var validationErrors = await ex.GetContentAsAsync<ValidationProblemDetails>();
            if (validationErrors?.Errors != null && validationErrors.Errors.Any())
            {
                return validationErrors.Errors.First().Value.First();
            }

            // Spezifische, verständliche Nachrichten für häufige Statuscodes.
            return ex.StatusCode switch
            {
                System.Net.HttpStatusCode.NotFound => "The requested resource was not found.",
                System.Net.HttpStatusCode.Forbidden => "You are not authorized to perform this action.",
                System.Net.HttpStatusCode.Unauthorized => "Authentication failed. Please log in again.",
                _ => $"An API error occurred: {ex.StatusCode}" // Fallback
            };
        }
    }
}
