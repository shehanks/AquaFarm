namespace AquaFarm.Infrastructure.Entities
{
    public class FishFarm
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal GPSLatitude { get; set; }
        public decimal GPSLongitude { get; set; }
        public int NumOfCages { get; set; }
        public bool HasBarge { get; set; }
        public string? Picture { get; set; }

        // Navigation
        public ICollection<Worker> Workers { get; set; } = new List<Worker>();
    }
}
