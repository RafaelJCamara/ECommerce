using System.Threading.Tasks;
using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IMapper _mapper;

    private readonly IDiscountRepository _repository;

    public DiscountService(IDiscountRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _repository.GetDiscount(request.ProductName);
        var mappedCoupon = _mapper.Map<CouponModel>(coupon);
        return mappedCoupon;
    }

    public override async Task<CreateDiscountResponse> CreateDiscount(CreateDiscountRequest request,
        ServerCallContext context)
    {
        var mappedCoupon = _mapper.Map<Coupon>(request.Coupon);
        var isSuccessInCreation = await _repository.CreateDiscount(mappedCoupon);
        return new CreateDiscountResponse { Success = isSuccessInCreation };
    }

    public override async Task<UpdateDiscountResponse> UpdateDiscount(UpdateDiscountRequest request,
        ServerCallContext context)
    {
        var mappedCoupon = _mapper.Map<Coupon>(request.Coupon);
        var isSuccessInUpdate = await _repository.UpdateDiscount(mappedCoupon);
        return new UpdateDiscountResponse { Success = isSuccessInUpdate };
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request,
        ServerCallContext context)
    {
        var isSuccessInDeletion = await _repository.DeleteDiscount(request.ProductName);
        return new DeleteDiscountResponse { Success = isSuccessInDeletion };
    }
}