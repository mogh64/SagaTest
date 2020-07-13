using MassTransit.Courier;
using Microsoft.Extensions.Logging;
using OrderApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Courier.Activities
{
    public class SubmitOrderActivity : IActivity<SubmitOrderArgument, SubmitOrderLog>
    {
        private readonly IOrderService orderService;
        private readonly ILogger<SubmitOrderActivity> logger;

       
        public SubmitOrderActivity(IOrderService orderService, ILogger<SubmitOrderActivity> logger)
        {
            this.orderService = orderService;
            this.logger = logger;
        }
        public async Task<CompensationResult> Compensate(CompensateContext<SubmitOrderLog> context)
        {
            logger.LogInformation($"Submit Order Courier compensated called for order {context.Log.OrderId}");
            await orderService.ReturnOrder(context.Log.OrderId);
            return context.Compensated();
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<SubmitOrderArgument> context)
        {
            logger.LogInformation($"Submit Order Courier called for order {context.Arguments.OrderId}");
            var order =  await orderService.SubmitOrder(context.Arguments.OrderId);
            return context.Completed(new { OrderId = order.Id });
        }
    }
    public interface SubmitOrderArgument
    {
        int OrderId { get; }        
    }
    public interface SubmitOrderLog
    {
        int OrderId { get; }
    }
}
