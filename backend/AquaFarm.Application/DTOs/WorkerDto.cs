namespace AquaFarm.Application.DTOs
{
    public class WorkerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Picture { get; set; }
        public int Age { get; set; }
        public string Email { get; set; } = null!;
        public string Position { get; set; } = null!;
        public DateTime CertifiedUntil { get; set; }
    }
}
