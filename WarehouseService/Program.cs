using Microsoft.EntityFrameworkCore;
using WarehouseService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHealthChecks();

builder.Services.AddDbContext<WarehouseDbContext>(options =>
    options.UseNpgsql("Host=postgres;Database=warehouse_db;Username=program;Password=test"));

var app = builder.Build();

app.MapHealthChecks("/manage/health");
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<WarehouseDbContext>();
    db.Database.EnsureCreated();
}

app.Run();