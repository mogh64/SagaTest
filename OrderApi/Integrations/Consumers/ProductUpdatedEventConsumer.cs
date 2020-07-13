using MassTransit;
using OrderApi.Infrastructure;
using Shared.Contract.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Integrations.Consumers
{
    public class ProductUpdatedEventConsumer : IConsumer<ProductUpdatedEvent>
    {
        private readonly OrderDbContext dbContext;

        public ProductUpdatedEventConsumer(OrderDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Task Consume(ConsumeContext<ProductUpdatedEvent> context)
        {
            var product = dbContext.Products.FirstOrDefault(x => x.Id == context.Message.ProductId);
            if (product == null)
            {
                throw new Exception("Product Not Found!");
            }
            product.Count = context.Message.Count;
            product.Price = context.Message.Price; 
            return dbContext.SaveChangesAsync();
        }
    }
}
