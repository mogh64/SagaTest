using MassTransit;
using OrderApi.Infrastructure;
using Shared.Contract.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Integrations.Consumers
{
    public class ProductsUpdatedEventConsumer : IConsumer<ProductsUpdatedEvent>
    {
        private readonly OrderDbContext dbContext;

        public ProductsUpdatedEventConsumer(OrderDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Task Consume(ConsumeContext<ProductsUpdatedEvent> context)
        {
            
            foreach (var item in context.Message.ProductUpdatedEvents)
            {
                var product = dbContext.Products.FirstOrDefault(x => x.Id == item.ProductId);
                if (product == null)
                {
                    throw new Exception("Product Not Found!");
                }
                product.Count = item.Count;
                product.Price = item.Price;
            }
            
            return dbContext.SaveChangesAsync();
        }
    }
}
