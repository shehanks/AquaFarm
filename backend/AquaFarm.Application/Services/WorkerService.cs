using AquaFarm.Application.CustomExceptions;
using AquaFarm.Application.DTOs;
using AquaFarm.Application.Services.Contracts;
using AquaFarm.Infrastructure.UnitOfWork;
using AutoMapper;

namespace AquaFarm.Application.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public WorkerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<WorkerDto> CreateWorkerAsync(CreateWorkerRequest request)
        {
            try
            {
                var workerEntity = _mapper.Map<Infrastructure.Entities.Worker>(request);

                await _unitOfWork.WorkerRepository.InsertAsync(workerEntity);
                await _unitOfWork.CompleteAsync();

                return _mapper.Map<WorkerDto>(workerEntity);
            }
            catch (AquaFarmException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AquaFarmException(
                    action: "CREATE_WORKER",
                    statusCode: 500,
                    ex.Message,
                    innerException: ex);
            }
        }

        public async Task<IEnumerable<WorkerDto>> GetWorkersByFishFarmAsync(int fishFarmId, int skip = 0, int take = 10)
        {
            try
            {
                var workers = await _unitOfWork.WorkerRepository.QueryAsync(
                filter: w => w.FishFarmId == fishFarmId,
                skip: skip,
                take: take
            );

                return _mapper.Map<IEnumerable<WorkerDto>>(workers);
            }
            catch (AquaFarmException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AquaFarmException(
                    action: "GET_WORKER_BY_FISHFARM",
                    statusCode: 500,
                    ex.Message,
                    innerException: ex);
            }
        }
    }
}
