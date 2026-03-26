using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concretes.DTOs
{
    public class ProductListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal DiscountedPrice => Price * (100 - DiscountRate) / 100;
        public string BrandName { get; set; } = null!;
        public string SubcategoryName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string? CoverImageUrl { get; set; }
        public int Rating { get; set; }
        public int ReviewCount { get; set; }
    }

    public class ProductDetailDto
    {

    }
}
