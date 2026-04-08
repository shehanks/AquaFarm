using AquaFarm.Infrastructure.Repositories.Contracts;

namespace AquaFarm.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IFishFarmRepository FishFarmRepository { get; }

        IWorkerRepository WorkerRepository { get; }

        Task<int> CompleteAsync();
    }
}
