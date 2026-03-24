using Core.Abstracts.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concretes.Entities
{
    /// <summary>
    /// Ürün bilgilerini ve özelliklerini içeren ana entity sınıfı
    /// Neden: E-ticaret uygulamasının kalbi. Tüm ürünlerin adı, fiyatı, stok bilgisi, 
    /// açıklaması ve diğer özelliklerini müşterilere sunmak için gereklidir.
    /// </summary>
    public class Product : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountRate { get; set; } = 0;
        public int StockQuantity { get; set; } = 0;

        // Foreign Keys
        public int SubcategoryId { get; set; }
        public int BrandId { get; set; }

        // Navigation Properties
        public virtual Subcategory? Subcategory { get; set; }
        public virtual Brand? Brand { get; set; }
        public virtual ICollection<ProductAttribute> Attributes { get; set; } = [];
        public virtual ICollection<ProductImage> Images { get; set; } = [];
        public virtual ICollection<CartItem> CartItems { get; set; } = [];
        public virtual ICollection<ProductReview> Reviews { get; set; } = [];
    }
}
