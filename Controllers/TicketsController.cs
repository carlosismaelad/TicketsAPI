using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsApi.Models;
using TicketsApi.Repositories.Interfaces;
using System.Security.Claims;

namespace TicketsApi.Controllers
{
    [ApiController]
    [Route("api/v1/tickets")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(ITicketRepository ticketRepository, ILogger<TicketsController> logger)
        {
            _ticketRepository = ticketRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var tickets = await _ticketRepository.ReadAllAsync();
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar tickets.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var ticket = await _ticketRepository.ReadAsync(id);
                return Ok(ticket);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar ticket.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Ticket ticket)
        {
            try
            {
                var createdTicket = await _ticketRepository.CreateAsync(ticket);
                return CreatedAtAction(nameof(GetById), new { id = createdTicket.Id }, createdTicket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar ticket.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return BadRequest("ID do ticket não corresponde.");
            }

            try
            {
                var updatedTicket = await _ticketRepository.UpdateAsync(ticket);
                if (updatedTicket == null)
                {
                    return NotFound($"Ticket com id {id} não encontrado.");
                }

                return Ok(updatedTicket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar ticket.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var ticket = await _ticketRepository.DeleteAsync(id);
                if (ticket == null)
                {
                    return NotFound($"Ticket com id {id} não encontrado.");
                }

                return Ok($"Ticket com id {id} foi fechado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar ticket.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            try
            {
                var tickets = await _ticketRepository.ReadAllByTermAsync(term);
                return Ok(tickets);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar tickets por termo.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }
}
