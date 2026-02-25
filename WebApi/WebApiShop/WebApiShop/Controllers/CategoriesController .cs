using Entities;
using Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Text.Json;
using DTO;
//using WebApiShop.Properties;

namespace WebApiShop.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }
        

        // GET api/<Users>/5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        {
            IEnumerable<CategoryDTO> categories = await _service.GetAsync();
            if (categories != null)
            {
                return Ok(categories);
            }
            return NoContent();
        }

        
    }
}
