using Microsoft.EntityFrameworkCore;
using AccessibilityMcpServer.Models;

namespace AccessibilityMcpServer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FacilityImage> FacilityImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 配置实体关系
            modelBuilder.Entity<Facility>()
                .HasOne(f => f.Category)
                .WithMany(c => c.Facilities)
                .HasForeignKey(f => f.CategoryId);

            modelBuilder.Entity<FacilityImage>()
                .HasOne(fi => fi.Facility)
                .WithMany(f => f.Images)
                .HasForeignKey(fi => fi.FacilityId);

            // 种子数据
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "景点", Description = "旅游景点", IsActive = true },
                new Category { Id = 2, Name = "酒店", Description = "住宿酒店", IsActive = true },
                new Category { Id = 3, Name = "餐厅", Description = "餐饮场所", IsActive = true }
            );

            // 示例设施数据
            modelBuilder.Entity<Facility>().HasData(
                new Facility
                {
                    Id = 1,
                    Name = "故宫博物院",
                    Province = "北京市",
                    City = "北京",
                    District = "东城区",
                    Latitude = 39.9163,
                    Longitude = 116.3972,
                    CategoryId = 1,
                    IsActive = true,
                    HasAccessibleRamp = 1,
                    HasAccessibleToilet = 1,
                    HasElevator = 1
                },
                new Facility
                {
                    Id = 2,
                    Name = "北京饭店",
                    Province = "北京市",
                    City = "北京",
                    District = "东城区",
                    Latitude = 39.9042,
                    Longitude = 116.4074,
                    CategoryId = 2,
                    IsActive = true,
                    Hotel_AccessiblePassage = true,
                    Hotel_AccessibleShower = true,
                    Hotel_ShowerSeat = true
                }
            );
        }
    }
}