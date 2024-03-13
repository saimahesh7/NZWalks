using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTOs
{
    public class CreateRegionDto
    {
        [Required]
        [MinLength(3,ErrorMessage ="Code has to be Minimum of 3 characters")]
        [MaxLength(3,ErrorMessage ="Code has to be Maximum of 3 characters")]
       // [Range(3, 3)]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Code has to be Maximum of 100 characters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
