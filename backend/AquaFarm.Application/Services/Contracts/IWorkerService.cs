using AquaFarm.Application.DTOs;

namespace AquaFarm.Application.Services.Contracts
{
    public interface IWorkerService
    {
        Task<WorkerDto> CreateWorkerAsync(CreateWorkerRequest request);

        Task<IEnumerable<WorkerDto>> GetWorkersByFishFarmAsync(int fishFarmId, int skip = 0, int take = 10);
    }
}
