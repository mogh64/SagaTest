using MassTransit;
using ProductApi.Services;
using Shared.Contract.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Integrations.Consumers
{
    public class ReturnProductTransactionConsumer : IConsumer<ReturnProductTransactionMessage>
    {
        private readonly IProductService productService;

        public ReturnProductTransactionConsumer(IProductService productService)
        {
            this.productService = productService;
        }
        public Task Consume(ConsumeContext<ReturnProductTransactionMessage> context)
        {
            Dictionary<int, int> productCounts = new Dictionary<int, int>();
            foreach (var item in context.Message.ProductBaskets)
            {
                productCounts.Add(item.ProductId, item.Count);
            }
           return  productService.ReturnProducts(productCounts);
        }
    }
}
