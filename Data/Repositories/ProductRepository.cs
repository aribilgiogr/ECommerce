using Core.Abstracts.IRepositories;
using Core.Concretes.Entities;
using Utils.Generics;

namespace Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ShopContext db) : base(db)
        {
        }
    }

    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(ShopContext db) : base(db)
        {
        }
    }

    public class ProductReviewRepository : Repository<ProductReview>, IProductReviewRepository
    {
        public ProductReviewRepository(ShopContext db) : base(db)
        {
        }
    }

    public class ProductAttributeRepository : Repository<ProductAttribute>, IProductAttributeRepository
    {
        public ProductAttributeRepository(ShopContext db) : base(db)
        {
        }
    }

    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ShopContext db) : base(db)
        {
        }
    }

    public class SubcategoryRepository : Repository<Subcategory>, ISubcategoryRepository
    {
        public SubcategoryRepository(ShopContext db) : base(db)
        {
        }
    }

    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        public BrandRepository(ShopContext db) : base(db)
        {
        }
    }

    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(ShopContext db) : base(db)
        {
        }
    }

    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(ShopContext db) : base(db)
        {
        }
    }
}
