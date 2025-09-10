using System.ComponentModel.DataAnnotations;

namespace AccessibilityMcpServer.Models
{
    public class Facility
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string Province { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string City { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string District { get; set; } = string.Empty;
        
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
        
        public bool IsActive { get; set; } = true;
        
        // 无障碍设施字段
        public int HasAccessibleRamp { get; set; } = 0;
        public int HasAccessibleToilet { get; set; } = 0;
        public int HasElevator { get; set; } = 0;
        public int HasAccessibleBoat { get; set; } = 0;
        public int HasAccessibleTour { get; set; } = 0;
        
        // 酒店专属无障碍设施
        public bool Hotel_AccessiblePassage { get; set; } = false;
        public bool Hotel_AccessibleShower { get; set; } = false;
        public bool Hotel_ShowerSeat { get; set; } = false;
        
        // 导航属性
        public virtual ICollection<FacilityImage> Images { get; set; } = new List<FacilityImage>();
    }
}