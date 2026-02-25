using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DashGen2026Context _context;

        public ProductRepository(DashGen2026Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAsync()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }

        public async Task<(IEnumerable<Product> products, int total)> GetProductsAsync(int[]? categoryId, string? description, double? minPrice, double? maxPrice, int? limit, int? page)
        {
            var query = _context.Products.Where(product =>
                        (description == null || product.ProductDescreption.Contains(description))
                        && (minPrice == null || product.Price >= minPrice)
                        && (maxPrice == null || product.Price <= maxPrice)
                        && (categoryId == null || categoryId.Length == 0 || categoryId.Contains(product.CategoryId)))
                            .OrderBy(product => product.Price);

            var total = await query.CountAsync();
            
            if (limit.HasValue && page.HasValue)
            {
                var products = await query
                    .Skip((page.Value - 1) * limit.Value)
                    .Take(limit.Value)
                    .Include(product => product.Category)
                    .ToListAsync();
                return (products, total);
            }
            else
            {
                var products = await query.Include(product => product.Category).ToListAsync();
                return (products, total);
            }
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.ProductId);
            if (existingProduct != null)
            {
                existingProduct.ProductName = product.ProductName;
                existingProduct.ProductDescreption = product.ProductDescreption;
                existingProduct.Price = product.Price;
                existingProduct.ImgUrl = product.ImgUrl;
                existingProduct.CategoryId = product.CategoryId;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
