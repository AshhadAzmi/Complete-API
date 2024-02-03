using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTOs
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot be more than 100 Characters.")]
        public string Name { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Code is less than 3 Characters, It should be 3 Characters Only")]
        [MaxLength(3, ErrorMessage = "Code is more than 3 Characters, It should be 3 Characters Only.")]
        public string Code { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
