using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Dtos;
using OrderApi.Model;
using OrderApi.Services;
using Shared.Contract.Events;
using Shared.Contract.Messages;
using System;
using System.Threading.Tasks;

namespace OrderApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
       
        private readonly IRequestClient<ChangeCustomerCreditMessage> changeCustomerCreditClient;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IOrderService orderService;

        public OrderController(IRequestClient<ChangeCustomerCreditMessage> changeCustomerCreditClient,
                               IPublishEndpoint publishEndpoint,
                               IOrderService orderService)
        {            
            this.changeCustomerCreditClient = changeCustomerCreditClient;
            this.publishEndpoint = publishEndpoint;
            this.orderService = orderService;
        }
        [HttpPost("AddCredit")]
        public async Task<string> AddCustomerCredit([FromBody]AddCreditDto addCreditDto )
        {
            var (accepted,rejected) = await  changeCustomerCreditClient.GetResponse<ChangeCustomerCreditResponse, InputValueRejectedResponse>(new {
                CostomerId = addCreditDto.CustomerId,
                Credit = addCreditDto.Credit
            });
            if (accepted.IsCompletedSuccessfully)
            {
                var result = await accepted;
                return $"Customer  {addCreditDto.CustomerId} New Credit is  {result.Message.TotalCredit}";
            }
            else
            {
                var result = await rejected;
                return result.Message.Reason;
            }
            
        }
        [HttpPost("AddOrder")]
        public  Task<int> CreateOrder([FromBody]CreateOrderDto createOrderDto)
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
            await publishEndpoint.Publish<OrderItemAddedEvent>(new
            {

                ProductId = addOrderItemDto.ProductId,
                Count = addOrderItemDto.Count
            });
            return orderItemId;
        }       
        public async Task<string> Get()
        {
            return "order v1";
        }
    }
}
