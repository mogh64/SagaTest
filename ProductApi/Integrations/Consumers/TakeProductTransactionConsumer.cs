using MassTransit;
using Microsoft.Extensions.Logging;
using ProductApi.Services;
using Shared.Contract.Events;
using Shared.Contract.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Integrations.Consumers
{
    public class TakeProductTransactionConsumer : IConsumer<TakeProductTransactionMessage>
    {
        private readonly ILogger<TakeProductTransactionConsumer> logger;
        private readonly IProductService productService;
        private readonly IPublishEndpoint publishEndpoint;

        public TakeProductTransactionConsumer(ILogger<TakeProductTransactionConsumer> logger,
            IProductService productService,
            IPublishEndpoint publishEndpoint)
        {
            this.logger = logger;
            this.productService = productService;
            this.publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<TakeProductTransactionMessage> context)
        {
            logger.LogInformation($"Take product called ");
         
            Dictionary<int, int> productCounts = new Dictionary<int, int>();
            foreach (var item in context.Message.ProductBaskets)
            {
                productCounts.Add(item.ProductId, item.Count);
            }
            var products = await productService.TakeProducts(productCounts);
            await publishEndpoint.Publish<ProductsUpdatedEvent>(new
            {
                ProductUpdatedEvents = products.Select(p =>new { ProductId = p.Id,p.Price,p.Count}).ToList()
            });

            await context.RespondAsync<IRequestResult>(new { Result = 1 });
        }
    }
}
