using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concretes.DTOs
{
    public class CartDto
    {
        public string CustomerId { get; set; } = null!;
        public IEnumerable<CartItemDto> Items { get; set; } = [];
    }

    public class CartItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ProductImage { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public int Quantity { get; set; }
    }
}
