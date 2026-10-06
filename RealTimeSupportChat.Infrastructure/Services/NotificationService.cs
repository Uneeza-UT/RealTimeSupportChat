using AutoMapper;
using RealTimeSupportChat.Application.Contracts.Identity;
using RealTimeSupportChat.Application.Contracts.Persistence;
using RealTimeSupportChat.Application.Contracts.Services;
using RealTimeSupportChat.Application.DTOs.Notification;
using RealTimeSupportChat.Application.Exceptions;
using RealTimeSupportChat.Application.Validations.Notification;
using RealTimeSupportChat.Domain;

namespace RealTimeSupportChat.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMapper _mapper;
        private readonly INotificationRepository _notificationRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IChatHubService _chatHubService;

        public NotificationService(IMapper mapper,
            INotificationRepository notificationRepository,
            ICurrentUserService currentUserService,
            IChatHubService chatHubService)
        {
            this._mapper = mapper;
            this._notificationRepository = notificationRepository;
            this._currentUserService = currentUserService;
            this._chatHubService = chatHubService;
        }



        public async Task<List<GetNotificationDto>> GetAsync()
        {
            string userId = _currentUserService.UserId;

            var notifications = await _notificationRepository.GetAllByReceiverIdAsync(userId);
            var data = _mapper.Map<List<GetNotificationDto>>(notifications);
            return data;
        }



        public async Task<List<GetNotificationDto>> GetUnreadAsync()
        {
            string userId = _currentUserService.UserId;

            var notifications = await _notificationRepository.GetUnreadByReceiverIdAsync(userId);
            var data = _mapper.Map<List<GetNotificationDto>>(notifications);
            return data;
        }



        // Sends notification to a specific user
        public async Task SendAsync(SendNotificationDto dto)
        {
            var validator = new SendNotificationValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid notification entity. ", validationResult);
            }

            var notification = _mapper.Map<Notification>(dto);
            await _notificationRepository.CreateAsync(notification);


            // Send the new notification to receiver in real time via SignalR

            var notificationDto = _mapper.Map<GetNotificationDto>(notification);
            await _chatHubService.SendNotificationAsync(notificationDto);
        }




        // Sends a list of notifications to specified users
        public async Task SendManyAsync(List<SendNotificationDto> dtos)
        {
            var validator = new SendNotificationValidator();

            foreach (var dto in dtos)
            {
                var validationResult = await validator.ValidateAsync(dto);

                if (validationResult.Errors.Any())
                {
                    throw new BadRequestException("Invalid notification entity. ", validationResult);
                }
            }
            

            var notifications = _mapper.Map<List<Notification>>(dtos);
            await _notificationRepository.CreateRangeAsync(notifications);



            // Send the new notifications to receivers in real time via SignalR

            var notificationDtos = _mapper.Map<List<GetNotificationDto>>(notifications);
            await _chatHubService.SendNotificationsAsync(notificationDtos);
        }



        public async Task MarkAsReadAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);

            if (notification == null)
            {
                throw new NotFoundException(nameof(Notification), id);
            }

            notification.IsRead = true;
            await _notificationRepository.SaveChangesAsync();
        }



        // Delete a notification or multiple notifications
        public async Task DeleteRangeAsync(List<int> ids)
        {
            var notifications = await _notificationRepository.GetAllByIdsAsync(ids);

            if (notifications.Count != ids.Count)
            {
                throw new NotFoundException(nameof(Notification), "One or more notifications were not found.");
            }


            // Ensures only the notification receiver can delete the notifications

            string userId = _currentUserService.UserId;

            if (notifications.Any(m => m.ReceiverId != userId))
            {
                throw new ForbiddenException("You are not authorized to delete one or more notifications.");
            }


            await _notificationRepository.DeleteRangeAsync(notifications);
        }
    }
}
