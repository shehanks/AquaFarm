using AquaFarm.Infrastructure.Data;
using AquaFarm.Infrastructure.Entities;
using AquaFarm.Infrastructure.Repositories.Contracts;

namespace AquaFarm.Infrastructure.Repositories
{
    public class FishFarmRepository : RepositoryBase<FishFarm>, IFishFarmRepository
    {
        public FishFarmRepository(AquaFarmDbContext context) 
            : base(context)
        {
        }
    }
}
