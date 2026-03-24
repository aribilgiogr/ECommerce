namespace Core.Concretes.Entities
{
    public class ProductAttribute
    {
        public int Id { get; set; }
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;

        // Foreign Keys
        public int ProductId { get; set; }

        // Navigation Properties
        public virtual Product? Product { get; set; }
    }
}
