namespace AquaFarm.Infrastructure.Entities
{
    public class Worker
    {
        public int Id { get; set; }
        public int FishFarmId { get; set; }
        public string Name { get; set; } = null!;
        public string? Picture { get; set; }
        public int Age { get; set; }
        public string Email { get; set; } = null!;
        public WorkerPosition Position { get; set; }
        public DateTime CertifiedUntil { get; set; }

        // Navigation
        public FishFarm FishFarm { get; set; } = null!;
    }
}
