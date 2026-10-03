using RealTimeSupportChat.Domain.Common;

namespace RealTimeSupportChat.Domain
{
    public class Attachment : BaseEntity
    {
        public int MessageId { get; set; }
        public Message? Message { get; set; }

        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }
    }
}
