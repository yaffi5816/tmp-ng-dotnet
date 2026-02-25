using AutoMapper;
using DTO;
using Entities;
using Repositories;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDTO>> GetAsync()
        {
            var orders = await _repository.GetAsync();
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<OrderDTO?> GetByIdAsync(int id)
        {
            var order = await _repository.GetByIdAsync(id);
            return order != null ? _mapper.Map<OrderDTO>(order) : null;
        }

        public async Task<IEnumerable<OrderDTO>> GetByUserIdAsync(int userId)
        {
            var orders = await _repository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> AddAsync(OrderDTO orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            var newOrder = await _repository.AddAsync(order);
            return _mapper.Map<OrderDTO>(newOrder);
        }

        public async Task UpdateAsync(int id, OrderDTO orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            order.OrderId = id;
            await _repository.UpdateAsync(order);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
