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
        public async Task<ActionResult<WorkerDto>> CreateWorker([FromBody] CreateWorkerRequest request)
        {
            var worker = await _workerService.CreateWorkerAsync(request);
            return CreatedAtAction(nameof(CreateWorker), new { id = worker.Id }, worker);
        }
    }
}
