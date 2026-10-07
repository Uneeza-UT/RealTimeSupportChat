using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeSupportChat.Application.Contracts.Services;
using RealTimeSupportChat.Application.DTOs.Message;


namespace RealTimeSupportChat.Api.Controllers
{
    [Route("api/ticket/{ticketId}/messages")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messagService;

        public MessageController(IMessageService messagService)
        {
            _messagService = messagService;
        }





        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<GetMessageDto>>> Get(int ticketId)
        {
            var messages = await _messagService.GetByTicketIdAsync(ticketId);
            return Ok(messages);
        }





        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CreateMessageDto>> Post(int ticketId, [FromForm] CreateMessageDto dto)
        {
            var response = await _messagService.SendAsync(ticketId, dto);
            return StatusCode(StatusCodes.Status201Created, response);
        }






        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(int ticketId, [FromBody] List<int> ids)
        {
            await _messagService.DeleteRangeAsync(ticketId, ids);
            return NoContent();
        }
    }
}
