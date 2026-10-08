using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RealTimeSupportChat.Application.Contracts.Identity;
using RealTimeSupportChat.Application.Contracts.Persistence;
using RealTimeSupportChat.Application.Contracts.Services;
using RealTimeSupportChat.Application.DTOs.Notification;
using RealTimeSupportChat.Application.DTOs.Ticket;
using RealTimeSupportChat.Application.Exceptions;
using RealTimeSupportChat.Application.Validations.Ticket;
using RealTimeSupportChat.Domain;
using RealTimeSupportChat.Domain.Enums;
using RealTimeSupportChat.Identity.Models;

namespace RealTimeSupportChat.Infrastructure.Services
{
    public class TicketService : ITicketService
    {
        private readonly IMapper _mapper;
        private readonly ITicketRepository _ticketRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;


        public TicketService(IMapper mapper,
            ITicketRepository ticketRepository,
            ICurrentUserService currentUserService,
            UserManager<ApplicationUser> userManager,
            INotificationService notificationService)
        {
            this._mapper = mapper;
            this._ticketRepository = ticketRepository;
            this._currentUserService = currentUserService;
            this._userManager = userManager;
            this._notificationService = notificationService;   
        }


        public async Task<List<GetTicketDto>> GetAsync()
        {
            IReadOnlyList<Ticket> tickets;

            string userId = _currentUserService.UserId;
            string role = _currentUserService.Role;
            

            switch (role)
            {
                case "Customer":
                    tickets = await _ticketRepository.GetAllByCustomerIdAsync(userId);
                    break;

                case "SupportManager":
                    tickets = await _ticketRepository.GetAsync();
                    break;

                case "SupportAgent":
                    tickets = await _ticketRepository.GetAllByAssignedToIdAsync(userId);
                    break;

                default:
                    throw new ForbiddenException("You are not authorized to view tickets.");
            }

            var data = _mapper.Map<List<GetTicketDto>>(tickets);
            return data;
        }



        public async Task<GetTicketDto> GetByIdAsync(int id)
        {
            var ticket = await GetTicketOrThrowAsync(id);
            string userId = _currentUserService.UserId;
            string role = _currentUserService.Role;


            switch (role)
            {
                case "Customer":
                    if (ticket.CustomerId != userId)
                    {
                        throw new ForbiddenException(
                            "You can only view tickets you created.");
                    }
                    break;


                case "SupportAgent":
                    if (ticket.AssignedToId != userId)
                    {
                        throw new ForbiddenException(
                            "You can only view tickets assigned to you.");
                    }
                    break;



                case "SupportManager":
                    break;


                default:
                    throw new ForbiddenException("You are not authorized to view tickets.");
            }

            var data = _mapper.Map<GetTicketDto>(ticket);
            return data;
        }



        // Creates a new ticket entity
        // Only a user with "Customer" role can create a ticket
        public async Task<int> CreateAsync(CreateTicketDto dto)
        {
            // Validate the dto
            var validator = new CreateTicketValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid ticket entity. ", validationResult);
            }

            var ticket = _mapper.Map<Ticket>(dto);
            ticket.CustomerId = _currentUserService.UserId;

            await _ticketRepository.CreateAsync(ticket);


            // Notify all support managers about a new ticket creation

            var users = await _userManager.GetUsersInRoleAsync("SupportManager");

            List<SendNotificationDto> notifications = new();

            if (users.Count != 0)
            {
                foreach (var user in users)
                {
                    notifications.Add(new SendNotificationDto
                    {
                        TicketId = ticket.Id,
                        ReceiverId = user.Id,
                        Message = $"A new ticket has been created. Subject: {ticket.Subject}."
                    });
                }

                await _notificationService.SendManyAsync(notifications);
            }

            return ticket.Id;
        }



        // Assign ticket to a support agent or support manager
        // Only users with "SupportManager" role can perform this task
        public async Task AssignTicketAsync(int id, AssignTicketDto dto)
        {
            // Validate the dto
            var validator = new AssignTicketValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid ticket entity. ", validationResult);
            }


            var ticket = await GetTicketOrThrowAsync(id);


            // Ensures that the user who is being assigned the ticket is a support agent or manager

            var user = await _userManager.FindByIdAsync(dto.AssignedToId);

            if (user == null)
            {
                throw new NotFoundException(nameof(ApplicationUser), dto.AssignedToId);
            }


            var userRoles = await _userManager.GetRolesAsync(user);

            if (!userRoles.Contains("SupportAgent") && !userRoles.Contains("SupportManager"))
            {
                throw new BadRequestException("Only users with the SupportAgent or SupportManager " +
                    "roles can be assigned a ticket.");
            }


            // Check if ticket is being reassigned or not

            string? previousAssigneeId = ticket.AssignedToId;


            ticket.AssignedToId = dto.AssignedToId;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _ticketRepository.SaveChangesAsync();


            // Notify the assigned user, previous assignee and customer about the ticket assignment

            List<SendNotificationDto> notifications = new();

            if (!string.IsNullOrWhiteSpace(previousAssigneeId))
            {
                notifications.Add(new SendNotificationDto
                {
                    TicketId = ticket.Id,
                    ReceiverId = previousAssigneeId,
                    Message = $"This ticket has been reassigned to {user.FirstName} {user.LastName}."

                });
            }


            notifications.Add(new SendNotificationDto
            {
                TicketId = ticket.Id,
                ReceiverId = ticket.AssignedToId,
                Message = previousAssigneeId == null
                    ? $"You have been assigned a new support ticket. Subject: {ticket.Subject}."
                    : $"This ticket has been reassigned to you. Subject: {ticket.Subject}."
            });


            notifications.Add(new SendNotificationDto
            {
                TicketId = ticket.Id,
                ReceiverId = ticket.CustomerId,
                Message = previousAssigneeId == null
                    ? $"The ticket you created has been assigned to {user.FirstName} {user.LastName}."
                    : $"The ticket you created has been reassigned to {user.FirstName} {user.LastName}."
            });

            await _notificationService.SendManyAsync(notifications);
        }



        // Change status of ticket
        // Only the user assigned to the ticket can perform this task
        public async Task ChangeTicketStatusAsync(int id, ChangeTicketStatusDto dto)
        {
            // Validate the dto
            var validator = new ChangeTicketStatusValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid ticket entity. ", validationResult);
            }


            var ticket = await GetTicketOrThrowAsync(id);


            if (ticket.AssignedToId != _currentUserService.UserId)
            {
                throw new ForbiddenException(
                    "You can only change the status of a ticket assigned to you.");
            }


            // Prevent the status change of a closed ticket
            if (ticket.Status == TicketStatus.Closed)
            {
                throw new BadRequestException("You cannot change the status of a closed ticket.");
            }



            // Prevent closing a ticket that has not been resolved
            if (dto.Status == TicketStatus.Closed && ticket.Status != TicketStatus.Resolved)
            {
                throw new BadRequestException("A ticket can only be closed after it has been resolved.");
            }


            ticket.Status = dto.Status;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _ticketRepository.SaveChangesAsync();


            // Notify the customer about the new ticket status

            var notification = new SendNotificationDto
            {
                TicketId = ticket.Id,
                ReceiverId = ticket.CustomerId,
                Message = $"The status of your ticket \"{ticket.Subject}\" has been changed to {ticket.Status}."
            };

            await _notificationService.SendAsync(notification);
        }



        // Only users with "SupportManager" role can perform this task
        public async Task DeleteAsync(int id)
        {
            var ticket = await GetTicketOrThrowAsync(id);

            if (ticket.Status != TicketStatus.Closed)
            {
                throw new BadRequestException(
                    "This ticket cannot be deleted because it hasn't been closed yet.");
            }


            List<SendNotificationDto> notifications = new();


            notifications.Add(new SendNotificationDto
            {
                TicketId = ticket.Id,
                ReceiverId = ticket.AssignedToId!,
                Message =  $"The ticket \"{ticket.Subject}\" you were assigned to has been deleted by the support Manager."
            });


            notifications.Add(new SendNotificationDto
            {
                TicketId = ticket.Id,
                ReceiverId = ticket.CustomerId,
                Message = $"The ticket \"{ticket.Subject}\" you created has been deleted by the support Manager."
            });

            await _notificationService.SendManyAsync(notifications);

            await _ticketRepository.DeleteAsync(ticket);
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
    }
}
