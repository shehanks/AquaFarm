using AquaFarm.Infrastructure.Entities;

namespace AquaFarm.Application.DTOs
{
    public class WorkerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Picture { get; set; }
        public int Age { get; set; }
        public string Email { get; set; } = null!;
        public WorkerPosition Position { get; set; }
        public DateTime CertifiedUntil { get; set; }
    }
}
