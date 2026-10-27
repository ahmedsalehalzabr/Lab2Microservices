using System.Net.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// إضافة HttpClient للاتصال بالخدمات الداخلية
builder.Services.AddHttpClient();

var app = builder.Build();

app.MapGet("/manage/health", () => Results.Ok("Gateway is healthy"));

// ==========================================
// Endpoint سيناريو الشراء (Purchase)
// ==========================================
app.MapPost("/purchase/{userId:guid}/{itemId:guid}", async (Guid userId, Guid itemId, IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient();

    // 1. التحقق من وجود المستخدم
    var userResponse = await client.GetAsync($"http://user-service:8050/users/{userId}");
    if (!userResponse.IsSuccessStatusCode)
        return Results.BadRequest("User not found");

    // 2. جلب العنصر والتحقق من توفره
    // (بما أننا لم نضف GetById في Warehouse، سنفترض أن العنصر موجود للتبسيط، أو نضيف المسار لاحقاً)
    // للتبسيط سنرسل طلب إنشاء طلب (Order) مباشرة
    var orderData = new
    {
        UserId = userId,
        ItemId = itemId,
        Status = "Created"
    };

    // 3. إنشاء الطلب في OrderService
    var orderResponse = await client.PostAsJsonAsync("http://order-service:8060/orders", orderData);
    if (!orderResponse.IsSuccessStatusCode)
        return Results.BadRequest("Failed to create order");

    var order = await orderResponse.Content.ReadFromJsonAsync<object>();

    // 4. تحديث حالة العنصر في WarehouseService (كمثال مبسط)
    // في مشروع حقيقي، سنحتاج إلى Endpoint مثل PUT /warehouse/items/{id}/status
    // حالياً سنعيد النتيجة الناجحة مع بيانات الطلب

    return Results.Ok(new { Message = "Purchase successful", Order = order });
});

app.MapReverseProxy();

app.Run();