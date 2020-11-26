using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using AuthBase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AuthBase
{
    public class AuthHttpClientHandler : HttpClientHandler
    {
        private const string AuthHeaderKey = "Authorization";
        private const string Bearer = "Bearer";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApiSettings _apiSettings;

        public AuthHttpClientHandler(
            IHttpContextAccessor httpContextAccessor,
            IOptions<ApiSettings> apiSettings)
        {
            ServerCertificateCustomValidationCallback = (
                sender, 
                cert, 
                chain, 
                sslPolicyErrors) => true;
            CheckCertificateRevocationList = false;

            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentException(nameof(httpContextAccessor));
            _apiSettings = apiSettings.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage,
            CancellationToken cancellationToken)
        {
            if (requestMessage.Headers.Authorization == null)
            {
                AddAuthHeader(requestMessage);
            }

            return await base.SendAsync(requestMessage, cancellationToken);
        }

        private void AddAuthHeader(HttpRequestMessage requestMessage)
        {
            if (_httpContextAccessor.HttpContext != null
                && _httpContextAccessor.HttpContext.Request.Headers.TryGetValue(AuthHeaderKey, out var value))
            {
                var token = value.ToString().Split(" ").LastOrDefault();
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(Bearer, token);
            }
            else if (string.IsNullOrWhiteSpace(_apiSettings.Token))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(Bearer, _apiSettings.Token);
            }
        }
    }
}