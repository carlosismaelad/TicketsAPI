using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsApi.Models;
using TicketsApi.Repositories.Interfaces;
using TicketsApi.Services;
using BC = BCrypt.Net.BCrypt;

namespace TicketsApi.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserRepository userRepository, TokenService tokenService, ILogger<AuthController> logger)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (loginModel == null)
            {
                _logger.LogWarning("Login falhou: Requisição inválida.");
                return BadRequest("Requisição inválida!");
            }
            var user = await _userRepository.GetUserByEmailAsync(loginModel.Email);

            if (user == null || !BC.Verify(loginModel.Password, user.Password))
            {
                _logger.LogWarning("Login falhou: Usuário ou senha inválida.");
                return Unauthorized("Usuário ou senha inválida!");
            }

            var token = _tokenService.GenerateToken(user);
            user.Password = "";

            _logger.LogInformation($"Usuário {user.Username} logado com sucesso.");

            return Ok(new
            {
                username = user.Username,
                password = user.Password,
                role = user.Role,
                token = token
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // Obtém o cabeçalho Authorization
            var authHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                _logger.LogWarning("Logout falhou: Cabeçalho 'Authorization' malformado.");
                return BadRequest("Cabeçalho 'Authorization' malformado.");
            }

            // Extrai o token do cabeçalho
            var token = authHeader.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Logout falhou: Token não fornecido.");
                return BadRequest("Token não fornecido.");
            }

            // Verifica se o token está revogado
            if (_tokenService.IsTokenRevoked(token))
            {
                _logger.LogWarning("Logout falhou: Token revogado.");
                return Unauthorized("Token revogado.");
            }

            // Revoga o token
            _tokenService.RevokeToken(token);
            _logger.LogInformation("Logout efetuado com sucesso para o token {Token}.", token);
            return Ok("Logout efetuado com sucesso!");
        }
    }
}