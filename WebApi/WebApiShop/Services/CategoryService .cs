using AutoMapper;
using DTO;
using Entities;
using Repositories;
using System.Text.Json;
namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAsync()
        {
            var categoryEntity = await _repository.GetAsync();
            return _mapper.Map<IEnumerable<CategoryDTO>>(categoryEntity);
        }
    }
}
