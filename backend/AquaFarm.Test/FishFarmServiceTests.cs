using AquaFarm.Application.CustomExceptions;
using AquaFarm.Application.DTOs;
using AquaFarm.Application.Services;
using AquaFarm.Infrastructure.Entities;
using AquaFarm.Infrastructure.UnitOfWork;
using AutoMapper;
using Moq;
using System.Linq.Expressions;

namespace AquaFarm.Test
{
    public class FishFarmServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private readonly IMapper _mapper;

        private readonly FishFarmService _service;

        public FishFarmServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateFishFarmRequest, FishFarm>();
                cfg.CreateMap<FishFarm, FishFarmDto>();
            });

            _mapper = config.CreateMapper();
            _service = new FishFarmService(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task CreateFishFarmAsync_ShouldReturnDto_WhenSuccessful()
        {
            // Arrange
            var request = new CreateFishFarmRequest { Name = "North West X1" };
            _unitOfWorkMock.Setup(u => u.FishFarmRepository.InsertAsync(It.IsAny<FishFarm>()))
                           .ReturnsAsync((FishFarm f) => f);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.CreateFishFarmAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Name, result.Name);
            _unitOfWorkMock.Verify(u => u.FishFarmRepository.InsertAsync(It.IsAny<FishFarm>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateFishFarmAsync_ShouldThrowAquaFarmException_WhenGenericExceptionOccurs()
        {
            // Arrange
            var request = new CreateFishFarmRequest { Name = "North West X1" };
            _unitOfWorkMock.Setup(u => u.FishFarmRepository.InsertAsync(It.IsAny<FishFarm>()))
                           .ThrowsAsync(new Exception("DB failed"));

            // Act
            var ex = await Assert.ThrowsAsync<AquaFarmException>(() => _service.CreateFishFarmAsync(request));

            // Assert
            Assert.Equal("CREATE_FISHFARM", ex.Action);
            Assert.Equal(500, ex.StatusCode);
        }

        [Fact]
        public async Task GetFishFarmsAsync_ShouldReturnList_WhenSuccessful()
        {
            // Arrange
            var fishFarms = new List<FishFarm>
            {
                new FishFarm { Name = "South West X1" },
                new FishFarm { Name = "North West X2" }
            };

            _unitOfWorkMock.Setup(u => u.FishFarmRepository.QueryAsync(
                    It.IsAny<Expression<Func<FishFarm, bool>>>(),
                    null, 0, 10))
                .ReturnsAsync(fishFarms);

            // Act
            var result = await _service.GetFishFarmsAsync(0, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<FishFarmDto>)result).Count);
        }

        [Fact]
        public async Task GetFishFarmsAsync_ShouldThrowAquaFarmException_WhenGenericExceptionOccurs()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.FishFarmRepository.QueryAsync(
                    It.IsAny<Expression<Func<FishFarm, bool>>>(),
                    null, 0, 10))
                .ThrowsAsync(new Exception("DB failed"));

            // Act
            var ex = await Assert.ThrowsAsync<AquaFarmException>(() => _service.GetFishFarmsAsync(0, 10));

            // Assert
            Assert.Equal("GET_FISHFARMS", ex.Action);
            Assert.Equal(500, ex.StatusCode);
        }

    }
}