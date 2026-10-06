using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeSupportChat.Application.Contracts.Services;
using RealTimeSupportChat.Application.DTOs.Ticket;


namespace RealTimeSupportChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }



        [HttpGet]
        public async Task<ActionResult<List<GetTicketDto>>> Get()
        {
            var tickets = await _ticketService.GetAsync();
            return Ok(tickets);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<GetTicketDto>> Get(int id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);
            return Ok(ticket);
        }



        [Authorize(Roles = "Customer")]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<CreateTicketDto>> Post([FromBody] CreateTicketDto dto)
        {
            var response = await _ticketService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = response });
        }





        [Authorize(Roles = "SupportManager")]
        [HttpPatch("{id}/assignment")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AssignTicketDto>> Patch(int id, [FromBody] AssignTicketDto dto)
        {
            await _ticketService.AssignTicketAsync(id, dto);
            return NoContent();
        }






        [Authorize(Roles = "SupportManager,SupportAgent")]
        [HttpPatch("{id}/status")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ChangeTicketStatusDto>> Patch(int id, [FromBody] ChangeTicketStatusDto dto)
        {
            await _ticketService.ChangeTicketStatusAsync(id, dto);
            return NoContent();
        }





        [Authorize(Roles = "SupportManager")]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(int id)
        {
            await _ticketService.DeleteAsync(id);
            return NoContent();
        }
    }
}
