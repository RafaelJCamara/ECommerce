using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;

    public DeleteOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var storedOrder = await _orderRepository.GetByIdAsync(request.Id);
        if (storedOrder == null) throw new NotFoundException(nameof(Order), request.Id);
        await _orderRepository.DeleteAsync(storedOrder);
        return Unit.Value;
    }
}