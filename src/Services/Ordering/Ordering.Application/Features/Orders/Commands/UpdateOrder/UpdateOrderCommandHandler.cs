using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var storedOrder = await _orderRepository.GetByIdAsync(request.Id);
        if (storedOrder == null) throw new NotFoundException(nameof(Order), request.Id);
        _mapper.Map(request, storedOrder, typeof(UpdateOrderCommand), typeof(Order));
        await _orderRepository.UpdateAsync(storedOrder);
        return Unit.Value;
    }
}