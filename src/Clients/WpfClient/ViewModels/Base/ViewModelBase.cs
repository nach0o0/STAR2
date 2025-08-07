using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Net.Http;
using WpfClient.Messages;
using WpfClient.Models;

namespace WpfClient.ViewModels.Base
{
    public abstract partial class ViewModelBase : ObservableObject
    {
        protected readonly IMessenger Messenger;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasErrorMessage))]
        private string? _errorMessage;

        [ObservableProperty]
        private string? _successMessage;

        [ObservableProperty]
        private bool _isLoading;

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        protected ViewModelBase() 
        {
            Messenger = App.Services.GetRequiredService<IMessenger>();
        }

        protected async Task ExecuteCommandAsync(Func<Task> action)
        {
            IsLoading = true;
            ErrorMessage = null;
            SuccessMessage = null;
            try
            {
                await action();
            }
            catch (ApiException ex)
            {
                var validationErrors = await ex.GetContentAsAsync<ValidationProblemDetails>();
                var message = (validationErrors?.Errors != null && validationErrors.Errors.Any())
                    ? validationErrors.Errors.First().Value.First()
                    : $"An API error occurred: {ex.StatusCode}";

                // Sende eine Fehlermeldung über den Messenger
                Messenger.Send(new StatusUpdateMessage(message, StatusMessageType.Error));
            }
            catch (InvalidOperationException ex)
            {
                Messenger.Send(new StatusUpdateMessage($"{ex.Message}", StatusMessageType.Error));
            }
            catch (HttpRequestException)
            {
                Messenger.Send(new StatusUpdateMessage("Could not connect to the server.", StatusMessageType.Error));
            }
            catch (Exception ex)
            {
                Messenger.Send(new StatusUpdateMessage($"An unexpected error occurred: {ex.Message}", StatusMessageType.Error));
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
