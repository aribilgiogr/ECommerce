using AutoMapper;
using Core.Abstracts;
using Core.Abstracts.IServices;
using Core.Concretes.DTOs;
using Core.Concretes.Entities;
using Core.Concretes.Enums;

namespace Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task ChangeOrderStatusAsync(int orderId, OrderStatus status)
        {
            var result = await unitOfWork.OrderRepository.FindByIdAsync(orderId);
            if (result.IsSuccess)
            {
                var order = result.Data;
                order.Status = status;
                await unitOfWork.OrderRepository.UpdateAsync(order);
                await unitOfWork.CommitAsync();
            }
        }

        public async Task<int?> CreateOrderAsync(string customerId)
        {
            var cartResult = await unitOfWork.CartRepository.FindManyAsync(c => c.CustomerId == customerId && c.Active, "Items.Product");
            if (cartResult.IsSuccess)
            {
                var cart = cartResult.Data.FirstOrDefault();
                if (cart != null)
                {
                    cart.Active = false;
                    var order = new Order
                    {
                        CustomerId = customerId,
                        CartId = cart.Id
                    };
                    await unitOfWork.CartRepository.UpdateAsync(cart);
                    await unitOfWork.OrderRepository.CreateAsync(order);
                    await unitOfWork.CommitAsync();

                    var orderItems = from item in cart.Items
                                     select new OrderItem
                                     {
                                         Id = item.Id,
                                         OrderId = order.Id,
                                         ProductId = item.ProductId,
                                         Quantity = item.Quantity,
                                         ListPrice = item.Product.Price,
                                         DiscountValue = item.Product.Price * item.Product.DiscountRate / 100
                                     };
                    await unitOfWork.OrderItemRepository.CreateManyAsync(orderItems);
                    await unitOfWork.CommitAsync();
                    return order.Id;
                }
            }
            return null;
        }

        public async Task<OrderDto?> GetOrderAsync(int orderId, string customerId)
        {
            var result = await unitOfWork.OrderRepository.FindByIdAsync(orderId);
            if (result.IsSuccess && result.Data.CustomerId == customerId)
            {
                return mapper.Map<OrderDto>(result.Data);
            }
            else
            {
                return null;
            }
        }
    }
}
