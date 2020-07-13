using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Dtos;
using OrderApi.Services;
using Shared.Contract.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderTransactionController : ControllerBase
    {
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IOrderService orderService;

        public OrderTransactionController(IPublishEndpoint publishEndpoint,
                                           IOrderService orderService)
        {
            this.publishEndpoint = publishEndpoint;
            this.orderService = orderService;
        }
        [HttpPost("SubmitOrder")]
        public async Task SubmitOrder([FromBody] SubmitOrderDto submitOrderDto)
        {
            var order = await orderService.SubmitOrder(submitOrderDto.OrderId);

            await publishEndpoint.Publish<OrderTransactionSubmittedEvent>(new
            {
                Credit = submitOrderDto.Credit,
                CustomerId = order.CustomerId,
                OrderId = submitOrderDto.OrderId,
                CorrelationId = Guid.NewGuid()
            });
        }
    }
}
