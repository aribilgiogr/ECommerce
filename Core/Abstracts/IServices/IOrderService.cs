using Core.Concretes.DTOs;
using Core.Concretes.Enums;

namespace Core.Abstracts.IServices
{
    public interface IOrderService
    {
        Task<int?> CreateOrderAsync(string customerId);
        Task ChangeOrderStatusAsync(int orderId, OrderStatus status);
        Task<OrderDto?> GetOrderAsync(int orderId, string customerId);
    }
}
