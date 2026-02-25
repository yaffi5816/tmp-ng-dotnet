using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DashGen2026Context _context;

        public OrderRepository(DashGen2026Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAsync()
        {
            return await _context.Orders.Include(o => o.User).Include(o => o.OrdersItems).ThenInclude(oi => oi.Product).ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders.Include(o => o.User).Include(o => o.OrdersItems).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
        {
            return await _context.Orders.Include(o => o.OrdersItems).ThenInclude(oi => oi.Product).Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task<Order> AddAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
