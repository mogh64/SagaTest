using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contract.Events;
using Shared.Contract.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Integrations.Consumers
{
    public class TakeProductConsumer : IConsumer<TakeProductMessage>
    {
        private readonly ILogger<TakeProductConsumer> logger;

        public TakeProductConsumer(ILogger<TakeProductConsumer> logger)
        {
            this.logger = logger;
        }
        public async Task Consume(ConsumeContext<TakeProductMessage> context)
        {
            logger.LogInformation($"Take product called for order {context.Message.OrderId}");
            await context.Publish<OrderCompletedEvent>(new
            {
                CorrelationId = context.Message.CorrelationId,
                OrderId = context.Message.OrderId
            });
        }
    }
}
