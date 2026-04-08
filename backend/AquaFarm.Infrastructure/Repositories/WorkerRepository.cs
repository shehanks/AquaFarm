using AquaFarm.Infrastructure.Data;
using AquaFarm.Infrastructure.Entities;
using AquaFarm.Infrastructure.Repositories.Contracts;

namespace AquaFarm.Infrastructure.Repositories
{
    public class WorkerRepository : RepositoryBase<Worker>, IWorkerRepository
    {
        public WorkerRepository(AquaFarmDbContext context)
            : base(context)
        {
        }
    }
}
