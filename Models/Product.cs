namespace WarehouseApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; // Must be unique
        public string Description { get; set; } = string.Empty;
    }
}
