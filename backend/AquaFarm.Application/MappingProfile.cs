using AquaFarm.Application.DTOs;
using AquaFarm.Infrastructure.Entities;
using AutoMapper;

namespace AquaFarm.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Create FishFarm mappings
            CreateMap<CreateFishFarmRequest, FishFarm>();
            CreateMap<FishFarm, FishFarmDto>();

            // Create Worker mappings
            CreateMap<CreateWorkerRequest, Worker>();
            CreateMap<Worker, WorkerDto>();
        }
    }
}
