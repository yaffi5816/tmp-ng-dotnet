using Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;
namespace Repositories
{
    public class CategoryRepository :  ICategoryRepository
    {

        DashGen2026Context _dashGen2026Context;

        public CategoryRepository(DashGen2026Context dashGen2026Context)
        {
            _dashGen2026Context = dashGen2026Context;
        }

        public async Task<IEnumerable<Category>> GetAsync()
        {
            return await _dashGen2026Context.Categories.ToListAsync();
        }
    }

}
