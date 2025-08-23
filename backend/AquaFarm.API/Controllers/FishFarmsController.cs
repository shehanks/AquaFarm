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

        public FishFarmsController(IFishFarmService fishFarmService)
        {
            _fishFarmService = fishFarmService;
        }

        [HttpPost]
        public async Task<ActionResult<FishFarmDto>> CreateFishFarm([FromBody] CreateFishFarmRequest request)
        {
            var createdFishFarm = await _fishFarmService.CreateFishFarmAsync(request);
            return CreatedAtAction(nameof(CreateFishFarm), new { id = createdFishFarm.Id }, createdFishFarm);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FishFarmDto>>> GetFishFarms([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var fishFarms = await _fishFarmService.GetFishFarmsAsync(skip, take);
            return Ok(fishFarms);
        }
    }
}
