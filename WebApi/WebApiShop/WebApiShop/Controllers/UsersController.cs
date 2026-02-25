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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadOnlyDTO>>> GetUsers([FromQuery] int currentId)
        {
            IEnumerable<UserReadOnlyDTO> usersDTO = await _service.GetAsync(currentId);
            if (usersDTO != null && usersDTO.Any())
            {
                return Ok(usersDTO);
            }
            return NoContent();

        }


        // GET api/<Users>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadOnlyDTO>> GetUserById(int id)
        {
            UserReadOnlyDTO user =await _service.GetUserById(id);
            if (user != null)
            {
                return Ok(user);
            }
            return NoContent();
        }

        // POST api/<Users>
        [HttpPost]
        public async Task<ActionResult<UserReadOnlyDTO>> Post([FromBody] UserRegisterDTO user)
        {
            UserReadOnlyDTO user1 = await _service.AddUser(user);
            if(user1 != null) 
            {
                return CreatedAtAction(nameof(GetUserById), new { Id = user1.UserId }, user1);
            }
            return BadRequest();
        }



        // POST api/<Users>
        [HttpPost("Login")]
        public async Task<ActionResult<UserReadOnlyDTO>> Login([FromBody] UserLoginDTO user)
        {
            UserReadOnlyDTO user1 = await _service.Login(user);
            if (user1 != null)
            {
                return CreatedAtAction(nameof(GetUserById), new { Id = user1.UserId }, user1);
            }
            return BadRequest();

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put( int id, [FromBody] UserUpdateDTO user)
        {
            await _service.UpdateUser(id, user);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromQuery] int currentUserId)
        {
            await _service.DeleteUser(currentUserId, id);
            return Ok();
        }
    }
}
