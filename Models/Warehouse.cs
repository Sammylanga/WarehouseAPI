namespace WarehouseApi.Models
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; // Must be unique
        public string Name { get; set; } = string.Empty;
        public List<WarehouseProduct> WarehouseProducts { get; set; } = new();
    }
}
