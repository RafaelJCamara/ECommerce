using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize("ClientIdPolicy")]
    public class OrderController : ControllerBase
    {

        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userName}", Name = "GetOrder")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersByUserName(string userName)
        {
            var ordersQuery = new GetOrdersListQuery(userName);
            var orders = await _mediator.Send(ordersQuery);
            return Ok(orders);
        }

        [HttpPost(Name = "CheckoutOrder")]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand order)
        {
            var createdOrderId = await _mediator.Send(order);
            return Ok(createdOrderId);
        }

        [HttpPut(Name = "UpdateOrder")]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderCommand order)
        {
            await _mediator.Send(order);
            return NoContent();
        }

        [HttpDelete("{id}",Name ="DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var deleteCommand = new DeleteOrderCommand { Id = orderId };
            await _mediator.Send(deleteCommand);
            return NoContent();
        }

    }
}

