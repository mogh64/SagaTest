using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using Microsoft.Extensions.Logging;
using OrderApi.Courier.Activities;
using OrderApi.Infrastructure;
using Shared.Contract.Constants;
using Shared.Contract.Events;
using Shared.Contract.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Courier.Consumers
{
    public class FullfillOrderConsumer : IConsumer<FullfillOrderMessage>
    {
        private readonly ILogger<FullfillOrderConsumer> logger;
        private readonly OrderDbContext dbContext;

        public FullfillOrderConsumer(ILogger<FullfillOrderConsumer> logger,OrderDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }
        public async Task Consume(ConsumeContext<FullfillOrderMessage> context)
        {
            var Baskets = dbContext.OrderItems.Where(x => x.OrderId == context.Message.OrderId)
                .Select(p=>new {ProductId= p.ProductId,Count = p.Count }).ToList();
            logger.LogInformation($"Fullfilled order {context.Message.OrderId}");
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var submitOrderUrl = QueueNames.GetActivityUri(nameof(SubmitOrderActivity));
            builder.AddActivity("SubmitOrder", submitOrderUrl, new
            {
                context.Message.OrderId
            });;
            builder.AddActivity("Payment", QueueNames.GetActivityUri(nameof(PaymentActivity)), new {
                context.Message.OrderId,
                context.Message.CustomerId,
                context.Message.Credit
            });
           
            builder.AddActivity("TakeProduct", QueueNames.GetActivityUri(nameof(TakeProductActivity)), new
            {
                context.Message.OrderId,
                Baskets
            });
            builder.AddVariable("OrderId", context.Message.OrderId);
            await builder.AddSubscription(context.SourceAddress,
                RoutingSlipEvents.Faulted | RoutingSlipEvents.Supplemental,
                RoutingSlipEventContents.None, x => x.Send<OrderFulfillFaulted>(new { context.Message.OrderId }));

            await builder.AddSubscription(context.SourceAddress,
                RoutingSlipEvents.Completed | RoutingSlipEvents.Supplemental,
                RoutingSlipEventContents.None, x => x.Send<OrderFullfillCompleted>(new { context.Message.OrderId }));

            var routingSlip = builder.Build();
            await context.Execute(routingSlip).ConfigureAwait(false);

        }
    }
}
