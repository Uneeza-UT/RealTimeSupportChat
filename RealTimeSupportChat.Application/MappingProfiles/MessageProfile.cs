using AutoMapper;
using RealTimeSupportChat.Application.DTOs.Message;
using RealTimeSupportChat.Domain;

namespace RealTimeSupportChat.Application.MappingProfiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, GetMessageDto>();
        }
    }
}
