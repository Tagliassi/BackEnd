using BACK_END_PROJECT.API.errors;
using BACK_END_PROJECT.API.security;
using BACK_END_PROJECT.API.users;
using BACK_END_PROJECT.API.users.requests;
using BACK_END_PROJECT.API.users.responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BACK_END_PROJECT.API.users
{
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Insert([FromBody] CreateUserRequest userRequest)
        {
            if (userRequest == null)
                return BadRequest(new BadRequestException("Invalid request"));

            var user = _service.Insert(userRequest.ToUser());
            return CreatedAtAction(nameof(FindById), new { id = user.Id }, new UserResponse(user));
        }

        [HttpGet]
        public IActionResult List([FromQuery] string sortDir, [FromQuery] string role)
        {
            var sortDirection = SortDirExtensions.GetByName(sortDir) ?? throw new BadRequestException("Invalid sort dir!");

            var users = _service.List(sortDirection, role)
                .Select(user => new UserResponse(user))
                .ToList();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult FindById(long id)
        {
            var user = _service.FindByIdOrNull(id);
            if (user == null) return NotFound();

            return Ok(new UserResponse(user));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Delete(long id)
        {
            var user = _service.Delete(id);
            if (user != null) return Ok(); // Se o usuário foi deletado, retorna OK

            return NotFound(); // Caso o usuário não exista
        }

        [HttpPatch("{id}")]
        [Authorize]
        public IActionResult Update(long id, [FromBody] UpdateUserRequest updateRequest)
        {
            var token = HttpContext.User.Identity as UserToken;

            if (token == null || (token.Id != id && !token.IsAdmin))
                throw new ForbiddenException();

            var user = _service.Update(id, updateRequest.Name);
            if (user == null) return NoContent();

            return Ok(new UserResponse(user));
        }

        [HttpPut("{id}/roles/{role}")]
        public IActionResult GrantRole(long id, string role)
        {
            var result = _service.AddRole(id, role.ToUpper());
            if (result) return Ok();

            return NoContent();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var loginResponse = _service.Login(loginRequest.Email, loginRequest.Password);
            if (loginResponse == null)
                return Unauthorized();

            return Ok(loginResponse);
        }
    }
}
