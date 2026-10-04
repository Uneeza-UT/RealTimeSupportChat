using Microsoft.AspNetCore.Http;

namespace RealTimeSupportChat.Application.Contracts.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file);
        Task DeleteFilesAsync(List<string> filePaths);
    }
}
