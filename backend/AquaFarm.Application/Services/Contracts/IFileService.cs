namespace AquaFarm.Application.Services.Contracts
{
    public interface IFileService
    {
        Task<string> UploadImageAsync(Stream content, string? fileName = null);
    }
}
