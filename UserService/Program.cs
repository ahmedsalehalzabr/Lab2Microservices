using Microsoft.EntityFrameworkCore;
using UserService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHealthChecks();

// إضافة DbContext مع سلسلة الاتصال
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql("Host=postgres;Database=user_db;Username=program;Password=test"));

var app = builder.Build();

app.MapHealthChecks("/manage/health");
app.MapControllers();

// إنشاء قاعدة البيانات والجداول تلقائيًا عند بدء التشغيل (للتطوير فقط)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    db.Database.EnsureCreated();
}

app.Run();