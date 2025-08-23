using AquaFarm.Application.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AquaFarm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();
            var url = await _fileService.UploadImageAsync(stream, file.FileName);

            return Ok(new { url });
        }
    }
}
