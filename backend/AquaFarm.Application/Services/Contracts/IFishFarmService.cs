using AquaFarm.Application.DTOs;

namespace AquaFarm.Application.Services.Contracts
{
    public interface IFishFarmService
    {
        Task<FishFarmDto> CreateFishFarmAsync(CreateFishFarmRequest request);

        Task<PaginatedResponse<FishFarmDto>> GetFishFarmsAsync(int skip = 0, int take = 10);

        Task<string> UploadFishFarmImageAsync(int fishFarmId, Stream stream, string? fileName = null);
    }
}
