using System.Threading.Tasks;
using Discount.Grpc.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Basket.API.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient client, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CouponModel> GetDiscountAsync(string productName)
        {
            var accessToken = ExtractTokenValueFromBearerToken(_httpContextAccessor.HttpContext.Request.Headers["Authorization"]);

            var callCredentials = CallCredentials.FromInterceptor((context, metadata) =>
                                                                    {
                                                                        metadata.Add("Authorization",
                                                                                      $"Bearer {accessToken}");
                                                                                      return Task.CompletedTask;
                                                                    });

            var channelOptions = new GrpcChannelOptions();
            channelOptions.Credentials = ChannelCredentials.Create(new SslCredentials(),callCredentials);

            var channel = GrpcChannel.ForAddress("https://localhost:5030", channelOptions);

            var client = new DiscountProtoService.DiscountProtoServiceClient(channel);

            return await client.GetDiscountAsync(new GetDiscountRequest { ProductName = productName });
        }

        private string ExtractTokenValueFromBearerToken(string bearerToken)
        {
            return bearerToken.Substring(7);
        }

    }
}