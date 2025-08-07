using CommunityToolkit.Mvvm.ComponentModel;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;

namespace WpfClient.ViewModels.Base
{
    public abstract partial class ViewModelBase : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasErrorMessage))]
        private string? _errorMessage;

        [ObservableProperty]
        private string? _successMessage;

        [ObservableProperty]
        private bool _isLoading;

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        protected async Task ExecuteCommandAsync(Func<Task> action)
        {
            IsLoading = true;
            ErrorMessage = null;
            try
            {
                await action();
            }
            catch (ApiException ex)
            {
                var validationErrors = await ex.GetContentAsAsync<ValidationProblemDetails>();
                if (validationErrors?.Errors != null && validationErrors.Errors.Any())
                {
                    ErrorMessage = validationErrors.Errors.First().Value.First();
                }
                else
                {
                    ErrorMessage = $"An API error occurred: {ex.StatusCode}";
                }
            }
            catch (HttpRequestException)
            {
                ErrorMessage = "Could not connect to the server.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An unexpected error occurred: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
