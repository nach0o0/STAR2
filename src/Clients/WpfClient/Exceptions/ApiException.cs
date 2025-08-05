using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public Dictionary<string, string[]>? ValidationErrors { get; }

        public ApiException(string message, int statusCode, Dictionary<string, string[]>? validationErrors = null)
            : base(message)
        {
            StatusCode = statusCode;
            ValidationErrors = validationErrors;
        }
    }
}
