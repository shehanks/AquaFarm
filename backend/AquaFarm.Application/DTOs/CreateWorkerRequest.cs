using AquaFarm.Infrastructure.Entities;
using System.ComponentModel.DataAnnotations;

namespace AquaFarm.Application.DTOs
{
    public class CreateWorkerRequest
    {
        [Required]
        public string Name { get; set; } = null!;

        public string? Picture { get; set; }

        [Required]
        [Range(18, 100)]
        public int Age { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public WorkerPosition Position { get; set; }

        [Required]
        public DateTime CertifiedUntil { get; set; }

        [Required]
        public int FishFarmId { get; set; }
    }
}
