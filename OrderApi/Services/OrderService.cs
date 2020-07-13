using Microsoft.EntityFrameworkCore;
using OrderApi.Infrastructure;
using OrderApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderDbContext dbContext;

        public OrderService(OrderDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> AddOrderItem(OrderItem orderItem)
        {
            var product = dbContext.Products.FirstOrDefault(x => x.Id == orderItem.ProductId);
            if (product.Count < orderItem.Count)
            {
                throw new Exception("Product Count exceeded the existance!");
            }
            var order = dbContext.Orders.FirstOrDefault(x => x.Id == orderItem.OrderId);
            order.OrderItems.Add(orderItem);
            product.Count -= orderItem.Count; 
            await dbContext.SaveChangesAsync();
            return orderItem.Id; 
        }
        public Task<List<OrderItem>> GetOrderItems(int orderId)
        {
            return dbContext.OrderItems.Where(x => x.OrderId == orderId).ToListAsync();
        }
        public async Task<int> CreateOrder(Order order)
        {
            order.State = OrderState.Created;
            dbContext.Add(order);
            await dbContext.SaveChangesAsync();
            return order.Id;
        }

        public async Task<Order> SubmitOrder(int orderId)
        {
            var order = dbContext.Orders.FirstOrDefault(x => x.Id == orderId);
            if(order==null)
            {
                throw new Exception("Order Not Found!");
            }
            order.State = OrderState.Submitted;
            await dbContext.SaveChangesAsync();
            return order;
        }

        public Task ReturnOrder(int orderId)
        {
            var order = dbContext.Orders.FirstOrDefault(x => x.Id == orderId);
            if (order == null)
            {
                throw new Exception("Order Not Found!");
            }
            order.State = OrderState.Created;
            return dbContext.SaveChangesAsync();
        }
    }
}
