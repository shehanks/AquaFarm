using AquaFarm.Application.CustomExceptions;
using AquaFarm.Application.DTOs;
using AquaFarm.Application.Services;
using AquaFarm.Infrastructure.Entities;
using AquaFarm.Infrastructure.UnitOfWork;
using AutoMapper;
using Moq;

namespace AquaFarm.Test
{
    public class WorkerServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;
        private readonly WorkerService _service;

        public WorkerServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateWorkerRequest, Worker>();
                cfg.CreateMap<Worker, WorkerDto>();
            });

            _mapper = config.CreateMapper();
            _service = new WorkerService(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task CreateWorkerAsync_ShouldReturnDto_WhenSuccessful()
        {
            // Arrange
            var request = new CreateWorkerRequest { Name = "Tom Hardy", FishFarmId = 1 };
            _unitOfWorkMock.Setup(u => u.WorkerRepository.InsertAsync(It.IsAny<Worker>()))
                           .ReturnsAsync((Worker w) => w);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.CreateWorkerAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Name, result.Name);
            _unitOfWorkMock.Verify(u => u.WorkerRepository.InsertAsync(It.IsAny<Worker>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateWorkerAsync_ShouldThrowAquaFarmException_WhenGenericExceptionOccurs()
        {
            // Arrange
            var request = new CreateWorkerRequest { Name = "Tom Hardy", FishFarmId = 1 };
            _unitOfWorkMock.Setup(u => u.WorkerRepository.InsertAsync(It.IsAny<Worker>()))
                           .ThrowsAsync(new Exception("DB failed"));

            // Act
            var ex = await Assert.ThrowsAsync<AquaFarmException>(() => _service.CreateWorkerAsync(request));

            // Assert
            Assert.Equal("CREATE_WORKER", ex.Action);
            Assert.Equal(500, ex.StatusCode);
        }

        [Fact]
        public async Task GetWorkersByFishFarmAsync_ShouldReturnList_WhenSuccessful()
        {
            // Arrange
            var workers = new List<Worker>
            {
                new Worker { Name = "Liam Neeson", FishFarmId = 1 },
                new Worker { Name = "Tom Hanks", FishFarmId = 1 }
            };

            _unitOfWorkMock.Setup(u => u.WorkerRepository.QueryAsync(
                    w => w.FishFarmId == 1, null, 0, 10))
                .ReturnsAsync(workers);

            // Act
            var result = await _service.GetWorkersByFishFarmAsync(1, 0, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<WorkerDto>)result).Count);
        }

        [Fact]
        public async Task GetWorkersByFishFarmAsync_ShouldThrowAquaFarmException_WhenGenericExceptionOccurs()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.WorkerRepository.QueryAsync(
                    w => w.FishFarmId == 1, null, 0, 10))
                .ThrowsAsync(new Exception("DB failed"));

            // Act
            var ex = await Assert.ThrowsAsync<AquaFarmException>(() => _service.GetWorkersByFishFarmAsync(1, 0, 10));

            // Assert
            Assert.Equal("GET_WORKER_BY_FISHFARM", ex.Action);
            Assert.Equal(500, ex.StatusCode);
        }
    }
}
