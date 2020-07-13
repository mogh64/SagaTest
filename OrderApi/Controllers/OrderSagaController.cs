using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Dtos;
using OrderApi.Model;
using OrderApi.Services;
using Shared.Contract.Events;
using System;
using System.Threading.Tasks;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderSagaController : ControllerBase
    {
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IOrderService orderService;
        public OrderSagaController(IPublishEndpoint publishEndpoint,
                               IOrderService orderService)
        {
            this.publishEndpoint = publishEndpoint;
            this.orderService = orderService;
        }
        [HttpPost("AddOrder")]
        public Task<int> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            return orderService.CreateOrder(new Order()
            {
                CustomerId = createOrderDto.CustomerId,
                OrderDate = createOrderDto.OrderDate
            });
        }
        [HttpPost("AddOrderItem")]
        public async Task<int> AddOrderItem([FromBody] AddOrderItemDto addOrderItemDto)
        {
            var orderItemId = await orderService.AddOrderItem(new OrderItem()
            {
                OrderId = addOrderItemDto.OrderId,
                ProductId = addOrderItemDto.ProductId,
                Count = addOrderItemDto.Count
            });
            return orderItemId;
        }
        [HttpPost("SubmitOrder")]
        public async Task SubmitOrder([FromBody] SubmitOrderDto submitOrderDto)
        {
            var order = await orderService.SubmitOrder(submitOrderDto.OrderId);

            await publishEndpoint.Publish<OrderSubmittedEvent>(new
            {
                Credit = submitOrderDto.Credit,
                CustomerId = order.CustomerId,
                OrderId = submitOrderDto.OrderId,
                CorrelationId = Guid.NewGuid()
            });
        }
    }
}
