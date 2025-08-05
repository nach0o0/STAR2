using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Services.Application.Auth;

namespace WpfClient.Services.Api
{
    public interface IAccessTokenProvider
    {
        string? GetAccessToken();
    }

    public class AccessTokenProvider : IAccessTokenProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public AccessTokenProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public string? GetAccessToken()
        {
            var authService = _serviceProvider.GetService<IAuthService>();
            return authService?.GetAccessToken();
        }
    }
}
