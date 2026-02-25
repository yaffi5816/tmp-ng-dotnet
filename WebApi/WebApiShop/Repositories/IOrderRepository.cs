using Entities;

namespace Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
        Task<Order> AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);
    }
}
