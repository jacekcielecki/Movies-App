using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesApp.Models;
using MoviesApp.Models.Dtos;
using MoviesApp.Services;

namespace MoviesApp.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult GetAll()
        {
            var users = _userService.GetUsers();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public ActionResult GetById([FromRoute]int id)
        {
            var user = _userService.GetUserById(id);
            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public ActionResult ChangePassword([FromRoute]int id, [FromBody]ChangePasswordDto changePasswordDto)
        {
            _userService.ChangePasswordById(id, changePasswordDto);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult DeleteById([FromRoute]int id)
        {
            _userService.DeleteUserById(id);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Create([FromBody]CreateUserDto newUserDto)
        {
            var newUserId = _userService.CreateUser(newUserDto);
            return Created($"/api/Users/{newUserId}", null);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public ActionResult Login([FromBody]UsernamePasswordDto usernamePasswordDto)
        {
            string jwtToken = _userService.UserLogin(usernamePasswordDto);
            return Ok(jwtToken);
        }
    }
}
