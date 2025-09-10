using System.ComponentModel.DataAnnotations;

namespace AccessibilityMcpServer.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // 导航属性
        public virtual ICollection<Facility> Facilities { get; set; } = new List<Facility>();
    }
}