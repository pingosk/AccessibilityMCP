# 无障碍设施 MCP 服务器

这是一个基于 Model Context Protocol (MCP) 的无障碍设施查询服务器，提供景点和酒店的无障碍设施信息查询功能。

## 功能特性

- 🏛️ **无障碍景点搜索**: 根据城市和无障碍设施类型搜索景点
- 🏨 **无障碍酒店搜索**: 根据城市和无障碍设施类型搜索酒店
- 🔍 **多维度筛选**: 支持坡道、厕所、电梯、船只、观光车等设施筛选
- 📊 **结构化数据**: 返回标准化的JSON格式数据

## 安装和使用

### 环境要求

- .NET 8.0 或更高版本
- SQL Server 数据库（可选，支持内存数据库用于演示）

### 快速开始

1. 克隆项目
```bash
git clone <repository-url>
cd AccessibilityMcpServer
```

2. 安装依赖
```bash
dotnet restore
```

3. 运行服务器
```bash
dotnet run
```

## MCP 工具

### SearchAccessibleAttractions
搜索无障碍景点

**参数:**
- `city` (string): 城市名称
- `hasAccessibleRamp` (bool?, 可选): 是否需要无障碍坡道
- `hasAccessibleToilet` (bool?, 可选): 是否需要无障碍厕所
- `hasElevator` (bool?, 可选): 是否需要电梯
- `hasAccessibleBoat` (bool?, 可选): 是否需要无障碍船只
- `hasAccessibleTour` (bool?, 可选): 是否需要无障碍观光车
- `limit` (int, 默认10): 返回结果数量限制

### SearchAccessibleHotels
搜索无障碍酒店

**参数:**
- `city` (string): 城市名称
- `hasAccessiblePassage` (bool?, 可选): 是否需要无障碍通道
- `hasAccessibleShower` (bool?, 可选): 是否需要无障碍淋浴
- `hasShowerSeat` (bool?, 可选): 是否需要淋浴座椅
- `limit` (int, 默认10): 返回结果数量限制

## 许可证

MIT License

## 贡献

欢迎提交 Issue 和 Pull Request！