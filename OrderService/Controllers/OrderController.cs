using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Controllers;

[ApiController]
[Route("orders")]
public class OrderController : ControllerBase
{
    private readonly OrderDbContext _db;

    public OrderController(OrderDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.Orders.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Order order)
    {
        order.Id = Guid.NewGuid();
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        return Ok(order);
    }
} 
