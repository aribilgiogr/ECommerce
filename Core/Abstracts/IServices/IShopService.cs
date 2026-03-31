using Core.Concretes.DTOs;

namespace Core.Abstracts.IServices
{
    public interface IShopService
    {
        Task<CartDto> GetCartAsync(string customerId);
        Task AddToCartAsync(string customerId, int productId, int quantity = 1);
        Task RemoveFromCartAsync(string customerId, int productId);
    }
}
