using AutoMapper;
using RealTimeSupportChat.Application.DTOs.Attachment;
using RealTimeSupportChat.Domain;

namespace RealTimeSupportChat.Application.MappingProfiles
{
    public class AttachmentProfile : Profile
    {
        public AttachmentProfile()
        {
            CreateMap<Attachment, GetAttachmentDto>();
        }
    }
}
