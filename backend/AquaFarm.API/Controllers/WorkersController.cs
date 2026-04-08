using AquaFarm.Application.DTOs;
using AquaFarm.Application.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AquaFarm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkersController : ControllerBase
    {
        private readonly IWorkerService _workerService;

        public WorkersController(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateWorker([FromBody] CreateWorkerRequest request)
        {
            var worker = await _workerService.CreateWorkerAsync(request);
            return CreatedAtAction(nameof(CreateWorker), new { id = worker.Id }, worker);
        }

        [HttpPost("{workerId}/upload-image")]
        public async Task<IActionResult> UploadImage(int workerId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();
            var url = await _workerService.UploadWorkerImageAsync(workerId, stream, file.FileName);
            await Task.Yield();

            return Ok(new { url });
        }
    }
}
