using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;

namespace WebApi.Controllers
{
  [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var user = _userService.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [HttpPost]
        public IActionResult GetAll()
        {
            var users =  _userService.GetAll();
            return Ok(users);
        }

        [HttpPost("saveUser")]
        public IActionResult save(Usuario miobj)
        {
            var user = _userService.saveUser(miobj);
            return Ok(user);
        }

        [HttpPost("getAllUsers")]
        public IActionResult getAllUsers()
        {
            var users = _userService.getAllUsers();
            return Ok(users);
        }

        [HttpPost("getUserByNom")]
        public IActionResult getUserByNom([FromBody] string nom)
        {
            var users = _userService.getUserByNom(nom);
            return Ok(users);
        }

        [HttpPost("removeUser")]
        public IActionResult remove(Usuario miobj)
        {
            var user = _userService.deleteUser(miobj);
            return Ok(user);
        }

        [HttpPost("getById")]
        public IActionResult getById([FromBody] int id)
        {
            var user = _userService.getById(id);
            return Ok(user);
        }

        [HttpPost("cantMaxUsers")]
        public IActionResult cantMaxUsers()
        {
            var res = _userService.cantMaxUsers();
            return Ok(res);
        }

        [HttpPost("updateAlerta")]
        public IActionResult updateAlerta(Usuario miobj)
        {
            var user = _userService.updateAlerta(miobj);
            return Ok(user);
        }

        [HttpPost("getUsersTrafico")]
        public IActionResult getUsersTrafico()
        {
            var users = _userService.getUsersTrafico();
            return Ok(users);
        }
    }
}
