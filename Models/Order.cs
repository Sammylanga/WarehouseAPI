namespace WarehouseApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int SourceWarehouseId { get; set; }
        public Warehouse SourceWarehouse { get; set; } = null!;
        public int DestinationWarehouseId { get; set; }
        public Warehouse DestinationWarehouse { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
