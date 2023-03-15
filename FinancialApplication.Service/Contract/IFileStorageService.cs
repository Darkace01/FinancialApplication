namespace FinancialApplication.Service.Contract
{
    public interface IFileStorageService
    {
        Task<bool> DeleteFile(string publicId);
        Task<FileStorageDTO> SaveFile(IFormFile file, string tag);
    }
}