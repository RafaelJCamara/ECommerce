using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Shopping.Aggregator.HttpHandlers
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = ExtractTokenValueFromBearerToken(_httpContextAccessor.HttpContext.Request.Headers["Authorization"]);
                
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.SetBearerToken(token);
            }             

            return await base.SendAsync(request, cancellationToken);
        }

        private string ExtractTokenValueFromBearerToken(string bearerToken)
        {
            return bearerToken.Substring(7);
        }

    }
}
