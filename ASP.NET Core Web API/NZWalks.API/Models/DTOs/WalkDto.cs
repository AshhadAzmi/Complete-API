using NZWalks.API.Models.Domains;

namespace NZWalks.API.Models.DTOs
{
    public class WalkDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }

        public RegionDTO Region { get; set; }
        public DifficultyDto Difficulty { get; set; }
    }
}
