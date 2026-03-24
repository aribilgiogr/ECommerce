using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concretes.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = null!;
        public bool IsCoverImage { get; set; } = false;

        // Foreign Keys
        public int ProductId { get; set; }

        // Navigation Properties
        public virtual Product? Product { get; set; }
    }
}
