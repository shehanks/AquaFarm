using AquaFarm.Infrastructure.Data;
using AquaFarm.Infrastructure.Repositories;
using AquaFarm.Infrastructure.Repositories.Contracts;

namespace AquaFarm.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IAsyncDisposable
    {
        private readonly AquaFarmDbContext _context;

        public IFishFarmRepository FishFarmRepository { get; }

        public IWorkerRepository WorkerRepository { get; }


        public UnitOfWork(
            AquaFarmDbContext context, 
            IFishFarmRepository fishFarmRepository, 
            IWorkerRepository workerRepository)
        {
            _context = context;
            FishFarmRepository = new FishFarmRepository(_context);
            WorkerRepository = new WorkerRepository(_context);
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public async ValueTask DisposeAsync()
        {
            if (_context != null)
                await _context.DisposeAsync();
        }
    }
}
