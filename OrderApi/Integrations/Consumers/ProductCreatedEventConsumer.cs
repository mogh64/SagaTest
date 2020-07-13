using MassTransit;
using OrderApi.Infrastructure;
using Shared.Contract.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Integrations.Consumers
{
    public class ProductCreatedEventConsumer : IConsumer<ProductCreatedEvent>
    {
        private readonly OrderDbContext dbContext;

        public ProductCreatedEventConsumer(OrderDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Task Consume(ConsumeContext<ProductCreatedEvent> context)
        {
            dbContext.Products.Add(new Model.Product()
            {
                Id = context.Message.ProductId,
                Count = context.Message.Count,
                Price = context.Message.Price
            });
           return dbContext.SaveChangesAsync();
        }
    }
}
