using OrderApi.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderApi.Services
{
    public interface IOrderService
    {
        Task<int> CreateOrder(Order order);
        Task<int> AddOrderItem(OrderItem orderItem);
        Task<Order> SubmitOrder(int orderId);
        Task ReturnOrder(int orderId);
        Task<List<OrderItem>> GetOrderItems(int orderId);
    }
}
