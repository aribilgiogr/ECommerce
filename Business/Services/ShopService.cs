using AutoMapper;
using Core.Abstracts;
using Core.Abstracts.IServices;
using Core.Concretes.DTOs;
using Core.Concretes.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ShopService : IShopService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ShopService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task AddToCartAsync(string customerId, int productId, int quantity = 1)
        {
            var cart = await getCart(customerId);

            var cartItemResult = await unitOfWork.CartItemRepository.FindFirstAsync(x => x.CartId == cart.Id && x.ProductId == productId);
            if (cartItemResult.IsSuccess)
            {
                CartItem cartItem = cartItemResult.Data;
                cartItem.Quantity += quantity;
                await unitOfWork.CartItemRepository.UpdateAsync(cartItem);
            }
            else
            {
                CartItem newCartItem = new()
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity
                };
                await unitOfWork.CartItemRepository.CreateAsync(newCartItem);
            }
            await unitOfWork.CommitAsync();
        }

        public async Task<CartDto> GetCartAsync(string customerId)
        {
            var cart = await getCart(customerId);
            return mapper.Map<CartDto>(cart);
        }

        public async Task RemoveFromCartAsync(string customerId, int productId)
        {
            var cart = await getCart(customerId);

            var cartItemResult = await unitOfWork.CartItemRepository.FindFirstAsync(x => x.CartId == cart.Id && x.ProductId == productId);

            if (cartItemResult.IsSuccess) {
                await unitOfWork.CartItemRepository.DeleteAsync(cartItemResult.Data);
                await unitOfWork.CommitAsync();
            }
        }

        private async Task<Cart> getCart(string customerId)
        {
            var result = await unitOfWork.CartRepository.FindManyAsync(c => c.CustomerId == customerId && c.Active,"Items.Product");

            if (result.IsSuccess)
            {
                return result.Data.FirstOrDefault();
            }
            else
            {
                var newCart = new Cart { CustomerId = customerId };
                await unitOfWork.CartRepository.CreateAsync(newCart);
                var commitResult = await unitOfWork.CommitAsync();
                if (commitResult.IsSuccess)
                {
                    return newCart;
                }
                else
                {
                    throw new Exception("Sepet oluşturulurken bir hata oluştu.");
                }
            }
        }
    }
}
