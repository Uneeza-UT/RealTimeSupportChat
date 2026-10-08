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
        private readonly IChatHubService _chatHubService;

        public MessageService(IMapper mapper,
            IMessageRepository messageRepository,
            ITicketRepository ticketRepository,
            ICurrentUserService currentUserService,
            IFileStorageService fileStorageService,
            INotificationService notificationService,
            IChatHubService chatHubService)
        {
            this._mapper = mapper;
            this._messageRepository = messageRepository;
            this._ticketRepository = ticketRepository;
            this._currentUserService = currentUserService;
            this._fileStorageService = fileStorageService;
            this._notificationService = notificationService;
            this._chatHubService = chatHubService;
        }



        public async Task<List<GetMessageDto>> GetByTicketIdAsync(int ticketId)
        {
            var ticket = await GetTicketOrThrowAsync(ticketId);

            string userId = _currentUserService.UserId;
            string errorMessage = "You can only view messages of tickets you created or are assigned to.";

            EnsureTicketAccess(ticket, userId, errorMessage);

       
            var messages = await _messageRepository.GetAllByTicketIdAndUserIdAsync(ticketId, userId);              
            var data = _mapper.Map<List<GetMessageDto>>(messages);
            return data;
        }



        
        public async Task<int> SendAsync(int ticketId, CreateMessageDto dto)
        {
            string userId = _currentUserService.UserId;

            // Validate the dto
            var validator = new CreateMessageValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid message entity. ", validationResult);
            }


            var ticket = await GetTicketOrThrowAsync(ticketId);


            // Prevent sending messages in unassigned tickets

            if (ticket.AssignedToId == null)
            {
                throw new BadRequestException("You cannot send messages in an unassigned ticket.");
            }


            // Ensure only the customer who created the ticket and
            // the user assigned to it can send messages in the ticket

            string errorMessage = "You can only send messages in tickets you created or are assigned to.";
            EnsureTicketAccess(ticket, userId, errorMessage);



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


            // Send the new message to both the sender and receiver in real time via SignalR

            var messageDto = _mapper.Map<GetMessageDto>(message);
            

            string receiverId = userId == ticket.CustomerId
                ? ticket.AssignedToId
                : ticket.CustomerId;

            await _chatHubService.SendMessageAsync(receiverId, messageDto);


            // Notify the user about a new message

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
        // Ensure users can only delete messages of their created or assigned tickets
        public async Task DeleteRangeAsync(int ticketId, List<int> ids)
        {
            string userId = _currentUserService.UserId;

            var ticket = await GetTicketOrThrowAsync(ticketId);

            string errorMessage = "You can only delete messages from tickets you created or are assigned to.";
            EnsureTicketAccess(ticket, userId, errorMessage);


            var messages = await _messageRepository.GetAllByIdsWithAttachmentsAsync(ids);


            if (messages.Count != ids.Count)
            {
                throw new NotFoundException("One or more messages were not found.");
            }


            // Ensure all messages belong to the same ticket

            if (messages.Any(m => m.TicketId != ticketId))
            {
                throw new BadRequestException("One or more messages do not belong to the specified ticket.");
            }


            // Ensures only the message sender can delete the messages

            var userRole = _currentUserService.Role;
            EnsureCanDeleteMessages(ticket, messages, userRole);


            // Delete attachments associated with the messages

            var filePaths = messages
                .SelectMany(m => m.Attachments)
                .Select(a => a.FilePath)
                .ToList();

            
            await _fileStorageService.DeleteFilesAsync(filePaths);

            await _messageRepository.DeleteRangeAsync(messages);


            // Remove the messages from both current participants in real time via SignalR

            string receiverId = userId == ticket.CustomerId
               ? ticket.AssignedToId!
               : ticket.CustomerId;

            await _chatHubService.DeleteMessagesAsync(ids, userId, receiverId);
        }



        // Gets a ticket or throws NotFoundException if it doesn't exist.
        private async Task<Ticket> GetTicketOrThrowAsync(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);

            if (ticket == null)
            {
                throw new NotFoundException(nameof(Ticket), id);
            }

            return ticket;
        }


        
        private void EnsureTicketAccess(Ticket ticket, string userId, string errorMessage)
        {
            if (ticket.CustomerId != userId && ticket.AssignedToId != userId)
            {
                throw new ForbiddenException(errorMessage);
            }
        }


        private void EnsureCanDeleteMessages(Ticket ticket, List<Message> messages, string userRole)
        {
            if (userRole == "Customer" && messages.Any(m => m.SenderId != ticket.CustomerId))
            {
                throw new ForbiddenException("You can only delete your own messages.");
            }

            else if ((userRole == "SupportAgent" || userRole == "SupportManager") &&
                messages.Any(m => m.SenderId == ticket.CustomerId))
            {
                throw new ForbiddenException("You can only delete your own messages.");
            }
        }
    }
}
