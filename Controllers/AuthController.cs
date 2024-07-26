using Microsoft.AspNetCore.Mvc;
using TicketsApi.Models;
using TicketsApi.Repositories.Interfaces;
using TicketsApi.Services;
using BC = BCrypt.Net.BCrypt;

namespace TicketsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;

        public AuthController(IUserRepository userRepository, TokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (loginModel == null)
            {
                return BadRequest("Requisição inválida!");
            }
            var user = await _userRepository.GetUserByEmailAsync(loginModel.Email);

            if (user == null || !BC.Verify(loginModel.Password, user.Password))
                return Unauthorized();

            var token = _tokenService.GenerateToken(user);

            return Ok(new { Token = token });
        }

    }
}