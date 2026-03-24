using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class ProductReview : BaseEntity
    {
        public string Review { get; set; } = null!;
        public int Vote { get; set; }
        public string? AttachmentImage { get; set; }

        // Foreign Keys
        public string CustomerId { get; set; } = null!;
        public int ProductId { get; set; }

        // Navigation Properties
        public virtual Customer? Customer { get; set; }
        public virtual Product? Product { get; set; }
    }
}
