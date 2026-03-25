using Core.Abstracts;
using Core.Abstracts.IRepositories;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Responses;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShopContext context;
        public UnitOfWork(ShopContext context)
        {
            this.context = context;
        }

        private IProductRepository? productRepository;
        public IProductRepository ProductRepository => productRepository ??= new ProductRepository(context);

        private IProductImageRepository? productImageRepository;
        public IProductImageRepository ProductImageRepository => productImageRepository ??= new ProductImageRepository(context);

        private IProductReviewRepository? productReviewRepository;
        public IProductReviewRepository ProductReviewRepository => productReviewRepository ??= new ProductReviewRepository(context);

        private IProductAttributeRepository? productAttributeRepository;
        public IProductAttributeRepository ProductAttributeRepository => productAttributeRepository ??= new ProductAttributeRepository(context);

        private ICategoryRepository? categoryRepository;
        public ICategoryRepository CategoryRepository => categoryRepository ??= new CategoryRepository(context);

        private ISubcategoryRepository? subcategoryRepository;
        public ISubcategoryRepository SubcategoryRepository => subcategoryRepository ??= new SubcategoryRepository(context);

        private IBrandRepository? brandRepository;
        public IBrandRepository BrandRepository => brandRepository ??= new BrandRepository(context);

        private ICartRepository? cartRepository;
        public ICartRepository CartRepository => cartRepository ??= new CartRepository(context);

        private ICartItemRepository? cartItemRepository;
        public ICartItemRepository CartItemRepository => cartItemRepository ??= new CartItemRepository(context);

        public async Task<IResult> CommitAsync()
        {
            try
            {
                await context.SaveChangesAsync();
                return Result.Success(204);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message, 500);
            }
        }

        public async ValueTask DisposeAsync()
        {
            await context.DisposeAsync();
        }
    }
}
