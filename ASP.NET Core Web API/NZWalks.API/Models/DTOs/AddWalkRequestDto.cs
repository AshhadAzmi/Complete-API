using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTOs
{
    public class AddWalkRequestDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot be more than 100 characters.")]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000, ErrorMessage = "Note more than 1000 characters.")]
        public string Description { get; set; }
        [Required]
        [Range(0,50, ErrorMessage = "Walk van't be more than 50Kms.")]
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionId { get; set; }
    }
}
