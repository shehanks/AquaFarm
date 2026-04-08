using AquaFarm.Application.DTOs;
using AquaFarm.Application.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AquaFarm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FishFarmsController : ControllerBase
    {
        private readonly IFishFarmService _fishFarmService;

        private readonly IWorkerService _workerService;

        private readonly IFileService _fileService;

        public FishFarmsController(
            IFishFarmService fishFarmService,
            IWorkerService workerService,
            IFileService fileService)
        {
            _fishFarmService = fishFarmService;
            _workerService = workerService;
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateFishFarm([FromBody] CreateFishFarmRequest request)
        {
            var createdFishFarm = await _fishFarmService.CreateFishFarmAsync(request);
            return CreatedAtAction(nameof(CreateFishFarm), new { id = createdFishFarm.Id }, createdFishFarm);
        }

        [HttpGet]
        public async Task<ActionResult> GetFishFarms([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var fishFarms = await _fishFarmService.GetFishFarmsAsync(skip, take);
            return Ok(fishFarms);
        }

        [HttpGet("{fishFarmId}/workers")]
        public async Task<ActionResult> GetWorkers(int fishFarmId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var workers = await _workerService.GetWorkersByFishFarmAsync(fishFarmId, skip, take);
            return Ok(workers);
        }

        [HttpPost("{fishFarmId}/upload-image")]
        public async Task<IActionResult> UploadImage(int fishFarmId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();
            var url = await _fishFarmService.UploadFishFarmImageAsync(fishFarmId, stream, file.FileName);

            return Ok(new { url });
        }
    }
}
