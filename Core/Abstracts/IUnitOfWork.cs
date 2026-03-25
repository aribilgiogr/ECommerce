using Core.Abstracts.IRepositories;
using Utils.Responses;

namespace Core.Abstracts
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IProductRepository ProductRepository { get; }
        IProductImageRepository ProductImageRepository { get; }
        IProductReviewRepository ProductReviewRepository { get; }
        IProductAttributeRepository ProductAttributeRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ISubcategoryRepository SubcategoryRepository { get; }
        IBrandRepository BrandRepository { get; }
        ICartRepository CartRepository { get; }
        ICartItemRepository CartItemRepository { get; }

        Task<IResult> CommitAsync();
    }
}
