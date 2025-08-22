using AquaFarm.Application.CustomExceptions;
using AquaFarm.Application.DTOs;
using AquaFarm.Application.Services.Contracts;
using AquaFarm.Infrastructure.UnitOfWork;
using AutoMapper;

namespace AquaFarm.Application.Services
{
    public class FishFarmService : IFishFarmService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public FishFarmService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FishFarmDto> CreateFishFarmAsync(CreateFishFarmRequest request)
        {
            try
            {
                var fishFarmEntity = _mapper.Map<Infrastructure.Entities.FishFarm>(request);

                await _unitOfWork.FishFarmRepository.InsertAsync(fishFarmEntity);
                await _unitOfWork.CompleteAsync();

                return _mapper.Map<FishFarmDto>(fishFarmEntity);
            }
            catch (AquaFarmException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AquaFarmException(
                    action: "CREATE_FISHFARM", 
                    statusCode: 500,
                    ex.Message, ex);
            }  
        }

        public async Task<IEnumerable<FishFarmDto>> GetFishFarmsAsync(int skip = 0, int take = 10)
        {
            try
            {
                var fishFarms = await _unitOfWork.FishFarmRepository.QueryAsync(
                    skip: skip,
                    take: take
                );

                return _mapper.Map<IEnumerable<FishFarmDto>>(fishFarms);
            }
            catch (AquaFarmException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AquaFarmException(
                    action: "GET_FISHFARMS",
                    statusCode: 500,
                    ex.Message, ex);
            }
        }
    }
}
