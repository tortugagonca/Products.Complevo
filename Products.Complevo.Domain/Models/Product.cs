namespace Products.Complevo.Domain.Models
{
    public class Product : BaseEntity
    {
        public uint ProductCode { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
    }
}
