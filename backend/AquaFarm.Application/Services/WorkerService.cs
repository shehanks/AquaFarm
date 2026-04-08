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

        private readonly IFileService _fileService;

        public WorkerService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
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

        public async Task<PaginatedResponse<WorkerDto>> GetWorkersByFishFarmAsync(int fishFarmId, int skip = 0, int take = 10)
        {
            try
            {
                var workers = await _unitOfWork.WorkerRepository.QueryAsync(
                    filter: w => w.FishFarmId == fishFarmId
                );

                var totalCount = workers.Count();

                workers = workers
                    .Skip(skip)
                    .Take(take);

                var items = _mapper.Map<IEnumerable<WorkerDto>>(workers);

                return new PaginatedResponse<WorkerDto>
                {
                    Items = items,
                    Meta = new()
                    {
                        TotalCount = totalCount,
                        PageSize = take,
                        Page = (skip / take) + 1
                    }
                };
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

        public async Task<string> UploadWorkerImageAsync(int workerId, Stream stream, string fileName)
        {
            try
            {
                var fishFarm = await _unitOfWork.WorkerRepository.GetByIdAsync(workerId);
                if (fishFarm == null)
                    throw new ApplicationException($"Fish farm not found. Action: UPLOAD_WORKER_IMAGE");

                var url = await _fileService.UploadImageAsync(stream, fileName);
                fishFarm.Picture = url;
                await _unitOfWork.CompleteAsync();

                return url;
            }
            catch (AquaFarmException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AquaFarmException(
                    action: "UPLOAD_WORKER_IMAGE",
                    statusCode: 500,
                    ex.Message,
                    innerException: ex);
            }
        }
    }
}
