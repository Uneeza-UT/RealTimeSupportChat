using Microsoft.AspNetCore.Mvc;
using RealTimeSupportChat.Application.Contracts.Services;
using RealTimeSupportChat.Application.DTOs.Notification;


namespace RealTimeSupportChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }



        [HttpGet]
        public async Task<ActionResult<List<GetNotificationDto>>> Get()
        {
            var tickets = await _notificationService.GetAsync();
            return Ok(tickets);
        }



        [HttpGet("unread")]
        public async Task<ActionResult<List<GetNotificationDto>>> GetUnread()
        {
            var tickets = await _notificationService.GetUnreadAsync();
            return Ok(tickets);
        }




        [HttpPatch("{id}/read")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [EndpointSummary("Mark a notification as read")]
        [EndpointDescription("Marks the specified notification as read.")]
        public async Task<ActionResult> Patch(int id)
        {
            await _notificationService.MarkAsReadAsync(id);
            return NoContent();
        }





        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete([FromBody] List<int> ids)
        {
            await _notificationService.DeleteRangeAsync(ids);
            return NoContent();
        }
    }
}
