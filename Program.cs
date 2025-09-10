using Microsoft.EntityFrameworkCore;
using AccessibilityMcpServer.Data;
using ModelContextProtocol.Server;

var builder = WebApplication.CreateBuilder(args);

// 配置日志
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// 配置数据库 - 使用内存数据库用于演示
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("AccessibilityDb")
           .EnableSensitiveDataLogging()
           .EnableDetailedErrors());

// 可选：使用SQL Server数据库（需要配置连接字符串）
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// if (!string.IsNullOrEmpty(connectionString))
// {
//     builder.Services.AddDbContext<ApplicationDbContext>(options =>
//         options.UseSqlServer(connectionString));
// }

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

// 配置MCP服务器
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithTools<AccessibilityMcpServer.Tools.AccessibilityTools>();

var app = builder.Build();

// 确保数据库已创建并填充种子数据
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

// 配置HTTP请求管道
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

// 配置MCP端点
app.MapMcpServer();

// 添加健康检查端点
app.MapGet("/", () => "无障碍设施 MCP 服务器正在运行");
app.MapGet("/health", () => "OK");

app.Run();