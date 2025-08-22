using AquaFarm.Application.DTOs;

namespace AquaFarm.Application.Services.Contracts
{
    public interface IFishFarmService
    {
        Task<FishFarmDto> CreateFishFarmAsync(CreateFishFarmRequest request);

        Task<IEnumerable<FishFarmDto>> GetFishFarmsAsync(int skip = 0, int take = 10);
    }
}
