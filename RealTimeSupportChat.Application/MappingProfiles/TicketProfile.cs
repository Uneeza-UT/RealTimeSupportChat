using AutoMapper;
using RealTimeSupportChat.Application.DTOs.Ticket;
using RealTimeSupportChat.Domain;

namespace RealTimeSupportChat.Application.MappingProfiles
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, GetTicketDto>();
            CreateMap<CreateTicketDto, Ticket>();
        }
    }
}
