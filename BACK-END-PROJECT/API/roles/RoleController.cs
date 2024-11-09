using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BACK_END_PROJECT.API.roles.requests;
using BACK_END_PROJECT.API.roles.responses;

namespace BACK_END_PROJECT.API.roles
{
    [Route("api/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        // POST: api/roles
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] CreateRoleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Se o modelo for inválido, retorna 400
            }

            // Converte o CreateRoleRequest para Role e chama o serviço para salvar
            var role = request.ToRole();
            var createdRole = await _roleService.InsertAsync(role);

            // Retorna a resposta com status 201 Created
            var roleResponse = new RoleResponse(createdRole);
            return CreatedAtAction(nameof(GetRoleByName), new { name = createdRole.Name }, roleResponse);
        }

        // GET: api/roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleResponse>>> List()
        {
            var roles = await _roleService.FindAllAsync();
            var roleResponses = new List<RoleResponse>();

            foreach (var role in roles)
            {
                roleResponses.Add(new RoleResponse(role));
            }

            return Ok(roleResponses);
        }

        // GET: api/roles/{name}
        [HttpGet("{name}")]
        public async Task<ActionResult<RoleResponse>> GetRoleByName(string name)
        {
            var role = await _roleService.FindByNameAsync(name);
            if (role == null)
            {
                return NotFound(new { Message = "Role não encontrada." });
            }

            var roleResponse = new RoleResponse(role);
            return Ok(roleResponse);
        }
    }
}
