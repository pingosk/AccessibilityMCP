using ModelContextProtocol.Server;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using AccessibilityMcpServer.Data;
using AccessibilityMcpServer.Models;

namespace AccessibilityMcpServer.Tools
{
    [McpServerToolType]
    public class AccessibilityTools
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccessibilityTools> _logger;

        public AccessibilityTools(ApplicationDbContext context, ILogger<AccessibilityTools> logger)
        {
            _context = context;
            _logger = logger;
        }

        [McpServerTool, Description("搜索无障碍景点")]
        public async Task<string> SearchAccessibleAttractions(
            [Description("城市名称（可包含或不包含'市'字）")] string city,
            [Description("是否需要无障碍坡道（可选）")] bool? hasAccessibleRamp = null,
            [Description("是否需要无障碍厕所（可选）")] bool? hasAccessibleToilet = null,
            [Description("是否需要电梯（可选）")] bool? hasElevator = null,
            [Description("是否需要无障碍船只（可选）")] bool? hasAccessibleBoat = null,
            [Description("是否需要无障碍观光车（可选）")] bool? hasAccessibleTour = null,
            [Description("返回结果数量限制，默认10")] int limit = 10)
        {
            try
            {
                city = city.Replace("市", "");
                var query = _context.Facilities
                    .Include(f => f.Category)
                    .Include(f => f.Images)
                    .Where(f => f.IsActive && f.City.Contains(city));

                // 根据无障碍设施参数筛选
                if (hasAccessibleRamp.HasValue)
                {
                    query = query.Where(f => f.HasAccessibleRamp == (hasAccessibleRamp.Value ? 1 : 0));
                }
                if (hasAccessibleToilet.HasValue)
                {
                    query = query.Where(f => f.HasAccessibleToilet == (hasAccessibleToilet.Value ? 1 : 0));
                }
                if (hasElevator.HasValue)
                {
                    query = query.Where(f => f.HasElevator == (hasElevator.Value ? 1 : 0));
                }
                if (hasAccessibleBoat.HasValue)
                {
                    query = query.Where(f => f.HasAccessibleBoat == (hasAccessibleBoat.Value ? 1 : 0));
                }
                if (hasAccessibleTour.HasValue)
                {
                    query = query.Where(f => f.HasAccessibleTour == (hasAccessibleTour.Value ? 1 : 0));
                }
                
                // 如果没有指定任何无障碍设施参数，则只查询有无障碍设施的景点
                if (!hasAccessibleRamp.HasValue && !hasAccessibleToilet.HasValue && !hasElevator.HasValue && 
                    !hasAccessibleBoat.HasValue && !hasAccessibleTour.HasValue)
                {
                    query = query.Where(f => 
                        f.HasAccessibleRamp == 1 || 
                        f.HasAccessibleToilet == 1 || 
                        f.HasElevator == 1 || 
                        f.HasAccessibleBoat == 1 || 
                        f.HasAccessibleTour == 1);
                }

                var attractions = await query
                    .Select(f => new
                    {
                        f.Name,
                        f.City,
                        Category = f.Category != null ? f.Category.Name : "未分类",
                        f.Latitude,
                        f.Longitude,
                        AccessibilityFeatures = new
                        {
                            HasAccessibleRamp = f.HasAccessibleRamp == 1,
                            HasAccessibleToilet = f.HasAccessibleToilet == 1,
                            HasElevator = f.HasElevator == 1,
                            HasAccessibleBoat = f.HasAccessibleBoat == 1,
                            HasAccessibleTour = f.HasAccessibleTour == 1
                        },
                        Images = f.Images.Select(i => i.ImageUrl).ToList()
                    })
                    .OrderByDescending(f => f.Name)
                    .Take(limit)
                    .ToListAsync();

                return System.Text.Json.JsonSerializer.Serialize(new
                {
                    success = true,
                    count = attractions.Count,
                    data = attractions,
                    query = new { city, hasAccessibleRamp, hasAccessibleToilet, hasElevator, hasAccessibleBoat, hasAccessibleTour, limit }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "搜索无障碍景点时发生错误");
                return System.Text.Json.JsonSerializer.Serialize(new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }

        [McpServerTool, Description("搜索无障碍酒店")]
        public async Task<string> SearchAccessibleHotels(
            [Description("城市名称")] string city,
            [Description("是否需要无障碍通道（可选）")] bool? hasAccessiblePassage = null,
            [Description("是否需要无障碍淋浴（可选）")] bool? hasAccessibleShower = null,
            [Description("是否需要淋浴座椅（可选）")] bool? hasShowerSeat = null,
            [Description("返回结果数量限制，默认10，最大不超过50")] int limit = 10)
        {
            try
            {
                city = city.Replace("市", "");
                var query = _context.Facilities
                    .Include(f => f.Category)
                    .Include(f => f.Images)
                    .Where(f => f.IsActive && f.City.Contains(city));
 
                // 筛选酒店类别
                query = query.Where(f => f.Category != null && f.Category.Name.Contains("酒店"));

                // 根据无障碍设施参数筛选
                if (hasAccessiblePassage.HasValue)
                {
                    query = query.Where(f => f.Hotel_AccessiblePassage == hasAccessiblePassage.Value);
                }
                if (hasAccessibleShower.HasValue)
                {
                    query = query.Where(f => f.Hotel_AccessibleShower == hasAccessibleShower.Value);
                }
                if (hasShowerSeat.HasValue)
                {
                    query = query.Where(f => f.Hotel_ShowerSeat == hasShowerSeat.Value);
                }
                
                // 如果没有指定任何无障碍设施参数，则只查询有无障碍设施的酒店
                if (!hasAccessiblePassage.HasValue && !hasAccessibleShower.HasValue && !hasShowerSeat.HasValue)
                {
                    query = query.Where(f => 
                        f.Hotel_AccessiblePassage || 
                        f.Hotel_AccessibleShower || 
                        f.Hotel_ShowerSeat);
                }

                var hotels = await query
                    .Select(f => new
                    {
                        f.Name,
                        f.City,
                        Category = f.Category != null ? f.Category.Name : "未分类",
                        f.Latitude,
                        f.Longitude,
                        AccessibilityFeatures = new
                        {
                            HasAccessiblePassage = f.Hotel_AccessiblePassage,
                            HasAccessibleShower = f.Hotel_AccessibleShower,
                            HasShowerSeat = f.Hotel_ShowerSeat
                        },
                        Images = f.Images.Select(i => i.ImageUrl).ToList()
                    })
                    .OrderByDescending(f => f.Name)
                    .Take(limit)
                    .ToListAsync();

                return System.Text.Json.JsonSerializer.Serialize(new
                {
                    success = true,
                    count = hotels.Count,
                    data = hotels,
                    query = new { city, hasAccessiblePassage, hasAccessibleShower, hasShowerSeat, limit }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "搜索无障碍酒店时发生错误");
                return System.Text.Json.JsonSerializer.Serialize(new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }
    }
}