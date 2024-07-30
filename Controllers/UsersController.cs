using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;
using TicketsApi.Models;
using TicketsApi.Repositories.Interfaces;

namespace TicketsApi.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            try
            {
                var createdUser = await _userRepository.CreateAsync(user);
                return CreatedAtAction(nameof(GetByUsername), new { username = createdUser.Username }, createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar usuário.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userRepository.ReadAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuários.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet("by-username")]
        [Authorize(Roles = "Admin, Default")]
        public async Task<IActionResult> GetByUsername([FromQuery] string username)
        {
            try
            {
                var user = await _userRepository.ReadByUsernameAsync(username);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuário.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            if (!User.IsInRole("Admin"))
                return Forbid("Você não tem permissão para esta ação.");

            try
            {
                var updatedUser = await _userRepository.UpdateAsync(user);
                if (updatedUser == null)
                {
                    return NotFound($"Usuário com id {user.Id} não encontrado.");
                }

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar usuário.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromBody] User user)
        {
            if (!User.IsInRole("Admin"))
                return Forbid("Você não tem permissão para esta ação.");

            try
            {
                var deletedUser = await _userRepository.DeleteAsync(user.Id);
                if (deletedUser == null)
                {
                    return NotFound($"Usuário com id {user.Id} não encontrado.");
                }

                return Ok($"Excluído com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar usuário.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Default")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            try
            {
                var users = await _userRepository.ReadAllByTermAsync(term);
                return Ok(users);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuários por termo.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }
}
