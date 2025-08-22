using System.ComponentModel.DataAnnotations;

namespace AquaFarm.Application.DTOs
{
    public class CreateFishFarmRequest
    {
        [Required, MaxLength(200)]
        public string Name { get; init; } = null!;

        [Required, Range(-90, 90)]
        public decimal GPSLatitude { get; init; }

        [Required, Range(-180, 180)]
        public decimal GPSLongitude { get; init; }

        [Required]
        public int NumOfCages { get; init; }

        public bool HasBarge { get; init; }

        [MaxLength(500)]
        public string? Picture { get; init; }
    }
}
