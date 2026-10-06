using RealTimeSupportChat.Application.DTOs.Ticket;

namespace RealTimeSupportChat.Application.Contracts.Services
{
    public interface ITicketService
    {
        Task<List<GetTicketDto>> GetAsync();
        Task<GetTicketDto> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateTicketDto dto);
        Task AssignTicketAsync(int id, AssignTicketDto dto);
        Task ChangeTicketStatusAsync(int id, ChangeTicketStatusDto dto);
        Task DeleteAsync(int id);
    }
}
