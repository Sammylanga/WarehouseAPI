using Microsoft.EntityFrameworkCore;
using WarehouseApi.Data;
using WarehouseApi.Models;

namespace WarehouseApi.Services
{
    public class WarehouseService
    {
        private readonly WarehouseDbContext _context;

        public WarehouseService(WarehouseDbContext context)
        {
            _context = context;
        }

        // Method to create a product
        public async Task<Product> CreateProductAsync(Product product)
        {
            if (await _context.Products.AnyAsync(w => w.Code == product.Code))
            {
                throw new InvalidOperationException("Product Code must be unique.");
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        // Method to create a warehouse
        public async Task<Warehouse> CreateWarehouseAsync(Warehouse warehouse)
        {
            if (await _context.Warehouses.AnyAsync(w => w.Code == warehouse.Code))
            {
                throw new InvalidOperationException("Warehouse Code must be unique.");
            }

            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync();
            return warehouse;
        }

        // Method to create an order (move products between warehouses)
        public async Task<Order> CreateOrderAsync(Order order)
        {
            // Get source and destination warehouse products
            var sourceWarehouseProduct = await _context.WarehouseProducts
                .FirstOrDefaultAsync(wp => wp.WarehouseId == order.SourceWarehouseId && wp.ProductId == order.ProductId);

            var destinationWarehouseProduct = await _context.WarehouseProducts
                .FirstOrDefaultAsync(wp => wp.WarehouseId == order.DestinationWarehouseId && wp.ProductId == order.ProductId);

            // Check if source has enough stock
            if (sourceWarehouseProduct == null || sourceWarehouseProduct.Quantity < order.Quantity)
            {
                throw new InvalidOperationException("Not enough product in source warehouse.");
            }

            // Reduce quantity in source warehouse
            sourceWarehouseProduct.Quantity -= order.Quantity;

            // Add quantity in destination warehouse
            if (destinationWarehouseProduct == null)
            {
                destinationWarehouseProduct = new WarehouseProduct
                {
                    WarehouseId = order.DestinationWarehouseId,
                    ProductId = order.ProductId,
                    Quantity = order.Quantity
                };
                _context.WarehouseProducts.Add(destinationWarehouseProduct);
            }
            else
            {
                destinationWarehouseProduct.Quantity += order.Quantity;
            }

            await _context.SaveChangesAsync();
            return order;
        }

        // Method to get products on hand in a warehouse
        public async Task<IEnumerable<WarehouseProduct>> GetProductsInWarehouseAsync(int? warehouseId, string? productCode)
        {
            var query = _context.WarehouseProducts.Include(wp => wp.Product).Include(wp => wp.Warehouse).AsQueryable();

            if (warehouseId.HasValue)
                query = query.Where(wp => wp.WarehouseId == warehouseId.Value);

            if (!string.IsNullOrEmpty(productCode))
                query = query.Where(wp => wp.Product.Code == productCode);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Warehouse>> GetWarehousesAsync()
        {
            return await _context.Warehouses.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task CreateWarehouseProductAsync(WarehouseProduct warehouseProduct)
        {
            _context.WarehouseProducts.Add(warehouseProduct);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateWarehouseProductAsync(WarehouseProduct warehouseProduct)
        {
            _context.WarehouseProducts.Update(warehouseProduct);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWarehouseProductAsync(WarehouseProduct warehouseProduct)
        {
            _context.WarehouseProducts.Remove(warehouseProduct);
            await _context.SaveChangesAsync();
        }

    }
}
