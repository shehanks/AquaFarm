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

        private readonly IFileService _fileService;

        public FishFarmService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
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
                    ex.Message,
                    innerException: ex);
            }
        }

        public async Task<PaginatedResponse<FishFarmDto>> GetFishFarmsAsync(int skip = 0, int take = 10)
        {
            try
            {
                var fishFarms = await _unitOfWork.FishFarmRepository.QueryAsync(
                    skip: skip,
                    take: take
                );

                var totalCount = await _unitOfWork.FishFarmRepository.CountAsync();
                var items = _mapper.Map<IEnumerable<FishFarmDto>>(fishFarms);

                return new PaginatedResponse<FishFarmDto>
                {
                    Items = items,
                    Meta = new ()
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
                    action: "GET_FISHFARMS",
                    statusCode: 500,
                    ex.Message,
                    innerException: ex);
            }
        }

        public async Task<string> UploadFishFarmImageAsync(int fishFarmId, Stream content, string? fileName = null)
        {
            try
            {
                var fishFarm = await _unitOfWork.FishFarmRepository.GetByIdAsync(fishFarmId);
                if (fishFarm == null)
                    throw new ApplicationException($"Fish farm not found. Action: UPLOAD_FISHFARM_IMAGE");

                var url = await _fileService.UploadImageAsync(content, fileName);
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
                    action: "UPLOAD_FISHFARM_IMAGE",
                    statusCode: 500,
                    ex.Message,
                    innerException: ex);
            }
        }
    }
}
