using MassTransit;
using MassTransit.Courier;
using Microsoft.Extensions.Logging;
using Shared.Contract.Constants;
using Shared.Contract.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Courier.Activities
{
    public class TakeProductActivity : IActivity<TakeProductArgument, TakeProductLog>
    {
        private readonly ILogger<TakeProductActivity> logger;
        private readonly IRequestClient<TakeProductTransactionMessage> requestClient;

        public TakeProductActivity(ILogger<TakeProductActivity> logger,IRequestClient<TakeProductTransactionMessage> requestClient)
        {
            this.logger = logger;
            this.requestClient = requestClient;
        }
        public async Task<CompensationResult> Compensate(CompensateContext<TakeProductLog> context)
        {
            
            logger.LogInformation($"Compensate Take Product Courier called for order {context.Log.OrderId}");
            var uri = QueueNames.GetMessageUri(nameof(ReturnProductTransactionMessage));
            var sendEndpoint = await context.GetSendEndpoint(uri);
            await sendEndpoint.Send<ReturnProductTransactionMessage>(new
            {
                ProductBaskets = context.Log.Baskets
            });
            return context.Compensated();
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<TakeProductArgument> context)
        {
            logger.LogInformation($"Take Product Courier called for order {context.Arguments.OrderId}");            
            var uri = QueueNames.GetMessageUri(nameof(TakeProductTransactionMessage));
            var sendEndpoint = await context.GetSendEndpoint(uri);
            //await sendEndpoint.Send<TakeProductTransactionMessage>(new
            //{
            //    ProductBaskets = context.Arguments.Baskets                
            //});
             await requestClient.GetResponse<IRequestResult>(new { ProductBaskets = context.Arguments.Baskets });
            return context.Completed(new { Baskets = context.Arguments.Baskets, OrderId=context.Arguments.OrderId });
        }
    }
    public interface TakeProductArgument
    {
        int OrderId { get; }
        IList<ProductBasket> Baskets { get; }
    }
    public interface TakeProductLog
    {
        int OrderId { get; }
        IList<ProductBasket> Baskets { get; }
    }
}
