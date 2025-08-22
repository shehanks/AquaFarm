namespace AquaFarm.Application.DTOs
{
    public class FishFarmDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal GPSLatitude { get; set; }
        public decimal GPSLongitude { get; set; }
        public int NumOfCages { get; set; }
        public bool HasBarge { get; set; }
        public string? Picture { get; set; }

        public List<WorkerDto> Workers { get; set; } = new();
    }
}
