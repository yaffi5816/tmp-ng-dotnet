using DTO;

namespace Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAsync();
        Task<OrderDTO?> GetByIdAsync(int id);
        Task<IEnumerable<OrderDTO>> GetByUserIdAsync(int userId);
        Task<OrderDTO> AddAsync(OrderDTO order);
        Task UpdateAsync(int id, OrderDTO order);
        Task DeleteAsync(int id);
    }
}
