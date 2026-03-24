using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;

        // Navigation Properties
        public virtual ICollection<Subcategory> Subcategories { get; set; } = [];
    }
}
