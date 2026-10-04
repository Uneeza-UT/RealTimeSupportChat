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

        public NotificationService(IMapper mapper,
            INotificationRepository notificationRepository,
            ICurrentUserService currentUserService)
        {
            this._mapper = mapper;
            this._notificationRepository = notificationRepository;
            this._currentUserService = currentUserService;
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

            var data = _mapper.Map<Notification>(dto);
            await _notificationRepository.CreateAsync(data);
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
            

            var data = _mapper.Map<List<Notification>>(dtos);
            await _notificationRepository.CreateRangeAsync(data);
        }



        public async Task MarkAsReadAsync(MarkNotificationAsReadDto dto)
        {
            var notification = await _notificationRepository.GetByIdAsync(dto.Id);

            if (notification == null)
            {
                throw new NotFoundException(nameof(Notification), dto.Id);
            }

            notification.IsRead = true;
            await _notificationRepository.SaveChangesAsync();
        }
    }
}
