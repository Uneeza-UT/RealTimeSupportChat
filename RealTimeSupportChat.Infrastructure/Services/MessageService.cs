using AutoMapper;
using RealTimeSupportChat.Application.Contracts.Identity;
using RealTimeSupportChat.Application.Contracts.Persistence;
using RealTimeSupportChat.Application.Contracts.Services;
using RealTimeSupportChat.Application.DTOs.Message;
using RealTimeSupportChat.Application.DTOs.Notification;
using RealTimeSupportChat.Application.Exceptions;
using RealTimeSupportChat.Application.Validations.Message;
using RealTimeSupportChat.Domain;
using RealTimeSupportChat.Domain.Enums;

namespace RealTimeSupportChat.Infrastructure.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMapper _mapper;
        private readonly IMessageRepository _messageRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileStorageService _fileStorageService;
        private readonly INotificationService _notificationService;

        public MessageService(IMapper mapper,
            IMessageRepository messageRepository,
            ITicketRepository ticketRepository,
            ICurrentUserService currentUserService,
            IFileStorageService fileStorageService,
            INotificationService notificationService)
        {
            this._mapper = mapper;
            this._messageRepository = messageRepository;
            this._ticketRepository = ticketRepository;
            this._currentUserService = currentUserService;
            this._fileStorageService = fileStorageService;
            this._notificationService = notificationService;
        }



        public async Task<List<GetMessageDto>> GetByTicketIdAsync(int ticketId)
        {
            IReadOnlyList<Message> messages;

            string userId = _currentUserService.UserId;
            string role = _currentUserService.Role;

            switch (role)
            {
                case "SupportManager":
                    messages = await _messageRepository.GetAllByTicketIdAsync(ticketId);
                    break;

                case "Customer" or "SupportAgent":
                    messages = await _messageRepository.GetAllByTicketIdAndUserIdAsync(ticketId, userId);
                    break;


                default:
                    throw new ForbiddenException("You are not authorized to view this ticket's messages.");
            }

            var data = _mapper.Map<List<GetMessageDto>>(messages);
            return data;
        }



        
        public async Task<int> SendAsync(CreateMessageDto dto)
        {
            string userId = _currentUserService.UserId;

            // Validate the dto
            var validator = new CreateMessageValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid message entity. ", validationResult);
            }


            var ticket = await _ticketRepository.GetByIdAsync(dto.TicketId);

            if (ticket == null)
            {
                throw new NotFoundException(nameof(Ticket), dto.TicketId);
            }


            // Prevent sending messages in unassigned tickets

            if (ticket.AssignedToId == null)
            {
                throw new BadRequestException("You cannot send messages in an unassigned ticket.");
            }


            // Ensure only the customer who created the ticket and
            // the user assigned to it can send messages in the ticket

            if (ticket.CustomerId != userId && ticket.AssignedToId != userId)
            {
                throw new ForbiddenException("You are not authorized to send messages in this ticket.");
            }


            // Prevent sending messages in closed tickets

            if (ticket.Status == TicketStatus.Closed)
            {
                throw new BadRequestException("You cannot send messages in a closed ticket.");
            }



            var message = new Message
            {
                TicketId = ticket.Id,
                SenderId = userId,
                Content = dto.Content
            };


            // Add attachments to the message entity

            if (dto.Attachments.Count != 0)
            {
                foreach (var file in dto.Attachments)
                {
                    var filePath = await _fileStorageService.SaveFileAsync(file);

                    var attachment = new Attachment
                    {
                        FileName = file.FileName,
                        FilePath = filePath,
                        ContentType = file.ContentType
                    };

                    message.Attachments.Add(attachment);
                }
            }

            await _messageRepository.CreateAsync(message);


            // Notify the user about a new message

            string receiverId = userId == ticket.CustomerId
                ? ticket.AssignedToId
                : ticket.CustomerId;


            var notification = new SendNotificationDto
            {
                TicketId = ticket.Id,
                ReceiverId = receiverId,
                Message = $"You have received a new message in ticket \"{ticket.Subject}\"."
            };

            await _notificationService.SendAsync(notification);

            return message.Id;
        }



        // Delete a message or multiple messages
        public async Task DeleteRangeAsync(List<int> ids)
        {
            var messages = await _messageRepository.GetAllByIdsWithAttachmentsAsync(ids);


            if (messages.Count != ids.Count)
            {
                throw new NotFoundException(nameof(Message), "One or more messages were not found.");
            }


            // Ensures only the message sender can delete the messages

            string userId = _currentUserService.UserId;

            if (messages.Any(m => m.SenderId != userId))
            {
                throw new ForbiddenException("You are not authorized to delete one or more messages.");
            }


            // Delete attachments associated with the messages

            var filePaths = messages
                .SelectMany(m => m.Attachments)
                .Select(a => a.FilePath)
                .ToList();

            
            await _fileStorageService.DeleteFilesAsync(filePaths);

            await _messageRepository.DeleteRangeAsync(messages);      
        }

    }
}
