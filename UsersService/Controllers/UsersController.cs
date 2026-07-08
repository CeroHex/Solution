using Microsoft.AspNetCore.Mvc;
using UsersService.Models;
using UsersService.Services;

namespace UsersService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        #region GET USERS
        [HttpGet]
        public async Task<IActionResult> getUsers()
        {
            var users = await _userService.getUsers();
            return Ok(users);
        }
        #endregion

        #region CREATE USER
        [HttpPost]
        public async Task<IActionResult> createUser(User user)
        {
            var createdUser = await _userService.createUser(user);

            return Ok(createdUser);
        }
        #endregion

        #region DELETE USER
        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteUser(Guid id)
        {
            var result = await _userService.deleteUser(id);

            if (!result)
                return NotFound();

            return Ok();
        }
        #endregion
    }
}