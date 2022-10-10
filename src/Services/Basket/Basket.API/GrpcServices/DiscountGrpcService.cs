using System.Threading.Tasks;
using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices;

public class DiscountGrpcService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _client;

    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient client)
    {
        _client = client;
    }

    public async Task<CouponModel> GetDiscountAsync(string productName)
    {
        return await _client.GetDiscountAsync(new GetDiscountRequest { ProductName = productName });
    }
}