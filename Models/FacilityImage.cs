using System.ComponentModel.DataAnnotations;

namespace AccessibilityMcpServer.Models
{
    public class FacilityImage
    {
        public int Id { get; set; }
        
        public int FacilityId { get; set; }
        public virtual Facility Facility { get; set; } = null!;
        
        [Required]
        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}