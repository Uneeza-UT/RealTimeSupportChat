using RealTimeSupportChat.Application.DTOs.Ticket;

namespace RealTimeSupportChat.Application.Contracts.Services
{
    public interface ITicketService
    {
        Task<List<GetTicketDto>> GetAsync();
        Task<GetTicketDto> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateTicketDto dto);
        Task AssignTicketAsync(AssignTicketDto dto);
        Task ChangeTicketStatusAsync(ChangeTicketStatusDto dto);
        Task DeleteAsync(int id);
    }
}
